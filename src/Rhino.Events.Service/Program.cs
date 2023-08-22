/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESSOURCES
 */
using Gravity.Abstraction.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Rhino.Api.Converters;
using Rhino.Events.Service.Domain;
using Rhino.Events.Service.Formatters;
using Rhino.Events.Service.Middleware;
using Rhino.Events.Service.Models;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

#region *** Url & Kestrel ***
builder.WebHost.UseUrls();
#endregion

#region *** Service       ***
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRouting(i => i.LowercaseUrls = true);
builder.Services.AddResponseCompression(i => i.EnableForHttps = true);
builder.Services
    .AddControllers(i => i.InputFormatters.Add(new TextPlainInputFormatter()))
    .AddJsonOptions(i =>
    {
        i.JsonSerializerOptions.WriteIndented = true;
        i.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        i.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        i.JsonSerializerOptions.Converters.Add(new TypeConverter());
        i.JsonSerializerOptions.Converters.Add(new ExceptionConverter());
    });
builder.Services.AddSwaggerGen(i =>
{
    i.SwaggerDoc("v3", new OpenApiInfo { Title = "Rhino Events Service", Version = "v3" });
    i.OrderActionsBy(a => a.HttpMethod);
    i.EnableAnnotations();
});
builder.Services.AddApiVersioning(i =>
{
    i.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(3, 0);
    i.AssumeDefaultVersionWhenUnspecified = true;
    i.ErrorResponses = new GenericErrorModel<object>();
    i.ReportApiVersions = true;
});
builder.Services.Configure<CookiePolicyOptions>(i =>
{
    i.CheckConsentNeeded = _ => true;
    i.MinimumSameSitePolicy = SameSiteMode.None;
});
builder
    .Services
    .AddCors(i => i.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
#endregion

#region *** Dependencies  ***
EventsServiceDomain.SetDependencies(builder);
#endregion

#region *** Configuration ***
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

var logsPath = app
    .Configuration
    .GetValue("Rhino:ReportConfiguration:LogsOut", Environment.CurrentDirectory);

app.ConfigureExceptionHandler(new TraceLogger("Rhino.Events.Service", "ExceptionHandler", logsPath));
app.UseResponseCompression();
app.UseCookiePolicy();
app.UseCors("CorsPolicy");
app.UseSwagger();
app.UseSwaggerUI(i =>
{
    i.SwaggerEndpoint("/swagger/v3/swagger.json", "Rhino Controllers v3");
    i.DisplayRequestDuration();
    i.EnableFilter();
    i.EnableTryItOutByDefault();
});
#endregion

#region *** Cache         ***
CacheManager.SyncCache();
#endregion

// Map a GET request to the "api/v3/ping" endpoint
// Responds with a simple "Pong" message to indicate a successful API endpoint response.
app.MapGet("api/v3/ping", () => "Pong");

// Map a GET request to the "api/v3/actions" endpoint
// Retrieves the keys of plugins stored in the CacheManager.PluginsCache.
// Responds with the list of plugin keys as an HTTP response.
app.MapGet("api/v3/actions", () => CacheManager.PluginsCache.Keys);

// Map a GET request to the "api/v3/actions/{action}" endpoint with a parameter
// Retrieves plugin data associated with the specified action from CacheManager.PluginsCache.
// Responds with a 404 Not Found if the action is not found in the cache.
// Responds with a 200 OK and the plugin data if the action is found in the cache.
app.MapGet("api/v3/actions/{action}", (string action) =>
{
    var plugins = CacheManager.PluginsCache;

    // Check if the requested action exists in the plugins cache
    if (!plugins.ContainsKey(action))
    {
        // If the action does not exist, return a 404 Not Found response
        return Results.NotFound();
    }
    else
    {
        // If the action exists, return a 200 OK response with the corresponding plugin data
        var pluginData = plugins[action];
        return Results.Ok(pluginData);
    }
});

// Map a POST request to the "api/v3/events/run/{action}" endpoint
app.MapPost("api/v3/events/run/{action}", (IDomain domain, string action, TestRunEventModel eventModel) =>
{
    // Create a new plugin instance based on the provided action name
    var plugin = domain.Plugins.NewPlugin(name: action);

    // Invoke the plugin's action with the provided event model as arguments
    plugin.Invoke(eventArgumentsModel: eventModel);

    // Return OK response
    return Results.Ok();
});

app.Run();
