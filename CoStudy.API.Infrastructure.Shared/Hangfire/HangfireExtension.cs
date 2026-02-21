using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.Hangfire
{
    public static class HangfireExtension
    {
        public static void RegisterHangfireService(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddHangfire(x => x.UsePostgreSqlStorage(configuration["Settings:PostgresConnection"]));
            //services.AddHangfireServer();
            services.AddTransient<IHangfireService, HangfireService>();

            var mongoConnection = configuration["MongoConnectionString"];

            var migrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new DropMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            };
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                config.UseMongoStorage(mongoConnection, "Default", new MongoStorageOptions { MigrationOptions = migrationOptions, CheckConnection = false });
            });
            services.AddHangfireServer();
        }

        public static void RegisterHangfireApp(this IApplicationBuilder app, 
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {
            app.UseHangfireDashboard("/hangfire");

            recurringJobManager.AddOrUpdate(
                "Remove Violence Post",
                () => serviceProvider.GetService<IHangfireService>().RemoveViolencePost(),
                Cron.Minutely
                );

            recurringJobManager.AddOrUpdate(
                "Remove Violence Comment",
                () => serviceProvider.GetService<IHangfireService>().RemoveViolenceComment(),
                Cron.Minutely
                );

            recurringJobManager.AddOrUpdate(
               "Remove Violence Reply Comment",
               () => serviceProvider.GetService<IHangfireService>().RemoveViolenceReply(),
               Cron.Minutely
               );
        }
    }
}
    