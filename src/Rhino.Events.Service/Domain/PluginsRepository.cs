/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESSOURCES
 */
using Gravity.Abstraction.Logging;

using Rhino.Events.Plugins;
using Rhino.Events.Settings;

using System;

namespace Rhino.Events.Service.Domain
{
    public class PluginsRepository : IPluginsRepository
    {
        // Define a static logger instance using NewLogger method
        private readonly static ILogger s_logger = NewLogger();

        /// <summary>
        /// Creates a new instance of a plugin with the specified name.
        /// </summary>
        /// <param name="name">The name of the plugin to create.</param>
        /// <returns>A new instance of the <see cref="ServiceEventPlugin"/> or null if not found.</returns>
        public ServiceEventPlugin NewPlugin(string name)
        {
            // Log: Attempting to create a new plugin with the specified name
            s_logger.Info($"Attempting to create a new plugin with name: {name}");

            // Get the plugin type from the cache based on the provided name
            var type = CacheManager.PluginsCache[name].Type;

            if (type == null)
            {
                // Log: Plugin type not found in cache
                s_logger.Info($"Plugin type for name '{name}' not found in cache.");
                return null;
            }

            // Log: Creating a new instance of the plugin type
            s_logger.Info($"Creating a new instance of plugin type: {type.FullName}");

            try
            {
                // Create a new instance of the plugin type using Activator
                var plugin = Activator.CreateInstance(type) as ServiceEventPlugin;

                if (plugin == null)
                {
                    // Log: Unable to create an instance of the plugin
                    s_logger.Error($"Unable to create an instance of plugin type: {type.FullName}");
                }
                else
                {
                    // Log: Successfully created a new plugin instance
                    s_logger.Info($"Successfully created a new instance of plugin: {plugin.GetType().FullName}");
                }

                return plugin;
            }
            catch (Exception e) when(e != null)
            {
                s_logger.Fatal($"Unable to create an instance of plugin type: {e.GetBaseException()}");
                throw;
            }
        }

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
