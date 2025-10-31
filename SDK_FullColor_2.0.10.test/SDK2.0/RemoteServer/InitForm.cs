using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SDKLibrary;
using System.Threading;
using System.Net;
using System.IO;

namespace RemoteServer
{
    public partial class InitForm : Form
    {
        public InitForm()
        {
            InitializeComponent();
        }

        public string Language  { get; private set;}
        public int Port { get; private set; }
        private int GetPort()
        {
            int port = 0;
            int.TryParse(textBox_Port.Text, out port);
            if (port < 1 || port > 65535)
            {
                port = 10001;
            }
            Port = port;
            return port;
        }

        public HDCommunicationManager CommManager { get;  set; }

        private delegate void ServerTipCallback(bool isSuccess, string tips);

        private void ServerTip(bool isSuccess, string tips)
        {
            ServerTipCallback stc = UpdateUI;
            this.Invoke(stc, new object[] { isSuccess, tips });
        }

        private void UpdateUI(bool isSuccess, string tips)
        {
            if (isSuccess)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {

                lab_Tips.Text = tips;
                lab_Tips.Visible = true;
            }
        }

        private void InitForm_Load(object sender, EventArgs e)
        {
            //List<string> list = new List<string>();
            //list.Add("English");
            ////list.Add("简体中文");
            //comboBox_Language.DataSource = list;
            //comboBox_Language.SelectedItem = "English";
        }

        private void OK_Click(object sender, EventArgs e)
        {

            try
            {
                if (radioButtonSever.Checked)
                {
                    CommManager.Listen(new IPEndPoint(IPAddress.Any, GetPort()));
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    IPAddress address;
                    if (IPAddress.TryParse(textBox_IP.Text, out address))
                    {
                        string exception;
                        CommManager.AddDevice(textBox_IP.Text, out exception);
                        if (exception.Length > 0)
                        {
                            lab_Tips.Text = exception;
                            lab_Tips.Visible = true;
                        }
                        else
                        {
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                lab_Tips.Text = ex.Message;
                lab_Tips.Visible = true;
            }

        }

        private void Cancle_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void textBox_Port_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void radioButtonSever_CheckedChanged(object sender, EventArgs e)
        {
            textBox_IP.Enabled = false;
            textBox_Port.Enabled = true;
        }

        private void radioButtonClient_CheckedChanged(object sender, EventArgs e)
        {
            textBox_IP.Enabled = true;
            textBox_Port.Text = "10001";
            textBox_Port.Enabled = false;
        }
    }
}
