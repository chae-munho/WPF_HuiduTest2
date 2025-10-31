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

namespace RemoteServer
{
    public partial class MyrelayView : Form
    {
         

        public MyrelayView()
        {
            InitializeComponent();
            //this.IsMdiContainer = true;
            Itemploys ploy;
            ploy.checkbox = checkBox1;
            ploy.name_lable = label11;
            ploy.time_lable = label21;
            ploy.setbtn = mySetButton1;
            ploy.timeSettingView = new RelayTimeSetting(this);
            ploy.timeSettingView.MdiParent = this.MdiParent;//指定子窗体的父窗体
            items.Add(ploy);

            Itemploys ploy2;
            ploy2.checkbox = checkBox2;
            ploy2.name_lable = label12;
            ploy2.time_lable = label22;
            ploy2.setbtn = mySetButton2;
            ploy2.timeSettingView = new RelayTimeSetting(this);
            ploy2.timeSettingView.MdiParent = this.MdiParent;//指定子窗体的父窗体
            items.Add(ploy2);

            Itemploys ploy3;
            ploy3.checkbox = checkBox3;
            ploy3.name_lable = label13;
            ploy3.time_lable = label23;
            ploy3.setbtn = mySetButton3;
            ploy3.timeSettingView = new RelayTimeSetting(this);
            ploy3.timeSettingView.MdiParent = this.MdiParent;//指定子窗体的父窗体
            items.Add(ploy3);

            Itemploys ploy4;
            ploy4.checkbox = checkBox4;
            ploy4.name_lable = label14;
            ploy4.time_lable = label24;
            ploy4.setbtn = mySetButton4;
            ploy4.timeSettingView = new RelayTimeSetting(this);
            ploy4.timeSettingView.MdiParent = this.MdiParent;//指定子窗体的父窗体
            items.Add(ploy4);

            Itemploys ploy5;
            ploy5.checkbox = checkBox5;
            ploy5.name_lable = label15;
            ploy5.time_lable = label25;
            ploy5.setbtn = mySetButton5;
            ploy5.timeSettingView = new RelayTimeSetting(this);
            ploy5.timeSettingView.MdiParent = this.MdiParent;//指定子窗体的父窗体
            items.Add(ploy5);

            Itemploys ploy6;
            ploy6.checkbox = checkBox6;
            ploy6.name_lable = label16;
            ploy6.time_lable = label26;
            ploy6.setbtn = mySetButton6;
            ploy6.timeSettingView = new RelayTimeSetting(this);
            ploy6.timeSettingView.MdiParent = this.MdiParent;//指定子窗体的父窗体
            items.Add(ploy6);
        }

        public List<Itemploys> items = new List<Itemploys>();


        public struct Itemploys
        {
            public CheckBox checkbox;
            public Label name_lable;
            public Label time_lable;
            public mySetButton setbtn;
            public RelayTimeSetting timeSettingView;
        }


        public void Setview(RelayInfo info)
        {
            for(int i = 0; i < items.Count; ++i)
            {
                String timeStr = "";
                items[i].checkbox.Checked = false;  // Convert.ToBoolean(info.items[i].useSwitch);
                items[i].name_lable.Text = info.items[i].customName;
                for(int j = 0;j < info.items[i].timeItems.Count; ++j)
                {
                    String str = String.Format("{0}-{1}", info.items[i].timeItems[j].Key.ToString(), info.items[i].timeItems[j].Value.ToString());
                    timeStr = timeStr + str + ',';
                }
                if(timeStr.Length > 0)
                {
                    timeStr = timeStr.Substring(0, timeStr.Length - 1);
                }
                items[i].time_lable.Text = timeStr;
                items[i].timeSettingView.Setview(info, i);
            }
        }

        public RelayInfo Getview()
        {
            RelayInfo info = new RelayInfo();
            for (int i = 0; i < items.Count; ++i)
            {
                RelayInfo.RelayItem item = new RelayInfo.RelayItem();
                item.useSwitch = Convert.ToInt32(items[i].timeSettingView.Getuseswitch());
                item.customName = items[i].name_lable.Text;
                String timeStr = items[i].time_lable.Text;
                String[] strlist = timeStr.Split(',');
                for(int j = 0; j < strlist.Length; ++j)
                {
                    String[] timearr = strlist[j].Split('-');
                    if(timearr.Length > 1)
                    {
                        item.timeItems.Add(new KeyValuePair<String, String>(timearr[0], timearr[1]));
                    }
                }
                info.items.Add(item);

            }
            return info;
        }

        private void showsetView(object sender, EventArgs e)
        {
            this.Enabled = false;
            mySetButton btn = (mySetButton)sender;
            foreach(var i in items)
            {
                if(i.setbtn == btn)
                {
                    i.timeSettingView.StartPosition = FormStartPosition.Manual;
                    int xWidth = SystemInformation.PrimaryMonitorSize.Width;//获取显示器屏幕宽度
                    int yHeight = SystemInformation.PrimaryMonitorSize.Height;//高度
                    i.timeSettingView.Location = new Point(xWidth * 2 / 5, yHeight / 3);
                    i.timeSettingView.Show();
                }
            }
        }
    }
}
