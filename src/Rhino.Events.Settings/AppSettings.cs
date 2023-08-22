/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESSOURCES
 */
using Microsoft.Extensions.Configuration;

using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Rhino.Events.Settings
{
    /// <summary>
    /// Represents the application settings configuration.
    /// </summary>
    public class AppSettings
    {
        // Default API version
        public const string ApiVersion = "3";

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class using default configuration sources.
        /// </summary>
        public AppSettings()
            : this(new ConfigurationBuilder()
                 .AddEnvironmentVariables()
                 .AddJsonFile("appsettings.json")
                 .AddJsonFile("appsettings.Development.json")
                 .Build())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class with the provided configuration.
        /// </summary>
        /// <param name="configuration">The configuration instance to use.</param>
        public AppSettings(IConfiguration configuration)
        {
            // Initialize configuration sections with default values
            ReportsAndLogs ??= new ReportConfiguration();

            // Assign the provided configuration instance
            Configuration = configuration;

            // Bind configuration sections to corresponding objects
            configuration.GetSection("Rhino:ReportConfiguration").Bind(ReportsAndLogs);
        }

        /// <summary>
        /// Gets the configuration instance used for AppSettings.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the configuration for report and logs settings.
        /// </summary>
        public ReportConfiguration ReportsAndLogs { get; }

        /// <summary>
        /// Gets the local IPv6 address.
        /// </summary>
        /// <returns>IPv6 Address.</returns>
        public static string GetLocalAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork))
            {
                if (!string.IsNullOrEmpty(ip.ToString()))
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Configuration for reporting and logs.
        /// </summary>
        public class ReportConfiguration
        {
            /// <summary>
            /// Gets or sets the logs output path.
            /// </summary>
            public string LogsOut { get; set; }
        }
    }
}