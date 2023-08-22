/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESSOURCES
 */
using Gravity.Abstraction.Logging;

using Rhino.Events.Settings;

using System;

namespace Rhino.Events.Plugins
{
    public abstract class ServiceEventPlugin
    {
        // Define a static logger instance using NewLogger method
        private static ILogger s_logger = NewLogger();

        /// <summary>
        /// Gets the logger instance for this class.
        /// </summary>
        public static ILogger Logger
        {
            get
            {
                // If logger is null, create a new instance using NewLogger method
                s_logger ??= NewLogger();
                return s_logger;
            }
        }

        /// <summary>
        /// Invokes the action with provided event arguments model.
        /// </summary>
        /// <param name="eventArgumentsModel">The event arguments model.</param>
        public void Invoke(object eventArgumentsModel)
        {
            // Log: Start of invoking the action
            Logger.Info($"Start of invoking action: {GetType().Name}");

            // Call the OnInvoke method to perform the action
            try
            {
                OnInvoke(eventArgumentsModel);
            }
            catch (Exception e) when (e != null)
            {
                // Log any exception that occurs during the action invocation
                Logger.Error($"Error occurred while invoking action {GetType().Name}: {e}");
            }

            // Log: Completion of the invoked action
            Logger.Info($"Action {GetType().Name} completed");
        }

        /// <summary>
        /// Method to be overridden by subclasses for implementing the action logic.
        /// </summary>
        /// <param name="eventArgumentsModel">The event arguments model.</param>
        protected abstract void OnInvoke(object eventArgumentsModel);

        // Creates a new instance of `ILogger` for the ServiceEventPlugin.
        private static ILogger NewLogger()
        {
            // Define constants for application and logger names
            const string applicationName = "Rhino.Events.Service";
            const string loggerName = "ServiceEventPlugin";

            // Get the log directory from application settings
            var logDirectory = new AppSettings().ReportsAndLogs.LogsOut;

            // Create a new instance of TraceLogger using the provided information
            return new TraceLogger(applicationName, loggerName, logDirectory);
        }
    }
}
