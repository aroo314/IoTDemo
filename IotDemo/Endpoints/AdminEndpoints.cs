using IotDemo.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace IotDemo.Endpoints;

public static class AdminEndpoints
{
    public static void Map(WebApplication app)
    {
        // POST /api/admin/reload-devices
        app.MapPost("/api/admin/reload-devices",
            (IDevicesSource source,
             InMemoryDevicesRepository devicesRepo,
             IConfiguration configuration) =>
            {
                var allow = configuration.GetValue<bool>("DeviceData:AllowDeviceReload");
                if (!allow) return Results.BadRequest(new { message = "Device reload is disabled by configuration." });

                var devices = source.LoadDevices();
                devicesRepo.SetDevices(devices);

                return Results.NoContent();
            })
            .WithName("ReloadDevices")
            .WithOpenApi(op =>
            {
                op.Summary = "Reload devices from configured file if enabled by configuration";
                return op;
            });
    }
}
