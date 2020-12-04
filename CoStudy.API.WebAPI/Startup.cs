using CoStudy.API.Application.Repositories;
using CoStudy.API.Infrastructure.Identity;
using CoStudy.API.Infrastructure.Identity.Contexts;
using CoStudy.API.Infrastructure.Identity.Helpers;
using CoStudy.API.Infrastructure.Identity.Services.Implements;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Extensions;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoStudy.API.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            //Config identity
            services.ConfigureIdentity();

            services.RegisterCustomRepository();
            services.RegisterCustomService();

            services.AddControllers();
            services.AddSwaggerExtension();
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureResponseCaching();
            services.ConfigureApiAuthentication();

            services.AddHttpContextAccessor();
            // configure strongly typed settings object

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // migrate database changes on startup (includes initial db creation)
           // context.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCustomAuthentication();
            app.UseSwaggerExtension();
            app.UseErrorHandlingMiddleware();
            app.UseStaticFiles();
            app.UseMiddleware(middleware: typeof(ErrorWrappingMiddleware));
            app.Use(async (context, next) =>
            {  // <----
                context.Request.EnableBuffering(); // or .EnableRewind();
                await next();
            });

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
