using System.Net.Sockets;
using System.Text.Json;
using System.Text;
using api_receiver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/receive", async () =>
{
    using (var udpClient = new UdpClient(8080))
    {
        var result = await udpClient.ReceiveAsync();
        string json = Encoding.UTF8.GetString(result.Buffer);

        var model = JsonSerializer.Deserialize<Test>(json);
        Console.WriteLine($"Received: Name={model.Name}, Age={model.Age}");

        return Results.Ok("Data received and logged to console");
    }
});

app.Run();
