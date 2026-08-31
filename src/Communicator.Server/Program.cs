using System.Net.WebSockets;
using System.Text;
using Communicator.Server.Realtime;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseWebSockets();

#region ENDPOINTS

app.MapGet("/", () => "Communicator server is running.");

app.Map("/ws", WebSocketHandler.HandleAsync);

#endregion

app.Run();

public partial class Program;