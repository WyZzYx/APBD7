using BusinessLogic.Services;
using DataAccess.Repositories;
using Domain;

var builder = WebApplication.CreateBuilder(args);

string cs = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddScoped<IDeviceRepository>(_ => new DeviceRepository(cs));
builder.Services.AddScoped<IDeviceService, DeviceService>();

var app = builder.Build();

app.MapGet("/api/devices", async (IDeviceService svc) =>
    await svc.GetAll());

app.MapGet("/api/devices/{id}", async (string id, IDeviceService svc) =>
{
    var d = await svc.GetById(id);
    return d is null ? Results.NotFound() : Results.Ok(d);
});

app.MapPost("/api/devices", async (Device device, IDeviceService svc) =>
{
    await svc.Create(device);
    return Results.Created($"/api/devices/{device.Id}", device);
});

app.MapDelete("/api/devices/{id}", async (string id, IDeviceService svc) =>
{
    await svc.Delete(id);
    return Results.NoContent();
});

app.Run();