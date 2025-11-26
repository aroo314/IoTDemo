using IotDemo.Domain;
using IotDemo.Infrastructure;

namespace IotDemo.Endpoints;

public static class StatsEndpoints
{
    public static void Map(WebApplication app)
    {
        // GET /api/stats
        app.MapGet("/api/stats",
            (InMemoryDevicesRepository devicesRepo, InMemoryMeasurementsRepository measurementsRepo) =>
            {
                var devices = devicesRepo.GetAll();
                var total = devices.Count;

                var online = devices.Count(d => d.Status == DeviceStatus.Online);
                var offline = devices.Count(d => d.Status == DeviceStatus.Offline);
                var unknown = devices.Count(d => d.Status == DeviceStatus.Unknown);

                var types = new
                {
                    TemperatureSensor = devices.Count(d => d.Type == DeviceType.TemperatureSensor),
                    HumiditySensor = devices.Count(d => d.Type == DeviceType.HumiditySensor),
                    MultiSensor = devices.Count(d => d.Type == DeviceType.MultiSensor),
                    Gateway = devices.Count(d => d.Type == DeviceType.Gateway),
                    Other = devices.Count(d => d.Type == DeviceType.Other)
                };

                // basic aggregates for measurements
                var allMeasurements = devices
                    .SelectMany(d => measurementsRepo.GetByDeviceId(d.Id))
                    .ToList();

                var tempValues = allMeasurements.Where(m => m.TemperatureC.HasValue).Select(m => m.TemperatureC!.Value).ToList();
                var humValues = allMeasurements.Where(m => m.HumidityPercent.HasValue).Select(m => m.HumidityPercent!.Value).ToList();

                var measurementsStats = new
                {
                    Count = allMeasurements.Count,
                    Temperature = new
                    {
                        Min = tempValues.Count > 0 ? tempValues.Min() : (double?)null,
                        Max = tempValues.Count > 0 ? tempValues.Max() : (double?)null,
                        Avg = tempValues.Count > 0 ? tempValues.Average() : (double?)null
                    },
                    Humidity = new
                    {
                        Min = humValues.Count > 0 ? humValues.Min() : (double?)null,
                        Max = humValues.Count > 0 ? humValues.Max() : (double?)null,
                        Avg = humValues.Count > 0 ? humValues.Average() : (double?)null
                    }
                };

                var result = new
                {
                    Devices = new
                    {
                        Total = total,
                        Online = online,
                        Offline = offline,
                        Unknown = unknown,
                        Types = types
                    },
                    Measurements = measurementsStats
                };

                return Results.Ok(result);
            })
            .WithName("GetStats")
            .WithOpenApi(op =>
            {
                op.Summary = "Return basic stats for devices and measurements";
                return op;
            });
    }
}
