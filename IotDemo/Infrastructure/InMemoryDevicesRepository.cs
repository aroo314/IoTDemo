using IotDemo.Domain;

namespace IotDemo.Infrastructure;

public class InMemoryDevicesRepository
{
    private List<Device> _devices = new();

    public IReadOnlyList<Device> GetAll() => _devices;

    public void SetDevices(IEnumerable<Device> devices)
    {
        _devices = devices.ToList();
    }

    public Device? GetById(string id)
    {
        return _devices.FirstOrDefault(d => d.Id == id);
    }
}
