/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESSOURCES
 */
using Microsoft.AspNetCore.Mvc.Formatters;

using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Rhino.Events.Service.Formatters
{
    /// <summary>
    /// Custom input formatter to read text/plain from micro-service signature.
    /// </summary>
    public class TextPlainInputFormatter : InputFormatter
    {
        // Constants
        private const string ContentType = MediaTypeNames.Text.Plain;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextPlainInputFormatter"/> class.
        /// </summary>
        public TextPlainInputFormatter()
        {
            // Add supported media type
            SupportedMediaTypes.Add(ContentType);
        }

        /// <summary>
        /// Reads an object from the request body.
        /// </summary>
        /// <param name="context">The <see cref="InputFormatterContext"/>.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous read operation.</returns>
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            // Create a StreamReader to read the request body
            using var streamReader = new StreamReader(context.HttpContext.Request.Body);

            // Read the content asynchronously
            var request = await streamReader.ReadToEndAsync().ConfigureAwait(false);

            // Return the read content as a successful result
            return await InputFormatterResult.SuccessAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// Determines whether this input formatter can read the input.
        /// </summary>
        /// <param name="context">The <see cref="InputFormatterContext"/>.</param>
        /// <returns><c>true</c> if this input formatter can read the input; otherwise, <c>false</c>.</returns>
        public override bool CanRead(InputFormatterContext context)
        {
            // Get the content type from the request
            var contentType = context.HttpContext.Request.ContentType;

            // Check if the content type starts with the supported content type
            return contentType?.StartsWith(ContentType) == true;
        }
    }
}
