using SDKLibrary;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Newtonsoft.Json.Linq;
using HuiduTest2.Models;
using System.Drawing; // Color

namespace HuiduTest2
{
    public partial class MainWindow : Window
    {
        private HDCommunicationManager _comm;
        private ObservableCollection<DeviceItem> _devices = new ObservableCollection<DeviceItem>();
        private Device _selected;
        private Timer _weatherTimer;

        //  OpenWeatherMap API
        private const string API_KEY = "c4d7023ec1c989df75f403cb5f291963";
        private const double LAT = 37.5665;  // 서울
        private const double LON = 126.9780;

        public MainWindow()
        {
            InitializeComponent();
            DeviceList.ItemsSource = _devices;

            InitSdk();

            //  3초마다 날씨 자동 갱신
            _weatherTimer = new Timer(3000);
            _weatherTimer.Elapsed += async (s, e) => await UpdateWeatherAsync();
            _weatherTimer.Start();
        }

        private void InitSdk()
        {
            try
            {
                _comm = new HDCommunicationManager();
                _comm.MsgReport += Comm_MsgReport;
                _comm.ResolvedInfoReport += Comm_ResolvedInfoReport;
                _comm.Listen(new IPEndPoint(IPAddress.Any, 10001));
                Log("Listening on port 10001...");

                _comm.StartScanLANDevice();
                Log("Scanning for controllers...");
            }
            catch (Exception ex)
            {
                Log("SDK Init Error: " + ex.Message);
            }
        }

        private void Comm_MsgReport(object sender, string msg)
        {
            Dispatcher.Invoke(delegate
            {
                Device dev = sender as Device;
                if (dev != null)
                {
                    Log(dev.GetDeviceInfo().deviceID + ": " + msg);
                    if (msg == "online" || msg == "offline")
                        RefreshDevices();
                }
                else
                {
                    Log(msg);
                }
            });
        }

        private void Comm_ResolvedInfoReport(Device device, ResolveInfo info)
        {
            Dispatcher.Invoke(delegate
            {
                Log(device.GetDeviceInfo().deviceID + " " + info.cmdType + " " + info.errorCode);
            });
        }

        private void RefreshDevices()
        {
            Dispatcher.Invoke(delegate
            {
                _devices.Clear();
                var list = _comm.GetDevices();
                foreach (var d in list)
                    _devices.Add(new DeviceItem(d));

                if (_devices.Any())
                {
                    _selected = _devices.First().Device;
                    Log("Selected: " + _selected.GetDeviceInfo().deviceID);
                }
            });
        }

        // 날씨 자동 업데이트
        private async Task UpdateWeatherAsync()
        {
            try
            {
                string url = string.Format(
                    "https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}&units=metric&lang=kr",
                    LAT, LON, API_KEY);

                using (var client = new System.Net.Http.HttpClient())
                {
                    var response = await client.GetStringAsync(url);
                    JObject json = JObject.Parse(response);

                    string condition = json["weather"]?[0]?["description"] != null
                        ? json["weather"][0]["description"].ToString()
                        : "?";

                    double temp = json["main"]?["temp"] != null
                        ? json["main"]["temp"].ToObject<double>()
                        : 0;

                    Dispatcher.Invoke(delegate
                    {
                        WeatherText.Text = condition;
                        TempText.Text = string.Format("{0:F1} ℃", temp);
                    });

                    if (_selected != null)
                        SendToLed(condition + "  " + string.Format("{0:F1}℃", temp));
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(delegate { Log("Weather update error: " + ex.Message); });
            }
        }

        //  전광판으로 텍스트 출력
        private void SendToLed(string text)
        {
            try
            {
                if (_selected == null) return;

                var info = _selected.GetDeviceInfo();
                var screen = new HdScreen(new ScreenParam() { isNewScreen = true });
                var program = new HdProgram(new ProgramParam()
                {
                    type = ProgramType.normal,
                    guid = Guid.NewGuid().ToString()
                });
                screen.Programs.Add(program);

                var area = program.AddArea(new AreaParam()
                {
                    guid = Guid.NewGuid().ToString(),
                    x = 0,
                    y = 0,
                    width = info.screenWidth,
                    height = info.screenHeight
                });

                var textItem = new TextAreaItemParam()
                {
                    guid = Guid.NewGuid().ToString(),
                    text = text,
                    fontName = "Arial",
                    fontSize = 28,
                    color = Color.Yellow
                };

                textItem.effect.inEffet = EffectType.IMMEDIATE_SHOW;
                textItem.effect.outEffet = EffectType.NOT_CLEAR_AREA;
                textItem.effect.duration = 5;

                area.AddText(textItem);
                _selected.SendScreen(screen);
                Log("LED updated: " + text);
            }
            catch (Exception ex)
            {
                Log("LED send error: " + ex.Message);
            }
        }

        // 전광판 전원 켜기
        private void PowerOnBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selected != null)
                {
                    _selected.OpenScreen();
                    Log("LED Power ON");
                }
                else
                    Log("No controller selected.");
            }
            catch (Exception ex)
            {
                Log("Power ON error: " + ex.Message);
            }
        }

        //  전광판 전원 끄기
        private void PowerOffBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selected != null)
                {
                    _selected.CloseScreen();
                    Log("LED Power OFF");
                }
                else
                    Log("No controller selected.");
            }
            catch (Exception ex)
            {
                Log("Power OFF error: " + ex.Message);
            }
        }

        private void DeviceList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DeviceItem item = DeviceList.SelectedItem as DeviceItem;
            if (item != null)
                _selected = item.Device;
        }

        private void Log(string msg)
        {
            Dispatcher.Invoke(delegate
            {
                LogBox.AppendText(string.Format("[{0:HH:mm:ss}] {1}\n", DateTime.Now, msg));
                LogBox.ScrollToEnd();
            });
        }
    }
}
