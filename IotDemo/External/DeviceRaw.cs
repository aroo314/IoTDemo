namespace IotDemo.External;

public class DeviceRaw
{
    public int Id { get; set; }
    public string Device_Name { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string? Loc { get; set; }
    public int Battery { get; set; }
    public string Fw_Ver { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateTime Last_Seen_Utc { get; set; }
}
