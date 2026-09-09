using Communicator.Server.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommunicatorServer(builder.Configuration);

var app = builder.Build();

app.UseCommunicatorServer();

app.Run();

namespace Communicator.Server
{
    public partial class Program;
}