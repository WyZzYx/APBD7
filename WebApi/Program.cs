using BusinessLogic.Services;
using DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
              ?? throw new InvalidOperationException("DefaultConnection missing");


builder.Services.AddTransient<IDeviceRepository>(sp =>
    new DeviceRepository(connStr));

builder.Services.AddTransient<IDeviceService, DeviceService>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.MapControllers();
app.Run();