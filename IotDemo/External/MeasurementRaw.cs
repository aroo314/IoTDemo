namespace IotDemo.External;

public class MeasurementRaw
{
    public string Id { get; set; } = default!;
    public string Device_Id { get; set; } = default!;
    public DateTime Ts_Utc { get; set; }
    public double? Temp_C { get; set; }
    public double? Humidity { get; set; }
}
