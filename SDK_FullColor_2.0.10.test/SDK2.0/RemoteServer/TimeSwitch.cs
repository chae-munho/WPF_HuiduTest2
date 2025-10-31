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
    public partial class TimeSwitch : Form
    {
        public TimeSwitch()
        {
            InitializeComponent();
            timeploy ploy;
            ploy.box = checkBox1;
            ploy.start = dateTimePicker1;
            ploy.end = dateTimePicker2;
            switch_item_list_.Add(ploy);

            timeploy ploy2;
            ploy2.box = checkBox2;
            ploy2.start = dateTimePicker3;
            ploy2.end = dateTimePicker4;
            switch_item_list_.Add(ploy2);

            timeploy ploy3;
            ploy3.box = checkBox3;
            ploy3.start = dateTimePicker5;
            ploy3.end = dateTimePicker6;
            switch_item_list_.Add(ploy3);

            timeploy ploy4;
            ploy4.box = checkBox4;
            ploy4.start = dateTimePicker7;
            ploy4.end = dateTimePicker8;
            switch_item_list_.Add(ploy4);

            checkBox5.Checked = false;
            tableLayoutPanel1.Enabled = false;
            
        }

        struct timeploy
        {
            public CheckBox box;
            public DateTimePicker start;
            public DateTimePicker end;
        }

        private List<timeploy> switch_item_list_ = new List<timeploy>();

        public void Setview(SwitchTimeInfo info)
        {
            checkBox5.Checked = Convert.ToBoolean(info.ploy_enable);
            int lines = info.items.Count;
            if(lines > 3)
            {
                this.checkBox1.Checked = Convert.ToBoolean(info.items[0].enable);
                this.dateTimePicker1.Value = Convert.ToDateTime(info.items[0].start);
                this.dateTimePicker2.Value = Convert.ToDateTime(info.items[0].end);

                this.checkBox2.Checked = Convert.ToBoolean(info.items[1].enable);
                this.dateTimePicker3.Value = Convert.ToDateTime(info.items[1].start);
                this.dateTimePicker4.Value = Convert.ToDateTime(info.items[1].end);

                this.checkBox3.Checked = Convert.ToBoolean(info.items[2].enable);
                this.dateTimePicker5.Value = Convert.ToDateTime(info.items[2].start);
                this.dateTimePicker6.Value = Convert.ToDateTime(info.items[2].end);

                this.checkBox4.Checked = Convert.ToBoolean(info.items[3].enable);
                this.dateTimePicker7.Value = Convert.ToDateTime(info.items[3].start);
                this.dateTimePicker8.Value = Convert.ToDateTime(info.items[3].end);
            } 
            
        }

        public SwitchTimeInfo Getview()
        {
            SwitchTimeInfo info = new SwitchTimeInfo();
            info.ploy_enable = checkBox5.Checked;
            info.open_enable = true;
            SwitchTimeInfo.PloyItem ploy = new SwitchTimeInfo.PloyItem();
            ploy.enable = checkBox1.Checked;
            ploy.start = dateTimePicker1.Value.ToString("HH:mm:ss");
            ploy.end = dateTimePicker2.Value.ToString("HH:mm:ss");
            info.items.Add(ploy);

            SwitchTimeInfo.PloyItem ploy1 = new SwitchTimeInfo.PloyItem();
            ploy1.enable = checkBox2.Checked;
            ploy1.start = dateTimePicker3.Value.ToString("HH:mm:ss");
            ploy1.end = dateTimePicker4.Value.ToString("HH:mm:ss");
            info.items.Add(ploy1);

            SwitchTimeInfo.PloyItem ploy2 = new SwitchTimeInfo.PloyItem();
            ploy2.enable = checkBox3.Checked;
            ploy2.start = dateTimePicker5.Value.ToString("HH:mm:ss");
            ploy2.end = dateTimePicker6.Value.ToString("HH:mm:ss");
            info.items.Add(ploy2);

            SwitchTimeInfo.PloyItem ploy3 = new SwitchTimeInfo.PloyItem();
            ploy3.enable = checkBox4.Checked;
            ploy3.start = dateTimePicker7.Value.ToString("HH:mm:ss");
            ploy3.end = dateTimePicker8.Value.ToString("HH:mm:ss");
            info.items.Add(ploy3);

            return info;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if(checkBox5 != sender && !checkTime())
                {
                    checkBox1.Checked = false;
                    return;
                }
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
            } else
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                if (checkBox5 != sender&&!checkTime())
                {
                    checkBox2.Checked = false;
                    return;
                }
                dateTimePicker3.Enabled = false;
                dateTimePicker4.Enabled = false;
            } else
            {
                dateTimePicker3.Enabled = true;
                dateTimePicker4.Enabled = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                if (checkBox5 != sender && !checkTime())
                {
                    checkBox3.Checked = false;
                    return;
                }
                dateTimePicker5.Enabled = false;
                dateTimePicker6.Enabled = false;
            } else
            {
                dateTimePicker5.Enabled = true;
                dateTimePicker6.Enabled = true;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                if (checkBox5 != sender && !checkTime())
                {
                    checkBox4.Checked = false;
                    return;
                }
                dateTimePicker7.Enabled = false;
                dateTimePicker8.Enabled = false;
            } else
            {
                dateTimePicker7.Enabled = true;
                dateTimePicker8.Enabled = true;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if(!checkBox5.Checked)
            {
                tableLayoutPanel1.Enabled = false;
            } else
            {
                tableLayoutPanel1.Enabled = true;
                checkBox1_CheckedChanged(sender, e);
                checkBox2_CheckedChanged(sender, e);
                checkBox3_CheckedChanged(sender, e);
                checkBox4_CheckedChanged(sender, e);
            }
        }

        private bool checkTime()
        {
            int i = 0;
            for (; i < switch_item_list_.Count; ++i)
            {
                if (switch_item_list_[i].start.Value.ToString("HH:mm:ss") == switch_item_list_[i].end.Value.ToString("HH:mm:ss") && switch_item_list_[i].end.Value.ToString("HH:mm:ss") != "00:00:00")
                {
                    String str = String.Format("第{0}组，开始时间不能与结束时间一样", i+1);
                    MessageBox.Show(str, "错误", MessageBoxButtons.OK);
                    return false;
                } else
                {
                    for (int j = 0; j < switch_item_list_.Count; ++j)
                    {
                        if (i != j && switch_item_list_[j].box.Checked)
                        {
                            String start = switch_item_list_[j].start.Value.ToString("HH:mm:ss");
                            String end = switch_item_list_[j].end.Value.ToString("HH:mm:ss");
                            String start1 = switch_item_list_[i].start.Value.ToString("HH:mm:ss");
                            String end1 = switch_item_list_[i].end.Value.ToString("HH:mm:ss");
                            if (TimeInRange(start, end, start1) ||
                            TimeInRange(start, end, end1) ||
                            TimeInRange(start1, end1, start) ||
                            TimeInRange(start1, end1, end))
                            {
                                String str = string.Format("第{0}组和第{1}组时间冲突", i+1, j+1);
                                MessageBox.Show(str, "错误", MessageBoxButtons.OK);
                                return false;
                            }
                        }
                    }
                }
            }
                return true;
        }




        private bool TimeInRange(String start, String end, String time)
        {
            if(String.Compare(time, start, true) == 1 && String.Compare(end, time, true) == 1)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
