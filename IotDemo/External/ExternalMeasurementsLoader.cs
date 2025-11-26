using System.Text.Json;
using IotDemo.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IotDemo.External;

public class ExternalMeasurementsLoader
{
    private readonly ILogger<ExternalMeasurementsLoader> _logger;
    private readonly string _filePath;

    public ExternalMeasurementsLoader(ILogger<ExternalMeasurementsLoader> logger, IConfiguration configuration)
    {
        _logger = logger;
        _filePath = configuration["MeasurementData:MeasurementsFilePath"] ?? string.Empty;
    }

    public IReadOnlyList<Measurement> LoadMeasurements()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_filePath))
            {
                _logger.LogError("MeasurementData:MeasurementsFilePath is not configured.");
                return Array.Empty<Measurement>();
            }

            if (!File.Exists(_filePath))
            {
                _logger.LogError("Measurements file not found at path: {Path}", _filePath);
                return Array.Empty<Measurement>();
            }

            var json = File.ReadAllText(_filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var raws = JsonSerializer.Deserialize<List<MeasurementRaw>>(json, options) ?? new List<MeasurementRaw>();
            var measurements = new List<Measurement>(raws.Count);

            foreach (var r in raws)
            {
                if (r == null) continue;

                var m = new Measurement
                {
                    Id = r.Id ?? string.Empty,
                    DeviceId = r.Device_Id ?? string.Empty,
                    TimestampUtc = r.Ts_Utc,
                    TemperatureC = r.Temp_C,
                    HumidityPercent = r.Humidity
                };

                // Basic validation
                if (string.IsNullOrWhiteSpace(m.Id) || string.IsNullOrWhiteSpace(m.DeviceId))
                {
                    _logger.LogWarning("Invalid measurement record encountered. Id or DeviceId missing. Skipping.");
                    continue;
                }

                measurements.Add(m);
            }

            return measurements;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load measurements from {Path}", _filePath);
            return Array.Empty<Measurement>();
        }
    }
}
