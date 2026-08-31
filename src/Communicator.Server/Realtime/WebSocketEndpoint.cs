namespace Communicator.Server.Realtime;

public sealed class WebSocketEndpoint(
    WebSocketHandler handler)
{
    public async Task HandleAsync(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 
                StatusCodes.Status400BadRequest;

            return;
        }

        using var webSocket =
            await context.WebSockets.AcceptWebSocketAsync();

        await handler.HandleAsync(
            webSocket,
            context.RequestAborted);
    }
}