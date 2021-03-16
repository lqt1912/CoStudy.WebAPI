using AspNetCoreRateLimit;
using AutoMapper;
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

namespace CoStudy.API.WebAPI
{
    /// <summary>
    /// Class Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
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
            app.UseRouting();

            app.UseSwaggerExtension();

            app.UseStaticFiles();

            app.UseMiddleware<JwtMiddleware>();
            app.UseErrorHandlingMiddleware();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



        }


    }
}
