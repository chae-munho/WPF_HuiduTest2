using SDKLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RemoteServer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public string Language { get; private set; }
        private HDCommunicationManager CommManager { get;  set; }
        public int Port { get; private set; }

        // 当前选中设备
        public Device SelecteDevice { get; private set; }


        // 节目GUID
        private string _ProgramGUID = Guid.NewGuid().ToString();

        // 区域GUID
        private string _AreaGUID = Guid.NewGuid().ToString();

        // 区域项GUID
        private string _AreaTextItemGUID = Guid.NewGuid().ToString();
        private string _AreaImageItemGUID = Guid.NewGuid().ToString();
        private string _AreaVideoItemGUID = Guid.NewGuid().ToString();
        private string _AreaClockItemGUID = Guid.NewGuid().ToString();



        // 当前促发控件事件的控件
        public object Sender { get; private set; }

        public List<UploadFileInfo> UploadFileInfoItems { get; private set; }

        private Timer _UploadFiletimer;
        private TimeInfo _timeInfo = new TimeInfo();
        private SwitchTimeInfo _switchTimeInfo = new SwitchTimeInfo();
        private ServerInfo _serverInfo = new ServerInfo();
        private LuminanceInfo _luminanceInfo = new LuminanceInfo();
        private void Init()
        {
            button_TextColor.ForeColor = button_TextColor.BackColor = Color.Red;

            for (int i = 0; i <= 8; ++i)
            {
                comboBox_InSpeed.Items.Add(i.ToString());
                comboBox_OutSpeed.Items.Add(i.ToString());
                comboBox_InSpeedImage.Items.Add(i.ToString());
                comboBox_OutSpeedImage.Items.Add(i.ToString());
            }

            comboBox_InSpeed.SelectedIndex = comboBox_OutSpeed.SelectedIndex = 4;
            comboBox_InSpeedImage.SelectedIndex = comboBox_OutSpeedImage.SelectedIndex = 4;


         
            DataTable dtTextIneffect = new DataTable();
            dtTextIneffect.Columns.Add("String", Type.GetType("System.String"));
            dtTextIneffect.Columns.Add("Value", typeof(int));

            DataTable dtImageIneffect = new DataTable();
            dtImageIneffect.Columns.Add("String", Type.GetType("System.String"));
            dtImageIneffect.Columns.Add("Value", typeof(int));

            for (int i = 0; i < (int)EffectType.EFFECT_COUNTS; ++i)
            {
                if(i == (int)EffectType.NOT_CLEAR_AREA || i == (int)EffectType.DOWN_SERIES_MOVE || i == (int)EffectType.UP_SERIES_MOVE ||
                    i == (int)EffectType.HT_DOWN_SERIES_MOVE || i == (int)EffectType.HT_UP_SERIES_MOVE)
                {
                    continue;
                }

                DataRow aRow = dtTextIneffect.NewRow();
                aRow[0] = ((EffectType)i).ToString();
                aRow[1] = (int)i;
                dtTextIneffect.Rows.Add(aRow);

                // image no support continue move
                if ((i >= (int)EffectType.LEFT_SERIES_MOVE && i <= (int)EffectType.DOWN_SERIES_MOVE)
                    || (i >= (int)EffectType.HT_LEFT_SERIES_MOVE && i <= (int)EffectType.HT_DOWN_SERIES_MOVE))
                {
                    continue;
                }

                aRow = dtImageIneffect.NewRow();
                aRow[0] = ((EffectType)i).ToString();
                aRow[1] = (int)i;
                dtImageIneffect.Rows.Add(aRow);
            }

            
            comboBox_InEffect.DisplayMember = "String";
            comboBox_InEffect.ValueMember = "Value";
            comboBox_InEffect.DataSource = dtTextIneffect;


            comboBox_InEffectImage.DisplayMember = "String";
            comboBox_InEffectImage.ValueMember = "Value";
            comboBox_InEffectImage.DataSource = dtImageIneffect;



            DataTable dtOutEffect = new DataTable();
            dtOutEffect.Columns.Add("String", Type.GetType("System.String"));
            dtOutEffect.Columns.Add("Value", typeof(int));

            for (int i = 0; i < (int)EffectType.EFFECT_COUNTS; ++i)
            {
                if ((i >= (int)EffectType.LEFT_SERIES_MOVE && i <= (int)EffectType.DOWN_SERIES_MOVE)
                    || (i >= (int)EffectType.HT_LEFT_SERIES_MOVE && i <= (int)EffectType.HT_DOWN_SERIES_MOVE))
                {
                    continue;
                }
                DataRow aRow = dtOutEffect.NewRow();
                aRow[0] = ((EffectType)i).ToString();
                aRow[1] = (int)i;
                dtOutEffect.Rows.Add(aRow);

                comboBox_OutEffect.Items.Add(((EffectType)i).ToString());
                comboBox_OutEffectImage.Items.Add(((EffectType)i).ToString());
            }

           
            comboBox_OutEffectImage.DisplayMember = comboBox_OutEffect.DisplayMember = "String";
            comboBox_OutEffectImage.ValueMember = comboBox_OutEffect.ValueMember = "Value";
            comboBox_OutEffectImage.DataSource = comboBox_OutEffect.DataSource = dtOutEffect;

            comboBox_InEffect.SelectedValue = comboBox_OutEffect.SelectedValue = (int)EffectType.RANDOM;
            comboBox_InEffectImage.SelectedValue = comboBox_OutEffectImage.SelectedValue = (int)EffectType.RANDOM;

            for (int i = 5; i <= 500; ++i)
            {
                if (i < 100)
                {
                    comboBox_FontSize.Items.Add(i);
                }
                else
                {
                    comboBox_FontSize.Items.Add(i);
                    i += 9;
                }
            }
            comboBox_FontSize.SelectedIndex = 7;

            comboBox_Font.Items.Add("Arial");
            comboBox_Font.SelectedIndex = 0;

            Text = "RemoteServer(TCP:" + Port.ToString() + ") ";

            UploadFileInfoItems = new List<UploadFileInfo>();


            // init clock page
      
            comboBox_Clocktype.Items.Add("digital");
            comboBox_Clocktype.Items.Add("dial");

            comboBox_ClockDate.Items.Add("YYYY/MM/DD");
            comboBox_ClockDate.Items.Add("MM/DD/YYYY");
            comboBox_ClockDate.Items.Add("DD/MM/YYYY");
            comboBox_ClockDate.Items.Add("Jan DD, YYYY");
            comboBox_ClockDate.Items.Add("DD Jan, YYYY");
            comboBox_ClockDate.Items.Add("YYYY年MM月DD日");
            comboBox_ClockDate.Items.Add("MM月DD日");

            comboBox_ClockTime.Items.Add("HH:MM:SS");
            comboBox_ClockTime.Items.Add("HH:MM");
            comboBox_ClockTime.Items.Add("HHsMMfSSm");
            comboBox_ClockTime.Items.Add("HHsMMf");

            comboBox_ClockWeek.Items.Add("Monday(translated)");
            comboBox_ClockWeek.Items.Add("Monday");
            comboBox_ClockWeek.Items.Add("Mon");


            checkBox_ClockTime.Checked = true;
            checkBox_ClockDate.Checked = true;
            comboBox_ClockDate.SelectedIndex = 0;
            comboBox_ClockTime.SelectedIndex = 0;
            comboBox_ClockWeek.SelectedIndex = 0;
            comboBox_Clocktype.SelectedIndex = 0;

            radioButton_TimeSwitch.Checked = true;
            SwitchTimeInfo info = new SwitchTimeInfo();
            richTextBox_xml.Text = info.SetSwitchTimeInfoToXml("");

            comboBox_ProgramType.Items.Add("normal");
            comboBox_ProgramType.Items.Add("offline");
            comboBox_ProgramType.SelectedIndex = 0;

            // init usb disk setting page
            Effective_All.Checked = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            CommManager = new HDCommunicationManager();
            CommManager.MsgReport += MsgReport;
            CommManager.ResolvedInfoReport += ResolvedInfoReport;
            InitForm initform = new InitForm();
            initform.CommManager = CommManager;
            if (initform.ShowDialog() == DialogResult.OK)
            {
                Port = initform.Port;

               // 自动查找本地局域网设备
               // CommManager.StartScanLANDevice();

                Init();
            }
            else
            {
                Application.Exit();
            }
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            /*
            if (SelecteDevice == null)
            {
                return;
            }


            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = "D:\\text.xml";
            dialog.Filter = "xml file (*.xml)|*.xml|all file(*.*)|*.*";
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            FileStream fs = File.Open(dialog.FileName, FileMode.Open);

            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            string xml = Encoding.UTF8.GetString(data);
           // xml = xml.Replace("##GUID", SelecteDevice.SdkGuid);
            SelecteDevice.SendFromXml(xml);

            */
        }

        private void MsgReport(object sender, string msg)
        {
            this.Invoke(new MsgReportEventHandler(MsgReportToUI), new object[] { sender, msg });
        }

        private void MsgReportToUI(Device device, string msg)
        {

            // device online offline
            if (msg == "online" || msg == "offline")
            {
                object select = listBox_Devices.SelectedItem;
                listBox_Devices.Items.Clear();

                List<Device> Devices = CommManager.GetDevices();
                foreach (Device obj in Devices)
                {
                    IPAddress ip = ((System.Net.IPEndPoint)obj.Client.TcpClient.Client.RemoteEndPoint).Address;
                    string strip = ip.ToString();
                    if (strip == "0.0.0.0")
                    {
                        int k = 0;
                    }
                    //listBox_Devices.Items.Add(ip.ToString());
                    listBox_Devices.Items.Add(obj.GetDeviceInfo().deviceID + "(" + obj.GetDeviceInfo().deviceName + ")");
                }
                listBox_Devices.SelectedItem = select;

                if (listBox_Devices.SelectedItem == null && listBox_Devices.Items.Count > 0)
                {
                    listBox_Devices.SelectedIndex = 0;
                }

                if (device != null)
                {
                    listBox_Tips.SelectedIndex = listBox_Tips.Items.Add(device.GetDeviceInfo().deviceID + ":" + msg);
                }

                ChangeSelectedDevice();
            }
            else
            {
                listBox_Tips.SelectedIndex = listBox_Tips.Items.Add(msg);
            }
        }

        private void ResolvedInfoReport(Device device, ResolveInfo ri)
        {
            this.Invoke(new ResolveInfoReportEventHandler(ResolvedInfoReportToUI), new object[] { device, ri });
        }

        // 接收反馈数据
        private void ResolvedInfoReportToUI(Device device, ResolveInfo ri)
        {
            // 获得字体信息并更新到界面
            if (ri.method == SdkMethod.GetAllFontInfo.ToString())
            {
                if (ri.returnInfo is List<FontInfo>)
                {
                    string oldText = comboBox_Font.Text;

                    comboBox_Font.Items.Clear();
                    List<FontInfo> fis = (List<FontInfo>)ri.returnInfo;
                    foreach (FontInfo fi in fis)
                    {
                        comboBox_Font.Items.Add(fi.fontName);
                    }
                    if(comboBox_Font.Items.Count == 0)
                    {
                        comboBox_Font.Items.Add("Arial");
                    }

                    bool findOld = false;
                    int index = 0;
                    foreach (var obj in comboBox_Font.Items)
                    {
                        if((string)obj == oldText)
                        {
                            comboBox_Font.SelectedIndex = index;
                            findOld = true;
                            break;
                        }
                        index++;
                    }
                    if (!findOld)
                    {
                        comboBox_Font.SelectedIndex = 0;
                    }

                }
            }
            else if (ri.method == SdkMethod.GetLuminancePloy.ToString())
            {
                if (radioButton_Luminance.Checked)
                {
                    _luminanceInfo = (LuminanceInfo)ri.returnInfo;
                    richTextBox_xml.Text = _luminanceInfo.SetLuminanceInfoToXml("");
                }
            }
            else if (ri.method == SdkMethod.GetTimeInfo.ToString() )
            {
                if (radioButton_Time.Checked)
                {
                    _timeInfo = (TimeInfo)ri.returnInfo;
                    richTextBox_xml.Text = _timeInfo.SetTimeInfoToXml("");
                }
            }
            else if (ri.method == SdkMethod.GetSwitchTime.ToString())
            {
                if (radioButton_TimeSwitch.Checked)
                {
                   _switchTimeInfo = (SwitchTimeInfo)ri.returnInfo;
                    _switchTimeInfo.open_enable = true;     // this value must be true when send setting data
                    richTextBox_xml.Text = _switchTimeInfo.SetSwitchTimeInfoToXml("");
                }
           
            }
            else if (ri.method == SdkMethod.GetSDKTcpServer.ToString())
            {
                if (radioButton_ServerInfo.Checked)
                {
                    _serverInfo = (ServerInfo)ri.returnInfo;
                    richTextBox_xml.Text = _serverInfo.SetServerInfoToXml("");
                }
            }
            else if (ri.method == SdkMethod.GetFiles.ToString())
            {
                if (ri.returnInfo is ReadbackFileListInfo)
                {
                   
                    ReadbackFileListInfo info = (ReadbackFileListInfo)ri.returnInfo;
                    // 更新同步文件列表
                    if (Sender == button_RefreshFileList)
                    {
                        listview_RemoteFileList.Items.Clear();
                        foreach (FileSession fi in info.fileList)
                        {
                            // no display config file
                            if (fi.name == "config.xml" || fi.name == "fpga.xml")
                            {
                                continue;
                            }

                            ListViewItem item1 = new ListViewItem(fi.name);
                            item1.SubItems.Add(fi.type);
                            item1.SubItems.Add(fi.size.ToString());
                            item1.SubItems.Add(fi.md5);
                            if (fi.existSize == fi.size && fi.size != 0)
                            {
                                item1.SubItems.Add("100%");
                            }
                            else if (fi.existSize < fi.size && fi.size > 0)
                            {

                                item1.SubItems.Add((fi.existSize * 100 / fi.size).ToString() + "%");
                            }
                            else
                            {
                                item1.SubItems.Add("error");
                            }
                            listview_RemoteFileList.Items.Add(item1);
                        }
                    }

                    if (Sender == button_SyncImageList || Sender == null || Sender == buttonSendAllFiles)
                    {
                        object oldText = comboBox_image.SelectedItem;


                        comboBox_image.Items.Clear();
                        foreach (FileSession fi in info.fileList)
                        {
                            if ((fi.type == "image" || fi.type == "tempImage") && fi.existSize == fi.size && fi.size != 0 && Device.GetHFileType(fi.name) == HFileType.kImageFile)
                            {
                                comboBox_image.Items.Add(fi.name);
                            }
                        }
                        comboBox_image.SelectedItem = oldText;
                        if (oldText == null && comboBox_image.Items.Count > 0)
                        {
                            comboBox_image.SelectedIndex = 0;
                        }
                    }

                    if (Sender == button_SyncVideoList || Sender == null || Sender == buttonSendAllFiles)
                    {
                        object oldText = comboBox_video.SelectedItem;
                        comboBox_video.Items.Clear();
                        foreach (FileSession fi in info.fileList)
                        {
                            if ((fi.type == "video" || fi.type == "tempVideo") && fi.existSize == fi.size && fi.size != 0)
                            {
                                comboBox_video.Items.Add(fi.name);
                            }
                        }
                        comboBox_video.SelectedItem = oldText;
                        if (oldText == null && comboBox_video.Items.Count > 0)
                        {
                            comboBox_video.SelectedIndex = 0;
                        }
                    }


                    if (Sender == buttonSendAllFiles)
                    {
                        sendAllImageAndVideo();
                    }

                }
            }
            else if (ri.method == SdkMethod.GetEnableUDiskFunction.ToString())
            {
                // Get U disk configurable function setting option check
                string xml = ri.srcXml.ToString();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNode node = doc.SelectSingleNode("//sdk/out");
                if (node.Attributes["result"] == null)
                {
                    return;
                }
                string attr = node.Attributes["result"].Value;
                if (attr != "kSuccess")
                {
                    return;
                }
                node = doc.SelectSingleNode("//sdk/out/effective");
                attr = node.Attributes["value"].Value;
                if (attr == "all" || attr == "insert")
                {
                    if (attr == "all")
                    {
                        Effective_All.Checked = true;
                    }
                    else
                    {
                        Effective_Insert.Checked = true;
                    }
                }

                node = doc.SelectSingleNode("//sdk/out/play");
                attr = node.Attributes["video"].Value;
                VideoImage.Checked = attr == "true" && node.Attributes["image"].Value == "true";

                node = doc.SelectSingleNode("//sdk/out/import");
                attr = node.Attributes["config"].Value;
                ImportConfig.Checked = attr == "true";
                attr = node.Attributes["updateProgram"].Value;
                ImportUpdateProgram.Checked = attr == "true";
                attr = node.Attributes["immeProgram"].Value;
                ImportImmeProgram.Checked = attr == "true";

                node = doc.SelectSingleNode("//sdk/out/storage");
                attr = node.Attributes["enable"].Value;
                StorageEnable.Checked = attr == "true";

            }
            else if (ri.srcXml != null &&
                !(ri.method == SdkMethod.AddProgram.ToString() || ri.method == SdkMethod.UpdateProgram.ToString() || ri.method == SdkMethod.DeleteProgram.ToString()))
            {
                richTextBox_xml.Text = ri.srcXml;
            }

            string tips = ri.time + " " + device.GetDeviceInfo().deviceID +  " " + ri.cmdType.ToString() + " " + ri.errorCode.ToString();
            if (ri.method != null)
            {
                tips += " " + ri.method.ToString();
            }

            if (ri.otherText != null)
            {
                tips += " " + ri.otherText;
            }

            listBox_Tips.SelectedIndex = listBox_Tips.Items.Add(tips);

        }

        private void button_ColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                ((Button)sender).ForeColor = ((Button)sender).BackColor = dialog.Color;
            }
        }

        private void ChangeSelectedDevice()
        {
            
            if (listBox_Devices.SelectedItem == null)
            {
                return;
            }

            Device oldSelectedDevice = SelecteDevice;

            SelecteDevice = null;

            string select = (string)listBox_Devices.SelectedItem.ToString();
            List<Device> Devices = CommManager.GetDevices();
            foreach (Device device in Devices)
            {
                if (device.GetDeviceInfo().deviceID + "(" + device.GetDeviceInfo().deviceName + ")" == select)
                {
                    SelecteDevice = device;
                    break;
                }
            }

            if (SelecteDevice != null)
            {
                DeviceInfo info = SelecteDevice.GetDeviceInfo();
                string title = "RemoteServer(TCP:" + Port.ToString() + ") " + info.screenWidth.ToString() + "*" + info.screenHeight.ToString();
                this.Text = title;
            }

            if (oldSelectedDevice != SelecteDevice && SelecteDevice != null)
            {
                listview_UploadList.Items.Clear();
                foreach (UploadFileInfo info in SelecteDevice.UploadItems)
                {
                    ListViewItem item1 = new ListViewItem(Path.GetFileName(info.path));
                    item1.SubItems.Add(info.type.ToString());
                    item1.SubItems.Add(info.fs.Length.ToString());
                    item1.SubItems.Add(info.md5);
                    item1.SubItems.Add(info.progress);

                    ListViewItem item = listview_UploadList.Items.Add(item1);
                    item.Tag = info;
                }
            }
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            if (SelecteDevice == null)
            {
                return;
            }

            ScreenParam screenParam = new ScreenParam();
            if(sender == button_Send)
            {
               screenParam.isNewScreen = true;
            }
            else if(sender == button_AddProgram)
            {
                screenParam.isNewScreen = false;
            }
            HdScreen screen = new HdScreen(screenParam);

            ProgramParam programParam = new ProgramParam();

            if ( comboBox_ProgramType.Text == "normal")
            {
                programParam.type = ProgramType.normal;
            }
            else if (comboBox_ProgramType.Text == "offline")
            {
                programParam.type = ProgramType.offline;
            }


            if (sender == button_AddProgram)
            {
                _ProgramGUID = Guid.NewGuid().ToString();
            }
            programParam.guid = _ProgramGUID;

            HdProgram program = new HdProgram(programParam);

            screen.Programs.Add(program);

            // !!!(only test !!) add a full screen background color to screen when send text.
            if (tabControl_Program.SelectedIndex == 0 && checkBox_Screenbackground.Checked)
            {
                AreaParam bgareaParam = new AreaParam();
                bgareaParam.x = 0;
                bgareaParam.y = 0;
                bgareaParam.width = SelecteDevice.GetDeviceInfo().screenWidth;
                bgareaParam.height = SelecteDevice.GetDeviceInfo().screenHeight;
                bgareaParam.guid = "00000000-0000-0000-0000-000000000000";
                HdArea bgarea = program.AddArea(bgareaParam);

                TextAreaItemParam bgtextItem = new TextAreaItemParam();
                bgtextItem.guid = "00000000-0000-0000-0000-000000000001";
                bgtextItem.text = "0";
                bgtextItem.useBackgroundColor = true;
                bgtextItem.color = bgtextItem.backgroundColor = button_ScreenBackground.BackColor;
                bgtextItem.effect.inEffet = EffectType.IMMEDIATE_SHOW;
                bgtextItem.effect.outEffet = EffectType.NOT_CLEAR_AREA;
                bgtextItem.effect.inSpeed = 5;
                bgtextItem.effect.outSpeed = 5;
                bgtextItem.effect.duration = 30;

                bgarea.AddText(bgtextItem);
            }

            AreaParam areaParam = new AreaParam();
            areaParam.x = 0;
            areaParam.y = 0;
            areaParam.width = SelecteDevice.GetDeviceInfo().screenWidth;
            areaParam.height = SelecteDevice.GetDeviceInfo().screenHeight;


            if (sender == button_AddProgram)
            {
                _AreaGUID = Guid.NewGuid().ToString();
            }
            areaParam.guid = _AreaGUID;

            HdArea area = program.AddArea(areaParam);


            // add text
            if (tabControl_Program.SelectedIndex == 0)
            {
               
                TextAreaItemParam textItem = new TextAreaItemParam();

                if (sender == button_AddProgram)
                {
                    _AreaTextItemGUID = Guid.NewGuid().ToString();
                }

                textItem.guid = _AreaTextItemGUID;
                textItem.fontName = comboBox_Font.Text;
                textItem.fontSize = int.Parse(comboBox_FontSize.Text);
                textItem.text = textBox_Text.Text;
                textItem.color = button_TextColor.BackColor;
                textItem.effect.inEffet = (EffectType)comboBox_InEffect.SelectedValue;
                textItem.effect.outEffet = (EffectType)comboBox_OutEffect.SelectedValue;
                textItem.effect.inSpeed = int.Parse(comboBox_InSpeed.Text);
                textItem.effect.outSpeed = int.Parse(comboBox_OutSpeed.Text);
                textItem.effect.duration = Int32.Parse(numericUpDown_staytime.Value.ToString());

                area.AddText(textItem);
            }
            // add image
            else if (tabControl_Program.SelectedIndex == 1)
            {
                ImageAreaItemParam imageItem = new ImageAreaItemParam();
                if (sender == button_AddProgram)
                {
                    _AreaImageItemGUID = Guid.NewGuid().ToString();
                }
                imageItem.guid = _AreaImageItemGUID;
                imageItem.file = comboBox_image.Text;
                imageItem.effect.inEffet = (EffectType)comboBox_InEffectImage.SelectedValue;
                imageItem.effect.outEffet = (EffectType)comboBox_OutEffectImage.SelectedValue;
                imageItem.effect.inSpeed = int.Parse(comboBox_InSpeedImage.Text);
                imageItem.effect.outSpeed = int.Parse(comboBox_OutSpeedImage.Text);
                imageItem.effect.duration = Int32.Parse(numericUpDown_staytimeImage.Value.ToString());

                area.AddImage(imageItem);
            }
            // add video
            else if (tabControl_Program.SelectedIndex == 2)
            {
                VideoAreaItemParam videoItem = new VideoAreaItemParam();
                if (sender == button_AddProgram)
                {
                    _AreaVideoItemGUID = Guid.NewGuid().ToString();
                }
                videoItem.guid = _AreaVideoItemGUID;
                videoItem.file = comboBox_video.Text;
                area.AddVedio(videoItem);
            }
            // add clock
            else if (tabControl_Program.SelectedIndex == 3)
            {
                ClockAreaItemParam item = new ClockAreaItemParam();
                if (sender == button_AddProgram)
                {
                    _AreaClockItemGUID = Guid.NewGuid().ToString();
                }
                item.guid = _AreaClockItemGUID;
                item.clockType = (ClockType)comboBox_Clocktype.SelectedIndex;
                item.date.dateDisplay = checkBox_ClockDate.Checked;
                item.date.dateFormat = comboBox_ClockDate.SelectedIndex;
                item.date.dateColor = button_ClockDateColor.ForeColor;

                item.time.timeDisplay = checkBox_ClockTime.Checked;
                item.time.timeFormat = comboBox_ClockTime.SelectedIndex;
                item.time.timeColor = button_ClockTimeColor.ForeColor;

                item.week.weekDisplay = checkBox_ClockWeek.Checked;
                item.week.weekFormat = comboBox_ClockWeek.SelectedIndex;
                item.week.weekColor = button_ClockWeekColor.ForeColor;

                item.title.titleDisplay = checkBox_ClockTitle.Checked;
                item.title.titleValue = textBox_ClockTitle.Text;
                item.title.titleColor = button_ClockTitleColor.ForeColor;

                item.lunarCalendar.lunarCalendarDisplay = checkBox_Clocklunarcalendar.Checked;
                item.lunarCalendar.lunarCalendarColor = button_ClockLunarCalendarColor.ForeColor;

                area.AddClock(item);
            }

            string xml = null;
            if (sender == button_UpdateProgram)
            {
                xml = SelecteDevice.UpdateDeviceProgram(program);
            }
            else
            {
                xml = SelecteDevice.SendScreen(screen);
            }

            radioButton_Xml.Checked = true;
            richTextBox_xml.Text = xml;

            //FileStream fs = new FileStream("h:\\save.xml", FileMode.Create);
            //byte[] buff = Encoding.UTF8.GetBytes(xml);
            //fs.Write(buff, 0, buff.Length);
            //fs.Close();

        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (SelecteDevice == null)
            {
                return;
            }

            ProgramParam programParam = new ProgramParam();
            programParam.guid = _ProgramGUID;

            HdProgram program = new HdProgram(programParam);

            string xml = SelecteDevice.DeleteDeviceProgram(program);

            radioButton_Xml.Checked = true;
            richTextBox_xml.Text = xml;

            //FileStream fs = new FileStream("d:\\Deletesave.xml", FileMode.Create);
            //byte[] buff = Encoding.UTF8.GetBytes(xml);
            //fs.Write(buff, 0, buff.Length);
            //fs.Close();
        }

        private void button_AddToUpload_Click(object sender, EventArgs e)
        {
            if (SelecteDevice == null)
            {
                return;
            }

            OpenFileDialog dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }
            foreach (var obj in UploadFileInfoItems)
            {
                if (obj.path == dialog.FileName)
                {
                    return;
                }
            }

            UploadFileInfo info = SelecteDevice.AddUploadFile(dialog.FileName, TempFile.Checked);

            //UploadFileInfoItems.Add(info);

            ListViewItem item1 = new ListViewItem(Path.GetFileName(info.path));
            item1.SubItems.Add(info.type.ToString());
            item1.SubItems.Add(info.fs.Length.ToString());
            item1.SubItems.Add(info.md5);
            item1.SubItems.Add(info.progress);

            ListViewItem item = listview_UploadList.Items.Add(item1);
            item.Tag = info;

        }

        private void button_DeleteUpload_Click(object sender, EventArgs e)
        {

            if (SelecteDevice == null)
            {
                return;
            }
            if (listview_UploadList.SelectedItems.Count > 0)
            {
                _UploadFiletimer = null;
                List<string> fileList = new List<string>();
                foreach (ListViewItem item in listview_UploadList.SelectedItems)
                {
                    UploadFileInfo info = (UploadFileInfo)item.Tag;
                    listview_UploadList.Items.Remove(item);
                    SelecteDevice.RemoveUploadFile(info);
                }
            }
        }

        private void button_Upload_Click(object sender, EventArgs e)
        {

            if (SelecteDevice == null)
            {
                return;
            }
            if (listview_UploadList.Items.Count > 0)
            {
                _UploadFiletimer = null;

                foreach (ListViewItem item in listview_UploadList.SelectedItems)
                {
                    UploadFileInfo info = (UploadFileInfo)item.Tag;   
                    UploadFileInfo newinfo = SelecteDevice.AddUploadFile(info.path, TempFile.Checked);
                    item.Tag = newinfo;
                }

                SelecteDevice.StartUploadFile();

                _UploadFiletimer = new Timer();
                _UploadFiletimer.Interval = 1000;
                _UploadFiletimer.Enabled = true;
                _UploadFiletimer.Tick += new EventHandler(timer1EventProcessor);
            }
        }

        public void timer1EventProcessor(object source, EventArgs e)
        {
            _UploadFiletimer = null;
            List<string> fileList = new List<string>();
            foreach (ListViewItem item in listview_UploadList.Items)
            {
                UploadFileInfo info = (UploadFileInfo)item.Tag;
                if (info.isSending)
                {
                    item.SubItems[4].Text = info.progress;
                }
                else if (info.hasFinished && item.SubItems[4].Text != info.progress)
                {
                    item.SubItems[4].Text = info.progress;
                }
            }

        }

        private void button_DeleteRomoteFile_Click(object sender, EventArgs e)
        {
            if (SelecteDevice == null)
            {
                return;
            }
            if (listview_RemoteFileList.SelectedItems.Count > 0)
            {
                List<string> fileList = new List<string>();
                foreach (ListViewItem item in listview_RemoteFileList.SelectedItems)
                {
                    fileList.Add(item.Text);
                    listview_RemoteFileList.Items.Remove(item);
                }
                SelecteDevice.DeleteFile(fileList);
            }
        }

        private void listBox_Devices_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSelectedDevice();
        }


        private void button_Readback_Click(object sender, EventArgs e)
        {
            if (SelecteDevice != null)
            {
                // SelecteDevice.GetLuminanceInfo();

                // SelecteDevice.GetSwitchTimeInfo();

                //SelecteDevice.OpenScreen();

                //SelecteDevice.GetTimeInfo();

                //SelecteDevice.GetBootLogoInfo();

                //SelecteDevice.GetTcpServerInfo();

                if (radioButton_Luminance.Checked)
                {
                    SelecteDevice.GetLuminanceInfo();
                }
                else if (radioButton_Time.Checked)
                {
                    SelecteDevice.GetTimeInfo();
                }
                else if (radioButton_TimeSwitch.Checked)
                {
                    SelecteDevice.GetSwitchTimeInfo();
                }
                else if (radioButton_ServerInfo.Checked)
                {
                    SelecteDevice.GetTcpServerInfo();
                }
                else
                {
                    SelecteDevice.OpenScreen();
                }
            }
        }

  
        // 同步图片、视频、文件列表
        private void button_SyncFileList_Click(object sender, EventArgs e)
        {
            if (SelecteDevice == null)
            {
                return;
            }
            Sender = sender;
            SelecteDevice.ReadbackFileList();
        }

        private void button_TextRefreshFont_Click(object sender, EventArgs e)
        {
            if (SelecteDevice == null)
            {
                return;
            }
            SelecteDevice.GetDeviceFontInfo();
        }

        private void button_Setting_Click(object sender, EventArgs e)
        {
             if (SelecteDevice != null)
            {
                try
                {
                    string xml = richTextBox_xml.Text;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    SelecteDevice.SendFromXml(xml);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


                //LuminanceInfo info = new LuminanceInfo();
                //info.defualtValue = 20;
                //SelecteDevice.SetLuminanceInfo(info);

                //SwitchTimeInfo info = new SwitchTimeInfo();
                //SwitchTimeInfo.PloyItem item = new SwitchTimeInfo.PloyItem();
                //info.items.Add(item);
                //info.ploy_enable = true;
                //SelecteDevice.SetSwitchTimeInfo(info);

                // SelecteDevice.CloseScreen();

               // TimeInfo info = new TimeInfo();
               // SelecteDevice.SetTimeInfo(info);

                //BootLogoInfo info = new BootLogoInfo();
                //SelecteDevice.SetBootLogoInfo(info);

                //ServerInfo info = new ServerInfo();
                //SelecteDevice.SetTcpServerInfo(info);
            }
        }


        private void radioButton_Setting_CheckedChanged(object sender, EventArgs e)
        {
            string xml = "";
            if (sender == radioButton_Luminance)
            {
                xml = _luminanceInfo.SetLuminanceInfoToXml("");
            }
            else if (sender == radioButton_Time)
            {
                xml = _timeInfo.SetTimeInfoToXml("");
            }
            else if (sender == radioButton_TimeSwitch)
            {

                xml = _switchTimeInfo.SetSwitchTimeInfoToXml("");
            }
            else if (sender == radioButton_ServerInfo)
            {

                xml = _serverInfo.SetServerInfoToXml("");
            }
            else if (sender == radioButton_Xml)
            {
                xml = "<?xml version = \"1.0\" encoding = \"utf-8\" ?>\r\n"
                          + "<sdk guid = \"##GUID\">\r\n<in method = \"GetProgram\" >\r\n</in>\r\n</sdk>\r\n";
            }

            richTextBox_xml.Text = xml;


        }

        private void comboBox_InEffect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox_InEffect.SelectedItem != null)
            {
                bool enable = true;
                int selectValue = (int)comboBox_InEffect.SelectedValue;
                if ((selectValue >= (int)EffectType.LEFT_SERIES_MOVE && selectValue <= (int)EffectType.DOWN_SERIES_MOVE)
                    || (selectValue >= (int)EffectType.HT_LEFT_SERIES_MOVE && selectValue <= (int)EffectType.HT_DOWN_SERIES_MOVE))
                {
                    enable = false;
                }
                comboBox_OutEffect.Enabled = enable;
                comboBox_OutSpeed.Enabled = enable;
                numericUpDown_staytime.Enabled = enable;
            }

        }

        private void button_ReInitDevice_Click(object sender, EventArgs e)
        {
            if (SelecteDevice == null)
            {
                return;
            }
        }

        private void button_Download_Click(object sender, EventArgs e)
        {
            // only test.
            return;

            if (SelecteDevice == null)
            {
                return;
            }
            if (listview_RemoteFileList.SelectedItems.Count > 0)
            {
                List<string> fileList = new List<string>();
                foreach (ListViewItem item in listview_RemoteFileList.SelectedItems)
                {
                    fileList.Add(item.Text);
                }

                if (fileList.Count > 0)
                {
                    SelecteDevice.DownloadFileFromDevice(fileList[0], "d:\\" + fileList[0]);
                }
            }
        }

        private void button_ScreenBackground_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                ((Button)sender).ForeColor = ((Button)sender).BackColor = dialog.Color;
            }
        }

        private void buttonSendAllFiles_Click(object sender, EventArgs e)
        {
            Sender = sender;
            SelecteDevice.ReadbackFileList();
        }

        private void sendAllImageAndVideo()
        {
            if (SelecteDevice == null)
            {
                return;
            }

            ScreenParam screenParam = new ScreenParam();
            screenParam.isNewScreen = true;
   
            HdScreen screen = new HdScreen(screenParam);

            ProgramParam programParam = new ProgramParam();
            programParam.type = ProgramType.normal;
  
            _ProgramGUID = Guid.NewGuid().ToString();  
            programParam.guid = _ProgramGUID;

            HdProgram program = new HdProgram(programParam);

            screen.Programs.Add(program);

            AreaParam areaParam = new AreaParam();
            areaParam.x = 0;
            areaParam.y = 0;
            areaParam.width = SelecteDevice.GetDeviceInfo().screenWidth;
            areaParam.height = SelecteDevice.GetDeviceInfo().screenHeight;

            _AreaGUID = Guid.NewGuid().ToString();
            areaParam.guid = _AreaGUID;

            HdArea area = program.AddArea(areaParam);

            // add all image
            for (int i = 0; i < comboBox_image.Items.Count; ++i)
            {
                ImageAreaItemParam imageItem = new ImageAreaItemParam();

                _AreaImageItemGUID = Guid.NewGuid().ToString();
                imageItem.guid = _AreaImageItemGUID;
                imageItem.file = comboBox_image.Items[i].ToString();
                imageItem.effect.inEffet = (EffectType)comboBox_InEffectImage.SelectedValue;
                imageItem.effect.outEffet = (EffectType)comboBox_OutEffectImage.SelectedValue;
                imageItem.effect.inSpeed = int.Parse(comboBox_InSpeedImage.Text);
                imageItem.effect.outSpeed = int.Parse(comboBox_OutSpeedImage.Text);
                imageItem.effect.duration = Int32.Parse(numericUpDown_staytimeImage.Value.ToString());

                area.AddImage(imageItem);
            }

            // add all video
            for (int i = 0; i < comboBox_video.Items.Count; ++i)
            {
                VideoAreaItemParam videoItem = new VideoAreaItemParam();
                _AreaVideoItemGUID = Guid.NewGuid().ToString(); 
                videoItem.guid = _AreaVideoItemGUID;
                videoItem.file = comboBox_video.Items[i].ToString();
                area.AddVedio(videoItem);
            }


            string xml = xml = SelecteDevice.SendScreen(screen);
            radioButton_Xml.Checked = true;
            richTextBox_xml.Text = xml;

            //FileStream fs = new FileStream("h:\\save.xml", FileMode.Create);
            //byte[] buff = Encoding.UTF8.GetBytes(xml);
            //fs.Write(buff, 0, buff.Length);
            //fs.Close();

        }

        private void Setting_Click(object sender, EventArgs e)
        {
            // set U disk func setting
            if (SelecteDevice == null)
            {
                return;
            }
            try
            {
                string effectiveText = Effective_All.Checked ? "all" : "insert";
                string effecitve = String.Format("<effective value = \"{0}\" />", effectiveText);

                string videoText = VideoImage.CheckState == CheckState.Checked ? "true" : "false";
                string play = String.Format("<play image = \"{0}\" video = \"{1}\" />", videoText, videoText);

                string configText = ImportConfig.CheckState == CheckState.Checked ? "true" : "false";
                string upgradeText = "true";
                string updateProgramText = ImportUpdateProgram.CheckState == CheckState.Checked ? "true" : "false";
                string immeProgramText = ImportImmeProgram.CheckState == CheckState.Checked ? "true" : "false";
                string import = String.Format("<import config = \"{0}\" upgrade = \"{1}\" updateProgram = \"{2}\" immeProgram = \"{3}\" />",
                    configText, upgradeText, updateProgramText, immeProgramText);

                string enbaleText = StorageEnable.CheckState == CheckState.Checked ? "true" : "false";
                string storage = String.Format("<storage enable = \"{0}\" />", enbaleText);

                string xml = String.Format("<?xml version = \"1.0\" encoding = \"utf-8\" ?>" +
                    "<sdk guid = \"##GUID\" >" +
                    "<in method = \"SetEnableUDiskFunction\" >" +
                    effecitve +
                    play +
                    import +
                    storage +
                    "</in> " +
                    "</sdk>");
                SelecteDevice.SendFromXml(xml);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReadSetting_Click(object sender, EventArgs e)
        {
            // read U disk function setting
            if (SelecteDevice == null)
            {
                return;
            }
            try
            {
                string xml = "<?xml version = \"1.0\" encoding = \"utf-8\" ?>" +
                    "<sdk guid = \"##GUID\">" +
                    "<in method = \"GetEnableUDiskFunction\" >" +
                    "</in>" +
                    "</sdk>";
                SelecteDevice.SendFromXml(xml);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
