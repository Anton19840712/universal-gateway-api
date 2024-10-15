using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Updated to use Controllers in .NET 8
builder.Services.AddRazorPages();  // Razor Pages added as part of MVC

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();

app.UseWebSockets();
app.Use(async (http, next) =>
{
    if (http.WebSockets.IsWebSocketRequest && http.Request.Path == "/ws")
    {
        var websocket = await http.WebSockets.AcceptWebSocketAsync();
        await Task.Run(async () =>
        {
            while (websocket.State == System.Net.WebSockets.WebSocketState.Open)
            {
                byte[] buffer = new byte[1024];
                var result = await websocket.ReceiveAsync(buffer, CancellationToken.None);

                await websocket.SendAsync(buffer, System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
            }
        });
    }
    else
    {
        await next();
    }
});

app.MapControllers(); // Mapping controllers in .NET 8

app.Run();

