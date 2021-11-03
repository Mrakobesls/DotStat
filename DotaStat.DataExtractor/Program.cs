using DB_Repositories;
using DB_UnitOfWork;
using DB_UnitOfWork.Inteface;
using DotaStat.Business;
using DotaStat.Business.Interfaces;
using DotaStat.Data.EntityFramework;
using DotaStat.DataExtractor.Quartz;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace DotaStat.DataExtractor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<DotaStatDbContext>();
                    services.AddSingleton<IUnitOfWork, UnitOfWork>();
                    services.AddSingleton<CurrentWinrateAllyRepository>();
                    services.AddSingleton<CurrentWinrateEnemyRepository>();
                    services.AddSingleton<IHeroStatisticsService, HeroStatisticsService>();
                    services.AddSingleton<IWeekPatchService, WeekPatchService>();
                    services.AddSingleton<IHeroService, HeroService>();
                    services.AddHostedService<Worker>();

                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionJobFactory();

                        var jobKey = new JobKey("FetcherScheduler");
                        q.AddJob<Fetcher>(opts => opts.WithIdentity(jobKey));
                        q.AddTrigger(opts => opts
                            .ForJob(jobKey)
                            .WithIdentity("FetcherScheduler-trigger")
                            .WithCronSchedule(hostContext.Configuration["Quartz:FetcherScheduler"]));
                    });
                    services.AddQuartzHostedService(
                        q => q.WaitForJobsToComplete = true);
                });
    }
}