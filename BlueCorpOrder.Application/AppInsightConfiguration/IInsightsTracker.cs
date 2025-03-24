using BlueCorpOrder.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Application.AppInsightConfiguration
{
    public interface IInsightsTracker
    {
        // <summary>
        /// Tracks a custom event in the system. 
        /// </summary>
        /// <param name="exception">The exception to log</param>
        /// <param name="source">The source from where it came</param>
        /// <param name="area">The area where it happened</param>
        /// <param name="type">The type of error</param>
        /// <param name="code">The error code</param>
        /// <param name="severity">The severity level. Critical requires immediate attention</param>
        void TrackException(
            Exception exception,
            string source = null,
            string area = null,
            string type = null,
            string code = null,
            ExceptionSeverity severity = ExceptionSeverity.Error);

        /// <summary>
        /// Tracks a custom event in the system. 
        /// </summary>
        /// <param name="eventName">The name of the event</param>
        /// <param name="properties">The properties to describe the event</param>
        /// <param name="metrics">related to the event</param>
        void TrackEvent(
            string eventName,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null);

        /// <summary>
        /// Send information about external dependency call in the application
        /// </summary>
        /// <param name="dependencyName">The name of the dependency</param>
        /// <param name="commandName">The executed command</param>
        /// <param name="startTime">The start time of the command</param>
        /// <param name="duration">the duration of the command</param>
        /// <param name="success">True if the command was executed successfully, otherwise false</param>
        /// <param name="properties">The additional properties used to track the dependency call</param>
        void TrackDependency(
            string dependencyName,
            string commandName,
            DateTimeOffset startTime,
            TimeSpan duration,
            bool success,
            IDictionary<string, string> properties = null);

        /// <summary>
        /// Tracks information about a request
        /// </summary>
        /// <param name="requestPath">The request path</param>
        /// <param name="startTime">The start time of the request</param>
        /// <param name="duration">The duration of the request</param>
        /// <param name="success">Whether or not the request was successful</param>
        /// <param name="statusCode">The status code of the response</param>
        /// <param name="properties">The properties associated with the request</param>
        void TrackRequest(
            string requestPath,
            DateTimeOffset startTime,
            TimeSpan duration,
            bool success,
            int statusCode,
            IDictionary<string, string> properties = null);

        Task<string> GetRequestBodyAsync(Stream stream);
        public void TrackEventToInsights(string eventName, string request, string response);
    }
}
