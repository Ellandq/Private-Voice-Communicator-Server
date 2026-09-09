using Communicator.Server.Application.Messaging;
using Communicator.Server.Infrastructure.Persistence;
using Communicator.Server.Realtime;
using Communicator.Server.Realtime.Connections;
using Communicator.Server.Realtime.Messaging;
using Communicator.Server.Realtime.Protocol;
using Communicator.Server.Realtime.Protocol.Handlers;
using Communicator.Server.Realtime.Protocol.Handlers.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Communicator.Server.Configuration;

public static class CommunicatorServerExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCommunicatorServer(IConfiguration configuration)
        {
            // ------------ DATABASE ----------------------------------
            services.AddDatabase(configuration);
        
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

        private IServiceCollection AddDatabase(IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("Database")
                ?? throw new InvalidOperationException(
                    "Database connection string is missing.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            return services;
        }
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