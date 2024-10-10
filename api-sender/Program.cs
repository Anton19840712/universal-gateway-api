using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using api_sender;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/send", async (Test model) =>
{
    string json = JsonSerializer.Serialize(model);
    byte[] data = Encoding.UTF8.GetBytes(json);

    using (var udpClient = new UdpClient())
    {
        var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        await udpClient.SendAsync(data, data.Length, endpoint);
    }

    return Results.Ok("Data sent over UDP");
});
app.UseHttpsRedirection();
app.Run();