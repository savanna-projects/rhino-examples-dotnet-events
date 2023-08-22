using Microsoft.Extensions.Options;

using System.Runtime.Serialization;
using System.Text.Json;

namespace Rhino.Events.Service.Models
{
    /// <summary>
    /// Contract for a generic error message.
    /// </summary>
    [DataContract]
    public class ErrorDetailsModel
    {
        /// <summary>
        /// Gets or sets the status code of this error.
        /// </summary>
        [DataMember]
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the message of this error.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the exception stack of this error.
        /// </summary>
        [DataMember]
        public string Stack { get; set; }

        /// <summary>
        /// Returns the JSON representation of this instance.
        /// </summary>
        /// <returns>The JSON representation of this instance.</returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });
        }

        /// <summary>
        /// Returns the JSON representation of this instance with the specified options.
        /// </summary>
        /// <param name="options">The JSON serialization options.</param>
        /// <returns>The JSON representation of this instance.</returns>
        public string ToString(JsonSerializerOptions options)
        {
            return JsonSerializer.Serialize(this, options);
        }
    }
}
