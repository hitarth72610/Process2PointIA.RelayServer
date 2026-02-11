using Process2PointIA.RelayServer.Models;
using Process2PointIA.RelayServer.Storage;

var builder = WebApplication.CreateBuilder(args);

// =============================================
// CLOUD + LOCAL COMPATIBLE PORT CONFIGURATION
// =============================================

// Render (and most cloud providers) provide PORT environment variable.
// If not present (local run), fallback to 8080.

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

// Listen on all network interfaces (0.0.0.0)
// Required for Docker + Cloud hosting
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

// =============================================
// BASIC HEALTH CHECK
// =============================================

app.MapGet("/", () => Results.Ok("Relay Server Online"));

// =============================================
// BOT → RELAY (Send Status)
// =============================================

app.MapPost("/api/bots/{botId}/status", (string botId, BotState state) =>
{
    state.BotId = botId;
    RelayStore.UpdateState(botId, state);
    return Results.Ok();
});

// =============================================
// CONTROLLER → RELAY (Get Status)
// =============================================

app.MapGet("/api/bots/{botId}/status", (string botId) =>
{
    var state = RelayStore.GetState(botId);
    return state is null ? Results.NotFound() : Results.Ok(state);
});

// =============================================
// CONTROLLER → RELAY (Send Command)
// =============================================

app.MapPost("/api/bots/{botId}/command/{command}", (string botId, string command) =>
{
    RelayStore.SetCommand(botId, command);
    return Results.Ok();
});

// =============================================
// BOT → RELAY (Fetch Command)
// =============================================

app.MapGet("/api/bots/{botId}/command", (string botId) =>
{
    var cmd = RelayStore.ConsumeCommand(botId);
    return cmd is null ? Results.NoContent() : Results.Ok(cmd);
});

app.Run();
