using Communicator.Server.Realtime;
using Communicator.Server.Realtime.Connections;
using Communicator.Server.Realtime.Messaging;

var builder = WebApplication.CreateBuilder(args);

#region DEPENDENCY INJECTION REGISTRATION

builder.Services.AddSingleton<ConnectionManager>();
builder.Services.AddSingleton<WebSocketMessageReader>();

builder.Services.AddTransient<WebSocketHandler>();
builder.Services.AddTransient<WebSocketEndpoint>();

#endregion

var app = builder.Build();

app.UseWebSockets();

#region ENDPOINTS

app.MapGet("/", () => "Communicator server is running.");

app.Map("/ws", async context =>
{
    var endpoint = context.RequestServices
        .GetRequiredService<WebSocketEndpoint>();

    await endpoint.HandleAsync(context);
});

#endregion

app.Run();

public partial class Program;