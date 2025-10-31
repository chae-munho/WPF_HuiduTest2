using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDKLibrary;
using System.Net.Sockets;
using System.Net;

namespace LocalSetting
{
    public partial class Form_Setting : Form
    {
        public Form_Setting()
        {
            InitializeComponent();
        }

       private HDCommunicationManager _commManager = new HDCommunicationManager();         // 通讯管理对象  
       public Device SelecteDevice { get; private set; }   // 当前选中设备
        private void Form_Setting_Load(object sender, EventArgs e)
        {
            // 开始扫描局域网设备
            _commManager.StartScanLANDevice();
            _commManager.MsgReport += MsgReport;
            _commManager.ResolvedInfoReport += ResolvedInfoReport;
        }

        private void button_ServerSetting_Click(object sender, EventArgs e)
        {

            if (SelecteDevice != null)
            {
                try
                {
                    //IPAddress address;
                    int port = 10001;
                    if (/*IPAddress.TryParse(textBox_Host.Text, out address) && */int.TryParse(textBox_Port.Text, out port))
                    {
                        ServerInfo serverInfo = new ServerInfo();
                        serverInfo.host = textBox_Host.Text;
                        serverInfo.port = port;
                        SelecteDevice.SetTcpServerInfo(serverInfo);
                    } 
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

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
                List<Device> Devices = _commManager.GetDevices();
                foreach (Device obj in Devices)
                {
                    //IPAddress ip = ((System.Net.IPEndPoint)obj.Client.TcpClient.Client.RemoteEndPoint).Address;
                    //listBox_Devices.Items.Add(ip.ToString());
                    listBox_Devices.Items.Add(obj.GetDeviceInfo().deviceID);
                }
                listBox_Devices.SelectedItem = select;

                if (listBox_Devices.SelectedItem == null && listBox_Devices.Items.Count > 0)
                {
                    listBox_Devices.SelectedIndex = 0;
                }

                if (device != null)
                {
                    string tips = device.GetDeviceInfo().deviceID + ":" + msg;
                    listBox_Tips.SelectedIndex = listBox_Tips.Items.Add(tips);
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

            if (ri.method == SdkMethod.GetSDKTcpServer.ToString())
            {
                ServerInfo serverInfo = (ServerInfo)ri.returnInfo;
                textBox_Host.Text = serverInfo.host;
                textBox_Port.Text = serverInfo.port.ToString();
            }
            else if (ri.method == SdkMethod.GetEth0Info.ToString())
            {
                EthernetInfo info = (EthernetInfo)ri.returnInfo;
                CheckBox_autoMode.Checked = info.isAutoDHCP;
                textBox_IP.Text = info.ip;
                textBox_Mask.Text = info.mask;
                textBox_Gate.Text = info.gateway;
                textBox_DNS.Text = info.dns;
            }
            string tips = ri.time + " " + device.GetDeviceInfo().deviceID + " " + ri.cmdType.ToString() + " " + ri.errorCode.ToString();
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

        private void button_RefreshServerInfo_Click(object sender, EventArgs e)
        {
            if (SelecteDevice != null)
            {
                SelecteDevice.GetTcpServerInfo();
            }
        }

        private void listBox_Devices_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSelectedDevice();
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
            List<Device> Devices = _commManager.GetDevices();
            foreach (Device device in Devices)
            {
                if (device.GetDeviceInfo().deviceID == select)
                {
                    SelecteDevice = device;
                    break;
                }
            }
        }

        private void btn_IPRefresh_Click(object sender, EventArgs e)
        {
            if (SelecteDevice != null)
            {
                SelecteDevice.GetEthernetInfo();
            }
        }

        private void btn_IPSetting_Click(object sender, EventArgs e)
        {
            if (SelecteDevice != null)
            {
                try
                {
                    IPAddress address;
                    if (IPAddress.TryParse(textBox_IP.Text, out address) && IPAddress.TryParse(textBox_Mask.Text, out address) && 
                        IPAddress.TryParse(textBox_Gate.Text, out address) && IPAddress.TryParse(textBox_DNS.Text, out address))
                    {
                        EthernetInfo info = new EthernetInfo();
                        info.isAutoDHCP = CheckBox_autoMode.Checked;
                        info.ip = textBox_IP.Text;
                        info.mask = textBox_Mask.Text;
                        info.gateway = textBox_Gate.Text;
                        info.dns = textBox_DNS.Text;
                        SelecteDevice.SetEthernetInfo(info);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
