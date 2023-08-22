/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESSOURCES
 */
using System;
using System.Text.Json.Serialization;

namespace Rhino.Events.Plugins.Attributes
{
    /// <summary>
    /// Represents an attribute for marking a service event.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceEventAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceEventAttribute"/> class with the specified key and description.
        /// </summary>
        /// <param name="key">The key associated with the service event.</param>
        /// <param name="description">The description of the service event.</param>
        public ServiceEventAttribute(string key, string description)
        {
            Key = key;
            Description = description;
        }

        /// <summary>
        /// Gets or sets the key associated with the service event.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the description of the service event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Overrides the TypeId property to provide custom behavior.
        /// </summary>
        /// <remarks>This override allows for custom handling of the TypeId property.</remarks>
        [JsonIgnore]
        public override object TypeId => base.TypeId;
    }
}
