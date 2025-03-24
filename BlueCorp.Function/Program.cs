using BlueCorpOrder.Application.AppInsightConfiguration;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueCorpOrder.Infrastructure.ThreePLServiceIntegration;
using BlueCorpOrder.Application.Extensions;
using BlueCorpOrder.Domain.RepositoryInterfaces;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        // Logging
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

            
        // Register Custom Services
        services.AddSingleton<IThreePLService, ThreePLService>();
       

        services.AddApplicationDependencies();
        services.ConfigureAppInsights();       
        services.AddSingleton<IReadyForDispatchRepository, IReadyForDispatchRepository>();

    })
    .Build();



host.Run();

await host.RunAsync();
