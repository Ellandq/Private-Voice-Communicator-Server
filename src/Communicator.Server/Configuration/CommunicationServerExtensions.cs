using Communicator.Server.Application.Messaging;
using Communicator.Server.Realtime;
using Communicator.Server.Realtime.Connections;
using Communicator.Server.Realtime.Messaging;
using Communicator.Server.Realtime.Protocol;
using Communicator.Server.Realtime.Protocol.Handlers;
using Communicator.Server.Realtime.Protocol.Handlers.Implementations;

namespace Communicator.Server.Configuration;

public static class CommunicatorServerExtensions
{
    public static IServiceCollection AddCommunicatorServer(
        this IServiceCollection services)
    {
        // ------------ SINGLETONS --------------------------------
        services.AddSingleton<ConnectionManager>();
        services.AddSingleton<WebSocketMessageReader>();
        services.AddSingleton<MessageDeserializer>();
        services.AddSingleton<MessageDispatcher>();
        services.AddSingleton<IMessageReceiver, MessageReceiver>();

        // ------------ TRANSIENT ---------------------------------
        services.AddTransient<WebSocketHandler>();
        services.AddTransient<WebSocketEndpoint>();

        // ------------ TRANSIENT - MESSAGE HANDLERS --------------
        services.AddTransient<IRealtimeMessageHandler, SendMessageHandler>();

        return services;
    }

    public static WebApplication UseCommunicatorServer(
        this WebApplication app)
    {
        app.UseWebSockets();

        app.MapGet(
            "/",
            () => "Communicator server is running.");

        app.Map(
            "/ws",
            async context =>
            {
                var endpoint = context.RequestServices
                    .GetRequiredService<WebSocketEndpoint>();

                await endpoint.HandleAsync(context);
            });

        return app;
    }
}