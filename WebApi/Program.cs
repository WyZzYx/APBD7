using System.Text.Json;
using BusinessLogic.Services;
using DataAccess.Repositories;
using Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDeviceRepository>(
    _ => new DeviceRepository(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDeviceService, DeviceService>();

var app = builder.Build();

app.MapGet("/api/devices", async (IDeviceService svc) =>
    Results.Ok(await svc.GetAll())
);

app.MapGet("/api/devices/{id}", async (string id, IDeviceService svc) => {
    var d = await svc.GetById(id);
    return d is not null ? Results.Ok(d) : Results.NotFound();
});

app.MapPost("/api/devices", async (HttpRequest req, IDeviceService service) =>
    {
        var doc  = await JsonDocument.ParseAsync(req.Body);
        var root = doc.RootElement;

        string deviceType = root.GetProperty("deviceType").GetString()!;

        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };

        Device device = deviceType switch
        {
            "PC"         => JsonSerializer.Deserialize<PCDevice>(root.GetRawText(), options)!,
            "Embedded"   => JsonSerializer.Deserialize<EmbeddedDevice>(root.GetRawText(), options)!,
            "SmartWatch" => JsonSerializer.Deserialize<Smartwatch>(root.GetRawText(), options)!,
            _ => throw new ArgumentException($"Unknown deviceType '{deviceType}'")
        };

        await service.Create(device);
        return Results.Created($"/api/devices/{device.Id}", device);
    })
    .Accepts<JsonElement>("application/json");

app.MapPut("/api/devices/{id}", async (string id, Device? d, IDeviceService svc) => {
    if (id != d.Id) return Results.BadRequest("ID mismatch");
    try {
        await svc.Update(d);
        return Results.NoContent();
    } catch (KeyNotFoundException) {
        return Results.NotFound();
    } catch (ArgumentException e) {
        return Results.BadRequest(e.Message);
    }
});

app.MapDelete("/api/devices/{id}", async (string id, IDeviceService svc) => {
    await svc.Delete(id);
    return Results.NoContent();
});

app.Run();