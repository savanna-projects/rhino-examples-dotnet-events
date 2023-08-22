/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESSOURCES
 */
using Rhino.Events.Plugins;

namespace Rhino.Events.Service.Domain
{
    /// <summary>
    /// Represents a repository for managing plugins within the service domain.
    /// </summary>
    public interface IPluginsRepository
    {
        /// <summary>
        /// Finds a plugin by its name.
        /// </summary>
        /// <param name="name">The name of the plugin to find.</param>
        /// <returns>The found <see cref="ServiceEventPlugin"/> or null if not found.</returns>
        ServiceEventPlugin NewPlugin(string name);
    }
}
