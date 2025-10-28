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
using System.Threading; // Task.Delay

namespace HuiduTest2
{
    public partial class MainWindow : Window
    {
        private HDCommunicationManager _comm;
        private ObservableCollection<DeviceItem> _devices = new ObservableCollection<DeviceItem>();
        private Device _selected;
        private System.Timers.Timer _weatherTimer;

        private string _lastWeather = "";
        private string _lastDust = "";
        private Color _lastDustColor = Color.White;
        private bool _isDisplayingWeather = true; // 번갈아 표시용

        // OpenWeatherMap API
        private const string API_KEY = "c4d7023ec1c989df75f403cb5f291963";
        private const double LAT = 37.5665;
        private const double LON = 126.9780;

        public MainWindow()
        {
            InitializeComponent();
            DeviceList.ItemsSource = _devices;
            InitSdk();

            // 1분마다 날씨 자동 갱신
            _weatherTimer = new System.Timers.Timer(60000);
            _weatherTimer.Elapsed += async (s, e) => await UpdateWeatherAsync();
            _weatherTimer.Start();

            // 파트별 표시 루프 (날씨<->미세먼지)
            Task.Run(DisplayLoopAsync);
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
            }
            catch (Exception ex)
            {
                Log("SDK Init Error: " + ex.Message);
            }
        }

        private void Comm_MsgReport(object sender, string msg)
        {
            Dispatcher.Invoke(() =>
            {
                Device dev = sender as Device;
                if (dev != null)
                {
                    Log($"{dev.GetDeviceInfo().deviceID}: {msg}");
                    if (msg == "online" || msg == "offline")
                        RefreshDevices();
                }
                else Log(msg);
            });
        }

        private void Comm_ResolvedInfoReport(Device device, ResolveInfo info)
        {
            Dispatcher.Invoke(() =>
                Log($"{device.GetDeviceInfo().deviceID} {info.cmdType} {info.errorCode}")
            );
        }

        private void RefreshDevices()
        {
            Dispatcher.Invoke(() =>
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

        //날씨 +    미세먼지 데이터 갱신
        private async Task UpdateWeatherAsync()
        {
            try
            {
                string weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={LAT}&lon={LON}&appid={API_KEY}&units=metric&lang=kr";
                string airUrl = $"https://api.openweathermap.org/data/2.5/air_pollution?lat={LAT}&lon={LON}&appid={API_KEY}";

                using (var client = new System.Net.Http.HttpClient())
                {
                    var weatherResponse = await client.GetStringAsync(weatherUrl);
                    JObject w = JObject.Parse(weatherResponse);
                    string city = w["name"]?.ToString() ?? "Unknown";
                    string cond = w["weather"]?[0]?["description"]?.ToString() ?? "?";
                    double temp = w["main"]?["temp"]?.ToObject<double>() ?? 0;
                    double feels = w["main"]?["feels_like"]?.ToObject<double>() ?? 0;
                    int hum = w["main"]?["humidity"]?.ToObject<int>() ?? 0;
                    double wind = w["wind"]?["speed"]?.ToObject<double>() ?? 0;

                    var airResponse = await client.GetStringAsync(airUrl);
                    JObject a = JObject.Parse(airResponse);
                    var comp = a["list"]?[0]?["components"];
                    double pm10 = comp?["pm10"]?.ToObject<double>() ?? 0;
                    double pm25 = comp?["pm2_5"]?.ToObject<double>() ?? 0;
                    (string dustGrade, Color dustColor) = GetDustInfo(pm25);

                    _lastWeather = $"{city}  {cond}  {temp:F1}℃ (체감 {feels:F1}℃)\n습도 {hum}%  바람 {wind:F1}m/s";
                    _lastDust = $"미세먼지(PM10): {pm10:F0}㎍/㎥\n초미세먼지(PM2.5): {pm25:F0}㎍/㎥ ({dustGrade})";
                    _lastDustColor = dustColor;

                    Dispatcher.Invoke(() =>
                    {
                        WeatherText.Text = _lastWeather;
                        TempText.Text = _lastDust;
                    });
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => Log("Weather update error: " + ex.Message));
            }
        }

        // 미세먼지 등급별 색상
        private (string, Color) GetDustInfo(double pm25)
        {
            if (pm25 <= 30) return ("좋음", Color.Green);
            else if (pm25 <= 80) return ("보통", Color.Yellow);
            else if (pm25 <= 150) return ("나쁨", Color.Orange);
            else return ("매우 나쁨", Color.Red);
        }

        // 순차적 표시 루프 (스크롤 없음)
        private async Task DisplayLoopAsync()
        {
            while (true)
            {
                if (_selected != null)
                {
                    if (_isDisplayingWeather)
                    {
                        SendToLed(_lastWeather, Color.LightBlue);
                        Log("📡 [LED] Weather displayed (5s)");
                    }
                    else
                    {
                        SendToLed(_lastDust, _lastDustColor);
                        Log("📡 [LED] Dust displayed (5s)");
                    }

                    await Task.Delay(5000); // 5초간 표시
                    _isDisplayingWeather = !_isDisplayingWeather;
                }
                else
                {
                    await Task.Delay(2000); // 장치 미선택 시 대기
                }
            }
        }

        // 전광판 출력 (고정 표시)
        private void SendToLed(string text, Color color)
        {
            try
            {
                if (_selected == null) return;

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
                    width = 192,
                    height = 96
                });

                var textItem = new TextAreaItemParam()
                {
                    guid = Guid.NewGuid().ToString(),
                    text = text,
                    fontName = "Arial",
                    fontSize = 13,
                    color = color,
                    bold = true,
                    hAlignment = SDKLibrary.HorizontalAlignment.center,
                    vAlignment = SDKLibrary.VerticalAlignment.middle,
                    isSingleLine = false,
                    useBackgroundColor = false
                };

                textItem.effect.inEffet = EffectType.IMMEDIATE_SHOW;
                textItem.effect.outEffet = EffectType.NOT_CLEAR_AREA;
                textItem.effect.duration = 5; // 표시 유지시간

                area.AddText(textItem);
                _selected.SendScreen(screen);
            }
            catch (Exception ex)
            {
                Log("LED send error: " + ex.Message);
            }
        }

        private void PowerOnBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _selected?.OpenScreen();
                Log("LED Power ON");
            }
            catch (Exception ex)
            {
                Log("Power ON error: " + ex.Message);
            }
        }

        private void PowerOffBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _selected?.CloseScreen();
                Log("LED Power OFF");
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
            Dispatcher.Invoke(() =>
            {
                LogBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\n");
                LogBox.ScrollToEnd();
            });
        }
    }
}
