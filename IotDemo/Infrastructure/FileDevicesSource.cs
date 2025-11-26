using IotDemo.Domain;
using IotDemo.External;

namespace IotDemo.Infrastructure;

public class FileDevicesSource : IDevicesSource
{
    private readonly ExternalDevicesLoader _loader;

    public FileDevicesSource(ExternalDevicesLoader loader)
    {
        _loader = loader;
    }

    public IReadOnlyList<Device> LoadDevices()
    {
        return _loader.LoadDevices();
    }
}
