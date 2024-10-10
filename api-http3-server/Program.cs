using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Добавьте необходимые сервисы
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 443;
});

// Настройка Kestrel для поддержки QUIC
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Any, 443, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http3;
        // Настройка сертификата, если требуется
        listenOptions.UseHttps("path/to/certificate.pfx", "password");
    });
});

var app = builder.Build();

// Настройка HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.Run();
