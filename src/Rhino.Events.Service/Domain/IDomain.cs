/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESSOURCES
 */
namespace Rhino.Events.Service.Domain
{
    /// <summary>
    /// Represents the interface for a domain class in the Rhino Events Service.
    /// </summary>
    public interface IDomain
    {
        /// <summary>
        /// Gets the repository for plugins associated with the domain.
        /// </summary>
        public IPluginsRepository Plugins { get; }
    }
}
