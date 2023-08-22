/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESSOURCES
 */
using Gravity.Abstraction.Logging;

using Rhino.Events.Plugins.Attributes;

namespace Rhino.Events.Plugins
{
    /// <summary>
    /// Represents a service event plugin for writing a log entry.
    /// </summary>
    [ServiceEvent(key: nameof(WriteLog), description: "Simple action to write a log entry.")]
    public class WriteLog : ServiceEventPlugin
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteLog"/> class.
        /// </summary>
        public WriteLog()
        {
            _logger = Logger.CreateChildLogger(loggerName: GetType().FullName);
        }

        /// <summary>
        /// Invoked when the service event is triggered.
        /// </summary>
        /// <param name="eventArgumentsModel">The model containing event arguments.</param>
        protected override void OnInvoke(object eventArgumentsModel)
        {
            _logger.Info("Foo Bar");  // Log "Foo Bar" as an informational message
        }
    }
}
