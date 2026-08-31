using Communicator.Server;
using Communicator.Server.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommunicatorServer();

var app = builder.Build();

app.UseCommunicatorServer();

app.Run();

public partial class Program;