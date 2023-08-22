/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESSOURCES
 */
using Rhino.Events.Plugins.Attributes;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Rhino.Events.Service.Models
{
    /// <summary>
    /// Represents a cached model of a service event plugin.
    /// </summary>
    [DataContract]
    public class EventCacheModel
    {
        /// <summary>
        /// Gets or sets the key associated with the plugin.
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the type of the plugin.
        /// </summary>
        [DataMember, JsonIgnore]
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the attribute associated with the plugin.
        /// </summary>
        [DataMember]
        public ServiceEventAttribute Attribute { get; set; }
    }
}
