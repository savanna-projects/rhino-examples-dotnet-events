/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESSOURCES
 */
using Gravity.Abstraction.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

using Rhino.Events.Service.Models;

using System;
using System.Net;
using System.Threading.Tasks;

namespace Rhino.Events.Service.Middleware
{
    /// <summary>
    /// Extensions package for error handling middleware.
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Configure global exceptions handler.
        /// </summary>
        /// <param name="app">Defines a class that provides the mechanisms to configure an application's request pipeline.</param>
        /// <param name="logger">Logger implementation to use with this middleware.</param>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError => AppError(appError, logger));
        }

        private static void AppError(IApplicationBuilder appError, ILogger logger)
        {
            appError.Run(async context => await RunContextAsync(context, logger).ConfigureAwait(false));
        }

        private static async Task RunContextAsync(HttpContext context, ILogger logger)
        {
            // Setup the response details for error handling
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

            // Exit conditions if no exception context is present
            if (contextFeature == null)
            {
                return;
            }

            // Log the error using the provided logger
            logger.Fatal($"{Environment.NewLine}Something went wrong", contextFeature.Error);

            // Create and populate error details for the response
            var errorDetails = new ErrorDetailsModel()
            {
                StatusCode = context.Response.StatusCode,
                Message = contextFeature.Error.Message,
                Stack = $"{contextFeature.Error.GetBaseException()}"
            };

            // Write the error details to the response
            await context.Response.WriteAsync(errorDetails.ToString()).ConfigureAwait(false);
        }
    }
}
