using AspNetCoreRateLimit;
using Audit.Core;
using Audit.WebApi;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Infrastructure.Identity;
using CoStudy.API.Infrastructure.Identity.Helpers;
using CoStudy.API.Infrastructure.Identity.Services.Implements;
using CoStudy.API.Infrastructure.Shared.AutoMapper;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.BackgroundTask.WorkerService;
using CoStudy.API.WebAPI.Extensions;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.EntityFrameworkCore;
using CoStudy.API.WebAPI.Models;
using Audit.PostgreSql.Configuration;

namespace CoStudy.API.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var writeLog = configuration["WriteLog"].ToString();

            if (writeLog == "Y")
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File("logs\\gateway_log.log", rollingInterval: RollingInterval.Day)
                    .CreateLogger();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseNpgsql(Configuration.GetValue<string>("Settings:PostgresConnection")));
            //Config identity
            services.ConfigureIdentity();

            services.AddOptions();
            services.AddMemoryCache();
            services.ConfigIpRateLimit(Configuration);

            services.RegisterCustomRepository();
            services.RegisterCustomService();

            services.AddControllers();
            services.AddSwaggerExtension();
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureResponseCaching();
            services.ConfigureApiAuthentication();

            services.AddHttpContextAccessor();

            services.AddSingleton<IWorker, Worker>();

            services.AddControllersWithViews(options =>
            {
                options.AddAuditFilter(config => config
                    .LogAllActions()
                    .WithEventType("{verb}.{controller}.{action}")
                    .IncludeHeaders(ctx => !ctx.ModelState.IsValid)
                    .IncludeRequestBody(d => (!d.ActionArguments.ContainsKey("Files") && !d.ActionArguments.ContainsKey("File")))
                    .IncludeModelState()
                    .IncludeResponseBody(ctx => ctx.HttpContext.Response.StatusCode == 200));
                options.Filters.Add(new AuditIgnoreActionFilter());
            });
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddCors();
            services.AddMvcCore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var sqlConnectionString = Configuration.GetValue<string>("Settings:PostgresConnection");
            Audit.Core.Configuration.Setup()
                .UsePostgreSql(config => config
                .ConnectionString(sqlConnectionString)
                .TableName("audit_log")
                .IdColumnName("id")
                .DataColumn("data", DataType.JSONB)
                .LastUpdatedColumnName("inserted_date"))
                .WithCreationPolicy(EventCreationPolicy.InsertOnStartReplaceOnEnd);

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIpRateLimiting();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSwaggerExtension();

            app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();
            app.UseErrorHandlingMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



        }

    }
}
