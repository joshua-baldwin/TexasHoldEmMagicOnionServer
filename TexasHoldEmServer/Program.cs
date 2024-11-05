using MagicOnion;
using MagicOnion.Server;
using TexasHoldEmServer.ServerManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();       // Add this line(Grpc.AspNetCore)
builder.Services.AddMagicOnion(); // Add this line(MagicOnion.Server)
builder.Services.AddSingleton<IServerManager>(new ServerManager());

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.MapMagicOnionService(); // Add this line

app.Run();