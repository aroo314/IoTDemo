using IotDemo.Domain;
using IotDemo.Infrastructure;

namespace IotDemo.Endpoints;

public static class DevicesEndpoints
{
    public static void Map(WebApplication app)
    {
        // GET /api/devices
        app.MapGet("/api/devices",
            (InMemoryDevicesRepository devicesRepo,
             string? type,
             string? status,
             string? location,
             int? minBattery,
             int? maxBattery,
             string? search,
             int page = 1,
             int pageSize = 10) =>
            {
                var items = devicesRepo.GetAll().AsEnumerable();

                // type filter
                if (!string.IsNullOrWhiteSpace(type))
                {
                    var t = type.Trim().ToLowerInvariant();
                    DeviceType? map = t switch
                    {
                        "temperature" => DeviceType.TemperatureSensor,
                        "humidity" => DeviceType.HumiditySensor,
                        "multi" => DeviceType.MultiSensor,
                        "gateway" => DeviceType.Gateway,
                        "other" => DeviceType.Other,
                        _ => null
                    };
                    if (map is not null)
                        items = items.Where(d => d.Type == map.Value);
                }

                // status filter
                if (!string.IsNullOrWhiteSpace(status))
                {
                    var s = status.Trim().ToLowerInvariant();
                    DeviceStatus? map = s switch
                    {
                        "online" => DeviceStatus.Online,
                        "offline" => DeviceStatus.Offline,
                        "unknown" => DeviceStatus.Unknown,
                        _ => null
                    };
                    if (map is not null)
                        items = items.Where(d => d.Status == map.Value);
                }

                // location filter (case-insensitive equals)
                if (!string.IsNullOrWhiteSpace(location))
                {
                    var loc = location.Trim();
                    items = items.Where(d => string.Equals(d.Location, loc, StringComparison.OrdinalIgnoreCase));
                }

                // battery range
                if (minBattery is not null) items = items.Where(d => d.BatteryPercent >= minBattery.Value);
                if (maxBattery is not null) items = items.Where(d => d.BatteryPercent <= maxBattery.Value);

                // search by name (case-insensitive contains)
                if (!string.IsNullOrWhiteSpace(search))
                {
                    var q = search.Trim().ToLowerInvariant();
                    items = items.Where(d => (d.Name ?? string.Empty).ToLowerInvariant().Contains(q));
                }

                // pagination
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                var skip = (page - 1) * pageSize;
                var result = items.Skip(skip).Take(pageSize).ToList();

                return Results.Ok(result);
            })
            .WithName("GetDevices")
            .WithOpenApi(op =>
            {
                op.Summary = "List devices with optional filters and pagination";
                return op;
            });

        // GET /api/devices/{id}
        app.MapGet("/api/devices/{id}",
            (string id,
             bool includeLastMeasurement,
             InMemoryDevicesRepository devicesRepo,
             InMemoryMeasurementsRepository measurementsRepo) =>
            {
                var device = devicesRepo.GetById(id);
                if (device is null) return Results.NotFound();

                if (!includeLastMeasurement) return Results.Ok(device);

                var last = measurementsRepo.GetByDeviceId(id).FirstOrDefault();
                var dto = new DeviceWithLastMeasurement(device, last);
                return Results.Ok(dto);
            })
            .WithName("GetDeviceById")
            .WithOpenApi(op =>
            {
                op.Summary = "Get device by id; optionally include last measurement";
                return op;
            });

        // GET /api/locations/{location}/devices
        app.MapGet("/api/locations/{location}/devices",
            (string location,
             InMemoryDevicesRepository devicesRepo,
             int page = 1,
             int pageSize = 10) =>
            {
                var items = devicesRepo.GetAll()
                    .Where(d => string.Equals(d.Location, location, StringComparison.OrdinalIgnoreCase));

                var list = items.ToList();
                if (list.Count == 0) return Results.NotFound();

                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                var skip = (page - 1) * pageSize;
                var paged = list.Skip(skip).Take(pageSize).ToList();

                return Results.Ok(paged);
            })
            .WithName("GetDevicesByLocation")
            .WithOpenApi(op =>
            {
                op.Summary = "List devices in given location (case-insensitive) with pagination";
                return op;
            });
    }

    private record DeviceWithLastMeasurement(Device Device, Measurement? LastMeasurement);
}
