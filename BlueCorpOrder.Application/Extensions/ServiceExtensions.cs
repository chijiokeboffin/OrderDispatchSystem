using BlueCorpOrder.Application.AppInsightConfiguration;
using BlueCorpOrder.Application.Models;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            return services;
        }
        public static IServiceCollection ConfigureAppInsights(this IServiceCollection services)
        {
            ///Configure AppInsight
            const string EnvironmentPropertyName = "Environment";
            const string ApplicationPropertyName = "Application";

            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
            var application = Environment.GetEnvironmentVariable("APPLICATION");
            var instrumentationKey = Environment.GetEnvironmentVariable("INSTRUMENTKEY");

            var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            var telemetryClient = new TelemetryClient(telemetryConfiguration);
            telemetryClient.Context.GlobalProperties[EnvironmentPropertyName] = environment;
            telemetryClient.Context.GlobalProperties[ApplicationPropertyName] = application;

            if (string.IsNullOrWhiteSpace(telemetryClient.InstrumentationKey))
            {
                telemetryClient.InstrumentationKey = instrumentationKey;
            }

            services.AddSingleton(telemetryClient);


            services.AddSingleton<ITelemetryLogger, AppInsightsTelemetryLogger>((ctx) =>
            {
                return new AppInsightsTelemetryLogger(telemetryClient);
            });

            services.AddSingleton<IInsightsTracker, InsightsTracker>((ctx) =>
            {
                var svc = ctx.GetService<ITelemetryLogger>();
                return new InsightsTracker(svc);
            });
            return services;
        }
    }
}
