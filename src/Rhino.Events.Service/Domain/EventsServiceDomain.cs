/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESSOURCES
 */
using Gravity.Abstraction.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Rhino.Events.Settings;

namespace Rhino.Events.Service.Domain
{
    /// <summary>
    /// Represents the domain class for the Events Service.
    /// </summary>
    public class EventsServiceDomain : IDomain
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventsServiceDomain"/> class.
        /// </summary>
        /// <param name="plugins">The repository for plugins.</param>
        public EventsServiceDomain(IPluginsRepository plugins)
        {
            Plugins = plugins;
        }

        /// <summary>
        /// Gets the repository for plugins.
        /// </summary>
        public IPluginsRepository Plugins { get; }

        /// <summary>
        /// Initializes dependencies for the Events Service.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        public static void SetDependencies(WebApplicationBuilder builder)
        {
            // Create an instance of AppSettings
            var appSettings = new AppSettings();

            // Register the PluginsRepository as a transient service
            builder.Services.AddTransient<IPluginsRepository, PluginsRepository>();

            // Register the EventsServiceDomain as a transient service
            builder.Services.AddTransient<IDomain, EventsServiceDomain>();

            // Register the AppSettings as a singleton service
            builder.Services.AddSingleton(appSettings);

            // Register the TraceLogger as a singleton service for domain logging
            builder.Services.AddSingleton(new TraceLogger(
                applicationName: "Rhino.Events.Service",
                loggerName: "EventsServiceDomain",
                inDirectory: appSettings.ReportsAndLogs.LogsOut));
        }
    }
}
