using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseWebSockets();

#region ENDPOINTS

app.MapGet("/", () => "Communicator server is running.");

#endregion

app.Run();