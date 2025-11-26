using IotDemo.Infrastructure;

namespace IotDemo.Endpoints;

public static class MeasurementsEndpoints
{
    public static void Map(WebApplication app)
    {
        // GET /api/devices/{id}/measurements
        app.MapGet("/api/devices/{id}/measurements",
            (string id,
             DateTime? from,
             DateTime? to,
             string? order,
             int? limit,
             InMemoryDevicesRepository devicesRepo,
             InMemoryMeasurementsRepository measurementsRepo) =>
            {
                var device = devicesRepo.GetById(id);
                if (device is null) return Results.NotFound();

                var items = measurementsRepo.GetByDeviceId(id).AsEnumerable();

                if (from is not null) items = items.Where(m => m.TimestampUtc >= from.Value);
                if (to is not null) items = items.Where(m => m.TimestampUtc <= to.Value);

                var ord = string.IsNullOrWhiteSpace(order) ? "desc" : order.Trim().ToLowerInvariant();
                items = ord == "asc"
                    ? items.OrderBy(m => m.TimestampUtc)
                    : items.OrderByDescending(m => m.TimestampUtc);

                var lim = limit ?? 100;
                if (lim < 1) lim = 100;
                var result = items.Take(lim).ToList();

                return Results.Ok(result);
            })
            .WithName("GetDeviceMeasurements")
            .WithOpenApi(op =>
            {
                op.Summary = "List measurements for a device with optional time range, order and limit";
                return op;
            });
    }
}
