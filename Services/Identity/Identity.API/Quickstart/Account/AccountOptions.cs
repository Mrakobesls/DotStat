// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Identity.API.Quickstart.Account;

public class AccountOptions
{
    public static bool AllowLocalLogin = false;
    public static bool AllowRememberLogin = true;
    public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

    public static bool ShowLogoutPrompt = false;
    public static bool AutomaticRedirectAfterSignOut = true;

    public static string InvalidCredentialsErrorMessage = "Invalid username or password";
}