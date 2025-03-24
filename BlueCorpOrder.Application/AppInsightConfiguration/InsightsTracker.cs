using BlueCorpOrder.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Application.AppInsightConfiguration
{
    public class InsightsTracker: IInsightsTracker
    {
        public const string UnknownUser = "Unknown";

        private const string SourcePropertyName = "Source";
        private const string AreaPropertyName = "Area";
        private const string ErrorTypePropertyName = "Type";
        private const string ErrorCodePropertyName = "Code";
        private const string SeverityPropertyName = "SeverityLevel";
        private readonly ITelemetryLogger _telemetryLogger;
        public InsightsTracker(ITelemetryLogger telemetryLogger)
        {
            _telemetryLogger = telemetryLogger;
        }


        /// <summary>
        /// Send information about external dependency call in the application
        /// </summary>
        /// <param name="dependencyName">The name of the dependency</param>
        /// <param name="commandName">The executed command</param>
        /// <param name="startTime">The start time of the command</param>
        /// <param name="duration">the duration of the command</param>
        /// <param name="success">True if the command was executed successfully, otherwise false</param>
        /// <param name="properties">The additional properties used to track the dependency call</param>
        public void TrackDependency(string dependencyName, string commandName, DateTimeOffset startTime, TimeSpan duration, bool success, IDictionary<string, string> properties = null)
        {
            if (string.IsNullOrWhiteSpace(dependencyName))
            {
                return;
            }

            Task.Run(() => _telemetryLogger.TrackDependency(dependencyName, commandName, startTime, duration, success, properties));
        }

        /// <summary>
        /// Tracks a custom event in the system. 
        /// </summary>
        /// <param name="eventName">The name of the event</param>
        /// <param name="properties">The properties to describe the event</param>
        /// <param name="metrics">related to the event</param>
        public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                return;
            }

            Task.Run(() => _telemetryLogger.TrackEvent(eventName, properties, metrics));
        }

        /// <summary>
        /// Tracks a custom event in the system. 
        /// </summary>
        /// <param name="exception">The exception to log</param>
        /// <param name="source">The source from where it came</param>
        /// <param name="area">The area where it happened</param>
        /// <param name="type">The type of error</param>
        /// <param name="code">The error code</param>
        /// <param name="severity">The severity level. Critical requires immediate attention</param>

        public void TrackException(Exception exception, string source = null, string area = null, string type = null, string code = null, ExceptionSeverity severity = ExceptionSeverity.Error)
        {
            if (exception == null)
            {
                return;
            }

            var properties = new Dictionary<string, string>
                                 {
                                     { SeverityPropertyName, severity.ToString() }
                                 };

            AddIfExist(SourcePropertyName, source, properties);
            AddIfExist(AreaPropertyName, area, properties);
            AddIfExist(ErrorTypePropertyName, type, properties);
            AddIfExist(ErrorCodePropertyName, code, properties);

            if (properties.Any() == false)
            {
                properties = null;
            }

            Task.Run(() => _telemetryLogger.TrackException(exception, properties));
        }

        /// <summary>
        /// Tracks information about a request
        /// </summary>
        /// <param name="requestPath">The request path</param>
        /// <param name="startTime">The start time of the request</param>
        /// <param name="duration">The duration of the request</param>
        /// <param name="success">Whether or not the request was successful</param>
        /// <param name="statusCode">The status code of the response</param>
        /// <param name="properties">The properties associated with the request</param>
        public void TrackRequest(string requestPath, DateTimeOffset startTime, TimeSpan duration, bool success, int statusCode, IDictionary<string, string> properties = null)
        {
            if (string.IsNullOrWhiteSpace(requestPath))
            {
                return;
            }

            Task.Run(() => _telemetryLogger.TrackRequest(requestPath, startTime, duration, success, statusCode, properties));
        }

        public async Task<string> GetRequestBodyAsync(Stream stream)
        {
            using var reader = new StreamReader(stream);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            return await reader.ReadToEndAsync();
        }
        private static void AddIfExist(string propertyName, string value, IDictionary<string, string> properties)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            if (value != null && !properties.ContainsKey(propertyName))
            {
                properties.Add(propertyName, value);
            }
        }
        public void TrackEventToInsights(string eventName, string request, string response)
        {
            var properties = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(request))
            {
                properties.Add(nameof(request), request);
            }

            if (!string.IsNullOrEmpty(response))
            {
                properties.Add(nameof(response), response);
            }

            TrackEvent(eventName, properties);
        }
    }
}
