namespace IotDemo.Domain;

public class Device
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DeviceType Type { get; set; }
    public string Location { get; set; } = default!;
    public int BatteryPercent { get; set; }
    public string FirmwareVersion { get; set; } = default!;
    public DeviceStatus Status { get; set; }
    public DateTime LastSeenUtc { get; set; }
}
