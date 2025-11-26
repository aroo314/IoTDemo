using IotDemo.Domain;

namespace IotDemo.Infrastructure;

public class InMemoryMeasurementsRepository
{
    private List<Measurement> _measurements = new();

    public IReadOnlyList<Measurement> GetByDeviceId(string deviceId)
    {
        return _measurements
            .Where(m => m.DeviceId == deviceId)
            .OrderByDescending(m => m.TimestampUtc)
            .ToList();
    }

    public void SetMeasurements(IEnumerable<Measurement> measurements)
    {
        _measurements = measurements.ToList();
    }
}
