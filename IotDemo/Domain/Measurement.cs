namespace IotDemo.Domain;

public class Measurement
{
    public string Id { get; set; } = default!;
    public string DeviceId { get; set; } = default!;
    public DateTime TimestampUtc { get; set; }
    public double? TemperatureC { get; set; }
    public double? HumidityPercent { get; set; }
}
