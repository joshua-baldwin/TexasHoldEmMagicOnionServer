using MagicOnion;
using MagicOnion.Server;
using TexasHoldEmServer.GameLogic;
using TexasHoldEmServer.ServerManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();       // Add this line(Grpc.AspNetCore)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        // WARN: Do not apply following policies to your production.
        //       If not configured carefully, it may cause security problems.
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();

        // NOTE: "grpc-status" and "grpc-message" headers are required by gRPC. so, we need expose these headers to the client.
        policy.WithExposedHeaders("grpc-status", "grpc-message");
    });
});
builder.Services.AddMagicOnion(); // Add this line(MagicOnion.Server)
builder.Services.AddSingleton<IServerManager>(new ServerManager());
builder.Services.AddSingleton(new GameLogicManager());
builder.WebHost
    .UseUrls("http://0.0.0.0:5137");

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/octet-stream",
    OnPrepareResponse = (ctx) =>
    {
        if (ctx.File.Name.EndsWith(".br"))
        {
            ctx.Context.Response.Headers.ContentEncoding = "br";
        }
        if (ctx.File.Name.Contains(".wasm")) ctx.Context.Response.Headers.ContentType = "application/wasm";
        if (ctx.File.Name.Contains(".js")) ctx.Context.Response.Headers.ContentType = "application/javascript";
    }
});
app.UseCors();
app.UseWebSockets();
app.UseGrpcWebSocketRequestRoutingEnabler();

app.UseRouting();

// NOTE: `UseGrpcWebSocketBridge` must be called after calling `UseRouting`.
app.UseGrpcWebSocketBridge();
app.MapMagicOnionService(); // Add this line
app.MapGet("/", () => "Hello World!");

app.Run();