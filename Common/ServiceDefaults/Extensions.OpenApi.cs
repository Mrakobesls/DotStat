﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ServiceDefaults;

public static partial class Extensions
{
    public static IHostApplicationBuilder AddDefaultOpenApi(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        var openApi = configuration.GetSection("OpenApi");

        if (!openApi.Exists())
        {
            return builder;
        }

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(
            options =>
            {
                // {
                //   "OpenApi": {
                //     "Document": {
                //         "Title": ..
                //         "Version": ..
                //         "Description": ..
                //     }
                //   }
                // }
                var document = openApi.GetRequiredSection("Document");

                var version = document.GetRequiredValue("Version") ?? "v1";

                options.SwaggerDoc(
                    version,
                    new OpenApiInfo
                    {
                        Title = document.GetRequiredValue("Title"),
                        Version = version,
                        Description = document.GetValue<string>("Description") ?? ""
                    }
                );

                // TODO no need for now
                return;

                var identitySection = configuration.GetSection("Identity");

                if (!identitySection.Exists())
                {
                    // No identity section, so no authentication open api definition
                    return;
                }

                // {
                //   "Identity": {
                //     "Url": "http://identity",
                //     "Scopes": {
                //         "statistics": "Statistics API"
                //      }
                //    }
                // }

                var identityUrlExternal = identitySection.GetRequiredValue("Url");
                var scopes = identitySection.GetRequiredSection("Scopes").GetChildren().ToDictionary(p => p.Key, p => p.Value);

                options.AddSecurityDefinition(
                    "oauth2",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            // TODO: Change this to use Authorization Code flow with PKCE
                            Implicit = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri($"{identityUrlExternal}/connect/authorize"),
                                TokenUrl = new Uri($"{identityUrlExternal}/connect/token"),
                                Scopes = scopes,
                            }
                        }
                    }
                );

                options.OperationFilter<AuthorizeCheckOperationFilter>([scopes.Keys.ToArray()]);
            }
        );

        return builder;
    }

    private static WebApplication UseDefaultOpenApi(this WebApplication app)
    {
        var configuration = app.Configuration;
        var openApiSection = configuration.GetSection("OpenApi");

        if (!openApiSection.Exists() || !app.Environment.IsDevelopment())
        {
            return app;
        }

        app.UseSwagger();
        app.UseSwaggerUI(
            setup =>
            {
                // {
                //   "OpenApi": {
                //     "Endpoint": {
                //         "Name":
                //     },
                //     "Auth": {
                //         "ClientId": ..,
                //         "AppName": ..
                //     }
                //   }
                // }

                var pathBase = configuration["PATH_BASE"];
                var authSection = openApiSection.GetSection("Auth");
                var endpointSection = openApiSection.GetRequiredSection("Endpoint");
                var version = openApiSection.GetRequiredValue("Document:Version");

                var swaggerUrl = endpointSection["Url"]
                    ?? $"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}/swagger/{version}/swagger.json";

                setup.SwaggerEndpoint(swaggerUrl, endpointSection.GetRequiredValue("Name"));

                if (authSection.Exists())
                {
                    setup.OAuthClientId(authSection.GetRequiredValue("ClientId"));
                    setup.OAuthAppName(authSection.GetRequiredValue("AppName"));
                }
            }
        );

        // Add a redirect from the root of the app to the swagger endpoint
        app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

        return app;
    }

    private sealed class AuthorizeCheckOperationFilter(string[] scopes) : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var metadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;

            if (!metadata.OfType<IAuthorizeData>().Any())
            {
                return;
            }

            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    [oAuthScheme] = scopes
                }
            };
        }
    }
}
