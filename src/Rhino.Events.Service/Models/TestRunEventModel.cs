/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESSOURCES
 */
using Rhino.Api.Contracts.AutomationProvider;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhino.Events.Service.Models
{
    /// <summary>
    /// Represents a data contract for a test run event model.
    /// </summary>
    [DataContract]
    public class TestRunEventModel
    {
        /// <summary>
        /// Gets or sets the RhinoTestRun associated with the event.
        /// </summary>
        [DataMember]
        public RhinoTestRun TestRun { get; set; }

        /// <summary>
        /// Gets or sets the environment data associated with the event.
        /// </summary>
        [DataMember]
        public IDictionary<string, object> Environment { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets or sets additional properties associated with the event.
        /// </summary>
        [DataMember]
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    }
}
