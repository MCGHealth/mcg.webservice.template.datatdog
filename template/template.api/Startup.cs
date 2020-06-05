using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using template.Api.DataAccess;
using template.Api.Infrastructure;
using template.Api.Infrastructure.Configuration;
using template.Api.Infrastructure.HealthChecks;
using template.Api.Infrastructure.Tracing;
using template.Api.Services;

namespace template.Api
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddHttpContextAccessor();

            services.AddSingleton<IAppSettings, AppSettings>();
            services.AddSingleton<IExampleDataRepository, UserDataRepository>();
            services.AddTransient<IExampleBusinessService, UserBusinessService>();

            services.AddServiceHealthChecks();
            services.AddDistributedTracing();
            services.AddSwaggerDocumentation();
        }

        public void Configure(IApplicationBuilder app)
        {
            AspectFactory.Settings = app.ApplicationServices.GetService<IAppSettings>();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerDocumentation();
            app.UseServiceHealthChecks();

<<<<<<< HEAD:template/Mcg.Webservice.Api/Startup.cs
            //--> see https://github.com/prometheus-net/prometheus-net/blob/master/README.md for more details
            app.UseMetricServer(url: "/ops/metrics");

=======
>>>>>>> feature/refinement:template/template.api/Startup.cs
            app.UseCors(options =>
            {
                /************************************************************************
                 * WARNING!!! The development configuration is set to '*' and should NOT
                 *            be allowed to go into higher environments with the setting!
                 ************************************************************************/
                options.WithOrigins(Configuration["CORS_ALLOWED_URLS"].Split(','));
                options.SetIsOriginAllowedToAllowWildcardSubdomains();
                options.AllowAnyMethod();
            });
        }
    }
}
