using IotDemo.Domain;

namespace IotDemo.Infrastructure;

public interface IDevicesSource
{
    IReadOnlyList<Device> LoadDevices();
}
