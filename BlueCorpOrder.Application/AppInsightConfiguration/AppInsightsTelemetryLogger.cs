using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Application.AppInsightConfiguration
{
    public class AppInsightsTelemetryLogger: ITelemetryLogger
    {
        private const string EnvironmentPropertyName = "Environment";

        private const string ApplicationPropertyName = "Application";

        private readonly TelemetryClient telemetryClient;

        public const string UnknownUser = "Unknown";

        public AppInsightsTelemetryLogger(TelemetryClient _telemetryClient)
        {
            telemetryClient = _telemetryClient;
        }

        public void TrackException(
            Exception exception,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null)
        {
            this.telemetryClient.TrackException(exception, properties, metrics);
        }

        public void TrackEvent(
            string eventName,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null)
        {
            this.telemetryClient.TrackEvent(eventName, properties, metrics);
        }

        public void TrackDependency(
            string dependencyName,
            string commandName,
            DateTimeOffset startTime,
            TimeSpan duration,
            bool success,
            IDictionary<string, string> properties = null)
        {
            var dependencyTelemetry = new DependencyTelemetry(dependencyName, commandName, startTime, duration, success);

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    dependencyTelemetry.Properties.Add(property);
                }
            }

            this.telemetryClient.TrackDependency(dependencyTelemetry);
        }

        public void TrackRequest(
            string requestPath,
            DateTimeOffset startTime,
            TimeSpan duration,
            bool success,
            int statusCode,
            IDictionary<string, string> properties = null)
        {
            var requestTelemetry = new RequestTelemetry(
                requestPath,
                startTime,
                duration,
                statusCode.ToString(),
                success);

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    requestTelemetry.Properties.Add(property);
                }
            }

            this.telemetryClient.TrackRequest(requestTelemetry);
        }
    }
}
