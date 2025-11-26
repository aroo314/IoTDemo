using System.Text.Json;
using IotDemo.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IotDemo.External;

public class ExternalDevicesLoader
{
    private readonly ILogger<ExternalDevicesLoader> _logger;
    private readonly string _filePath;

    public ExternalDevicesLoader(ILogger<ExternalDevicesLoader> logger, IConfiguration configuration)
    {
        _logger = logger;
        _filePath = configuration["DeviceData:DevicesFilePath"] ?? string.Empty;
    }

    public IReadOnlyList<Device> LoadDevices()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_filePath))
            {
                _logger.LogError("DeviceData:DevicesFilePath is not configured.");
                return Array.Empty<Device>();
            }

            if (!File.Exists(_filePath))
            {
                _logger.LogError("Devices file not found at path: {Path}", _filePath);
                return Array.Empty<Device>();
            }

            var json = File.ReadAllText(_filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var raws = JsonSerializer.Deserialize<List<DeviceRaw>>(json, options) ?? new List<DeviceRaw>();
            var devices = new List<Device>(raws.Count);

            foreach (var r in raws)
            {
                if (r == null) continue;

                var location = string.IsNullOrWhiteSpace(r.Loc) ? "unknown" : r.Loc.Trim();
                if (location == "unknown")
                {
                    _logger.LogWarning("Device {Id} has empty location. Using 'unknown'.", r.Id);
                }

                var device = new Device
                {
                    Id = r.Id ?? string.Empty,
                    Name = r.Device_Name ?? string.Empty,
                    Type = MapType(r.Type),
                    Location = location,
                    BatteryPercent = SafeInt(r.Battery),
                    FirmwareVersion = r.Fw_Ver ?? string.Empty,
                    Status = MapStatus(r.Status),
                    LastSeenUtc = r.Last_Seen_Utc
                };

                // Basic validation: ensure Id and Name exist
                if (string.IsNullOrWhiteSpace(device.Id) || string.IsNullOrWhiteSpace(device.Name))
                {
                    _logger.LogWarning("Invalid device record encountered. Id or Name missing. Skipping.");
                    continue;
                }

                devices.Add(device);
            }

            return devices;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load devices from {Path}", _filePath);
            return Array.Empty<Device>();
        }
    }

    private static int SafeInt(int value) => value < 0 ? 0 : value;

    private static DeviceType MapType(string? type)
    {
        if (string.IsNullOrWhiteSpace(type)) return DeviceType.Other;
        switch (type.Trim().ToLowerInvariant())
        {
            case "sensor_temp": return DeviceType.TemperatureSensor;
            case "sensor_humidity": return DeviceType.HumiditySensor;
            case "sensor_multi": return DeviceType.MultiSensor;
            case "gateway": return DeviceType.Gateway;
            default: return DeviceType.Other;
        }
    }

    private static DeviceStatus MapStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status)) return DeviceStatus.Unknown;
        switch (status.Trim().ToLowerInvariant())
        {
            case "online": return DeviceStatus.Online;
            case "offline": return DeviceStatus.Offline;
            default: return DeviceStatus.Unknown;
        }
    }
}
