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
    public partial class RelayTimeSetting : Form
    {
        public RelayTimeSetting(Form form)
        {
            InitializeComponent();
            _form = form;
        }

        Form _form = null;



        private void RelayTimeSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            _form.Enabled = true;
        }

        struct TimeItem
        {
            public DateTimePicker start;
            public DateTimePicker end;
            public mySetButton deleteBtn;
        }

        public bool Getuseswitch()
        {
            return useswitchBox.Checked;
        }

        private List<TimeItem> timeItems = new List<TimeItem>();

        public void Setview(RelayInfo info, int index)
        {
            DeleteAllTimeItem();
            useswitchBox.Checked = Convert.ToBoolean(info.items[index].useSwitch);
            textBox1.Text = info.items[index].customName;
            for (int j = 0; j < info.items[index].timeItems.Count; ++j)
            {
                String starttime = info.items[index].timeItems[j].Key.ToString();
                String endtime = info.items[index].timeItems[j].Value.ToString();
                if(starttime != string.Empty && endtime != string.Empty)
                {
                    addItem(starttime, endtime);
                }
            }
        }

        private void addItem(String starttime, String endtime)
        {
            if (timeItems.Count == 10)
            {
                mySetButton1.Enabled = false;
                return;
            }
            String[] starttimeList = starttime.Split(':');
            String[] endtimeList = endtime.Split(':');
            ++tableLayoutPanel1.RowCount;
            tableLayoutPanel1.Height = (tableLayoutPanel1.RowCount) * 30;
            // 行高
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30));
            // 设置cell样式，
            //tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            int i = tableLayoutPanel1.RowCount - 1;
            DateTimePicker startTimePicker = new DateTimePicker();
            startTimePicker.Anchor = System.Windows.Forms.AnchorStyles.None;
            startTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            startTimePicker.Location = new System.Drawing.Point(78, 47);
            startTimePicker.ShowUpDown = true;
            startTimePicker.Size = new System.Drawing.Size(132, 21);
            startTimePicker.TabIndex = 10;
            startTimePicker.Value = new System.DateTime(1997, 10, 17, Convert.ToInt32(starttimeList.ElementAt(0)), Convert.ToInt32(starttimeList.ElementAt(1)), Convert.ToInt32(starttimeList.ElementAt(2)));
            startTimePicker.ValueChanged += new System.EventHandler(TimeChanged);
            tableLayoutPanel1.Controls.Add(startTimePicker, 0, i);

            DateTimePicker endTimePicker = new DateTimePicker();
            endTimePicker.Anchor = System.Windows.Forms.AnchorStyles.None;
            endTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            endTimePicker.Location = new System.Drawing.Point(78, 47);
            endTimePicker.ShowUpDown = true;
            endTimePicker.Size = new System.Drawing.Size(132, 21);
            endTimePicker.TabIndex = 10;
            endTimePicker.Value = new System.DateTime(1997, 10, 17, Convert.ToInt32(endtimeList.ElementAt(0)), Convert.ToInt32(endtimeList.ElementAt(1)), Convert.ToInt32(endtimeList.ElementAt(2)));
            endTimePicker.ValueChanged += new System.EventHandler(TimeChanged);
            tableLayoutPanel1.Controls.Add(endTimePicker, 1, i);

            mySetButton deleteBtn = new mySetButton();
            deleteBtn.CheckStyleX = mySetButton.CheckStyle.style3;
            deleteBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            tableLayoutPanel1.Controls.Add(deleteBtn, 2, i);
            deleteBtn.Click += new System.EventHandler(DeleteTimeItem);
            deleteBtn.Click += new System.EventHandler(TimeChanged);
            TimeItem a;
            a.start = startTimePicker;
            a.end = endTimePicker;
            a.deleteBtn = deleteBtn;

            timeItems.Add(a);
            TimeChanged();

            
        }

        private void mySetButton1_Click(object sender, EventArgs e)
        {
            if (timeItems.Count == 10)
            {
                mySetButton1.Enabled = false;
            }
            ++tableLayoutPanel1.RowCount;
            tableLayoutPanel1.Height = (tableLayoutPanel1.RowCount) * 30;
            // 行高
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30));
            // 设置cell样式，
            //tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            int i = tableLayoutPanel1.RowCount - 1;
            DateTimePicker startTimePicker = new DateTimePicker();
            startTimePicker.Anchor = System.Windows.Forms.AnchorStyles.None;
            startTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            startTimePicker.Location = new System.Drawing.Point(78, 47);
            startTimePicker.ShowUpDown = true;
            startTimePicker.Size = new System.Drawing.Size(132, 21);
            startTimePicker.TabIndex = 10;
            startTimePicker.Value = new System.DateTime(2021, 3, 1, 0, 0, 0, 0);
            startTimePicker.ValueChanged += new System.EventHandler(TimeChanged);
            tableLayoutPanel1.Controls.Add(startTimePicker, 0, i);

            DateTimePicker endTimePicker = new DateTimePicker();
            endTimePicker.Anchor = System.Windows.Forms.AnchorStyles.None;
            endTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            endTimePicker.Location = new System.Drawing.Point(78, 47);
            endTimePicker.ShowUpDown = true;
            endTimePicker.Size = new System.Drawing.Size(132, 21);
            endTimePicker.TabIndex = 10;
            endTimePicker.Value = new System.DateTime(2021, 3, 1, 0, 0, 0, 0);
            endTimePicker.ValueChanged += new System.EventHandler(TimeChanged);
            tableLayoutPanel1.Controls.Add(endTimePicker, 1, i);

            mySetButton deleteBtn = new mySetButton();
            deleteBtn.CheckStyleX = mySetButton.CheckStyle.style3;
            deleteBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            tableLayoutPanel1.Controls.Add(deleteBtn, 2, i);
            deleteBtn.Click += new System.EventHandler(DeleteTimeItem);
            deleteBtn.Click += new System.EventHandler(TimeChanged);
            TimeItem a;
            a.start = startTimePicker;
            a.end = endTimePicker;
            a.deleteBtn = deleteBtn;

            timeItems.Add(a);
            TimeChanged(sender,e);
        }

        private void TimeChanged()
        {
            if (!checkTime())
            {
                OKBtn.Enabled = false;
            }
            else
            {
                checkLable.Text = "";
                OKBtn.Enabled = true;
            }
        }
        private void TimeChanged(object sender, EventArgs e)
        {
            if(!checkTime())
            {
                OKBtn.Enabled = false;
            } 
            else
            {
                checkLable.Text = "";
                OKBtn.Enabled = true;
            }
        }
        private bool checkTime()
        {
            int i = 0;
            for (; i < timeItems.Count; ++i)
            {
                if (timeItems[i].start.Value.ToString("HH:mm:ss") == timeItems[i].end.Value.ToString("HH:mm:ss"))
                {
                    String str = String.Format("第{0}组,开始时间和结束时间不能一样！", (i+1).ToString());
                    checkLable.Text = str;
                    return false;
                }
                else
                {
                    for (int j = 0; j < timeItems.Count; ++j)
                    {
                        String start = timeItems[j].start.Value.ToString("HH:mm:ss");
                        String end = timeItems[j].end.Value.ToString("HH:mm:ss");
                        String start1 = timeItems[i].start.Value.ToString("HH:mm:ss");
                        String end1 = timeItems[i].end.Value.ToString("HH:mm:ss");
                        if (TimeInRange(start, end, start1) ||
                        TimeInRange(start, end, end1) ||
                        TimeInRange(start1, end1, start) ||
                        TimeInRange(start1, end1, end))
                        {
                            String str = string.Format("第{0}组和第{1}组时间冲突", i + 1, j + 1);
                            checkLable.Text = str;
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        private bool TimeInRange(String start, String end, String time)
        {
            if (String.Compare(time, start, true) == 1 && String.Compare(end, time, true) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DeleteAllTimeItem()
        {
            // 行数
            int i = 0;
            for (; i < timeItems.Count; i++)
            {
                tableLayoutPanel1.Controls.Remove(timeItems[i].start);
                tableLayoutPanel1.Controls.Remove(timeItems[i].end);
                tableLayoutPanel1.Controls.Remove(timeItems[i].deleteBtn);
                // 移动,当前行row的下行往上移动
                for (int k = i + 1; k < tableLayoutPanel1.RowCount - 1; k++)
                {
                    Control ctlNext = tableLayoutPanel1.GetControlFromPosition(0, k + 1);
                    tableLayoutPanel1.SetCellPosition(ctlNext, new TableLayoutPanelCellPosition(0, k));
                    Control ctlNext1 = tableLayoutPanel1.GetControlFromPosition(1, k + 1);
                    tableLayoutPanel1.SetCellPosition(ctlNext1, new TableLayoutPanelCellPosition(1, k));
                    Control ctlNext2 = tableLayoutPanel1.GetControlFromPosition(2, k + 1);
                    tableLayoutPanel1.SetCellPosition(ctlNext2, new TableLayoutPanelCellPosition(2, k));
                }

                //移除最后一行，最后为空白行
                tableLayoutPanel1.RowStyles.RemoveAt(tableLayoutPanel1.RowCount - 1);
                tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount - 1;
            }
            timeItems.Clear();
            tableLayoutPanel1.Height = tableLayoutPanel1.RowCount * 30;
        }

        private void  DeleteTimeItem(object sender, EventArgs e)
        {
            // 行数
            int row = 0;
            int i = 0;
            mySetButton deleteBtn = (mySetButton)sender;
            for (; i < timeItems.Count; ++i)
            {
                if(timeItems[i].deleteBtn == deleteBtn)
                {
                    tableLayoutPanel1.Controls.Remove(timeItems[i].start);
                    tableLayoutPanel1.Controls.Remove(timeItems[i].end);
                    tableLayoutPanel1.Controls.Remove(timeItems[i].deleteBtn);
                    // 移动,当前行row的下行往上移动
                    for (int k = i+1; k < tableLayoutPanel1.RowCount - 1; k++)
                    {
                        Control ctlNext = tableLayoutPanel1.GetControlFromPosition(0, k + 1);
                        tableLayoutPanel1.SetCellPosition(ctlNext, new TableLayoutPanelCellPosition(0, k));
                        Control ctlNext1 = tableLayoutPanel1.GetControlFromPosition(1, k + 1);
                        tableLayoutPanel1.SetCellPosition(ctlNext1, new TableLayoutPanelCellPosition(1, k));
                        Control ctlNext2 = tableLayoutPanel1.GetControlFromPosition(2, k + 1);
                        tableLayoutPanel1.SetCellPosition(ctlNext2, new TableLayoutPanelCellPosition(2, k));
                    }

                    //移除最后一行，最后为空白行
                    tableLayoutPanel1.RowStyles.RemoveAt(tableLayoutPanel1.RowCount - 1);
                    tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount - 1;
                    break;
                }
            }
            timeItems.RemoveAt(i);
            tableLayoutPanel1.Height = tableLayoutPanel1.RowCount * 30;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            _form.Enabled = true;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            foreach(var i in ((MyrelayView)_form).items)
            {
                if(i.timeSettingView == this)
                {
                    i.name_lable.Text = this.textBox1.Text;
                    String timestr = "";
                    for(int j = 0; j < this.timeItems.Count; ++j)
                    {
                        String strtemp = String.Format("{0}-{1}", timeItems[j].start.Value.ToString("HH:mm:ss"), timeItems[j].end.Value.ToString("HH:mm:ss"));
                        timestr = timestr + strtemp + ',';
                    }
                    if (timestr.Length > 0)
                    {
                        timestr = timestr.Substring(0, timestr.Length - 1);
                    }
                    i.time_lable.Text = timestr;
                    break;
                }
            }
            this.Hide();
            _form.Enabled = true;
        }

    }
}
