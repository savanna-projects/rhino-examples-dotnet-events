/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESSOURCES
 */
using Gravity.Loader;

using Rhino.Events.Plugins.Attributes;
using Rhino.Events.Service.Models;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;

namespace Rhino.Events.Service.Domain
{
    /// <summary>
    /// Manages the caching of plugins within the service domain.
    /// </summary>
    public static class CacheManager
    {
        private static IDictionary<string, EventCacheModel> s_pluginsCache = FindPlugins();

        /// <summary>
        /// Gets the cache of plugins by their keys.
        /// </summary>
        public static IDictionary<string, EventCacheModel> PluginsCache
        {
            get
            {
                // If plugins cache is null, find and initialize plugins cache
                s_pluginsCache ??= FindPlugins();
                return s_pluginsCache;
            }
        }

        /// <summary>
        /// Synchronizes the application cache by displaying a loading animation while loading cache data.
        /// </summary>
        public static void SyncCache()
        {
            // Define a nested method for displaying the loading animation
            static void GetAnimation(int animationSpeed, CancellationToken token)
            {
                // Array of characters for the loading animation
                var loaderChars = new char[] { '|', '/', '-', '\\' };

                // Start a new task to display the loading animation
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        foreach (char c in loaderChars)
                        {
                            Console.Write(c);  // Display the current loading character
                            Thread.Sleep(animationSpeed);  // Pause for the specified animation speed
                            if (token.IsCancellationRequested)
                            {
                                return;  // Exit the task if cancellation is requested
                            }
                            try
                            {
                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);  // Move the cursor back to overwrite the character
                            }
                            catch (Exception e) when (e != null)
                            {
                                // Ignore exceptions related to setting cursor position
                            }
                        }
                    }
                }, token);
            }

            try
            {
                Console.Write("Loading Application Cache, Please Wait ");
                var tokenSource = new CancellationTokenSource();
                var stopwatch = new Stopwatch();

                stopwatch.Start();
                GetAnimation(animationSpeed: 75, token: tokenSource.Token);  // Display the loading animation

                var plugins = PluginsCache.Count;  // Get the count of cached entities
                stopwatch.Stop();
                tokenSource.Cancel();  // Cancel the animation task
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);  // Move the cursor back to clear the loading animation
                Console.Write("... Done!");

                Console.WriteLine();
                // Display the number of cached entities and the time taken for caching
                Console.WriteLine($"Total of {plugins} Entities Cached; Time (sec.): {stopwatch.ElapsedMilliseconds / 1000}");
                Console.WriteLine();
            }
            catch (Exception e) when (e != null)
            {
                Trace.TraceError($"{e}");  // Log the error
                Console.WriteLine($"Sync-Plugins = (InternalServerError | {e.Message})");  // Display an error message
            }
        }

        // Finds and initializes the plugins cache based on the attributes.
        private static IDictionary<string, EventCacheModel> FindPlugins()
        {
            // Get the executing assembly and its types
            var types = new AssembliesLoader().GetTypes();

            // Filter out types with ServiceEventAttribute and create a cache
            var plugins = types.Where(type => type.GetCustomAttribute<ServiceEventAttribute>() != null).ToArray();
            var cache = new ConcurrentDictionary<string, EventCacheModel>(StringComparer.OrdinalIgnoreCase);

            // Populate the cache with plugin types and their associated keys
            foreach (var plugin in plugins)
            {
                var attribute = plugin.GetCustomAttribute<ServiceEventAttribute>();
                var key = attribute.Key;
                cache[key] = new EventCacheModel
                {
                    Attribute = attribute,
                    Key = key,
                    Type = plugin
                };
            }

            // Get a dictionary of plugin keys and associated types
            return cache;
        }
    }
}
