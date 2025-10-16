using SDKLibrary;

namespace HuiduTest2.Models
{
    public class DeviceItem
    {
        public Device Device { get; }
        public string Display { get; }

        public DeviceItem(Device device)
        {
            Device = device;
            var info = device.GetDeviceInfo();

            string id = info.deviceID ?? "Unknown";
            string name = info.deviceName ?? "Unknown";

            Display = $"{id} ({name})";
        }

        public override string ToString()
        {
            return Display;
        }
    }
}
