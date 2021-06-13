using AspNetCoreRateLimit;
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

            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddCors();
            services.AddMvcCore();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


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
