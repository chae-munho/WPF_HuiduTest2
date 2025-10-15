using SDKLibrary;
using System.ComponentModel;

namespace HuiduTest2.Models
{
    public class DeviceItem : INotifyPropertyChanged
    {
        private string _deviceName;
        private string _ip;
        private string _status;

        public Device Device { get; private set; }

        public string DeviceName
        {
            get { return _deviceName; }
            set
            {
                _deviceName = value;
                OnPropertyChanged("DeviceName");
            }
        }

        public string IP
        {
            get { return _ip; }
            set
            {
                _ip = value;
                OnPropertyChanged("IP");
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public DeviceItem(Device dev)
        {
            Device = dev;
            var info = dev.GetDeviceInfo();

            DeviceName = info.deviceName;

            // ✅ IP 필드가 존재하지 않는 SDK 대응
            try
            {
                // SDK 버전에 따라 필드 이름이 다름 (deviceIP, ip, ipAddress 중 하나)
                var prop = info.GetType().GetProperty("deviceIP") ??
                           info.GetType().GetProperty("ip") ??
                           info.GetType().GetProperty("ipAddress");

                IP = prop != null ? prop.GetValue(info)?.ToString() : "Unknown";
            }
            catch
            {
                IP = "Unknown";
            }

            Status = "Connected";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            return $"{DeviceName} ({IP}) - {Status}";
        }
    }
}
