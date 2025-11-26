using IotDemo.Endpoints;
using IotDemo.External;
using IotDemo.Infrastructure;
using IotDemo.Swagger;

var builder = WebApplication.CreateBuilder(args);

// DI
builder.Services.AddSingleton<InMemoryDevicesRepository>();
builder.Services.AddSingleton<InMemoryMeasurementsRepository>();
builder.Services.AddSingleton<ExternalDevicesLoader>();
builder.Services.AddSingleton<ExternalMeasurementsLoader>();
builder.Services.AddSingleton<IDevicesSource, FileDevicesSource>();

// Swagger
SwaggerConfig.Add(builder.Services);

var app = builder.Build();

// Swagger
SwaggerConfig.Use(app);

app.UseHttpsRedirection();

// Load data at startup
var devicesSource = app.Services.GetRequiredService<IDevicesSource>();
var devicesRepo = app.Services.GetRequiredService<InMemoryDevicesRepository>();
var measurementsRepo = app.Services.GetRequiredService<InMemoryMeasurementsRepository>();
var measurementsLoader = app.Services.GetRequiredService<ExternalMeasurementsLoader>();

var devices = devicesSource.LoadDevices();
devicesRepo.SetDevices(devices);

var measurements = measurementsLoader.LoadMeasurements();
measurementsRepo.SetMeasurements(measurements);

// Endpoints
DevicesEndpoints.Map(app);
MeasurementsEndpoints.Map(app);
StatsEndpoints.Map(app);
AdminEndpoints.Map(app);

app.Run();
