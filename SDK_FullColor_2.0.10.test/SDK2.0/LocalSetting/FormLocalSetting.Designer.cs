namespace LocalSetting
{
    partial class Form_Setting
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox_Devices = new System.Windows.Forms.ListBox();
            this.tabControl_Setting = new System.Windows.Forms.TabControl();
            this.tabPage_ServerSetting = new System.Windows.Forms.TabPage();
            this.button_RefreshServerInfo = new System.Windows.Forms.Button();
            this.button_ServerSetting = new System.Windows.Forms.Button();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            this.textBox_Host = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage_IPSetting = new System.Windows.Forms.TabPage();
            this.btn_IPRefresh = new System.Windows.Forms.Button();
            this.btn_IPSetting = new System.Windows.Forms.Button();
            this.textBox_DNS = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_Gate = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_Mask = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.CheckBox_autoMode = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.listBox_Tips = new System.Windows.Forms.ListBox();
            this.tabControl_Setting.SuspendLayout();
            this.tabPage_ServerSetting.SuspendLayout();
            this.tabPage_IPSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox_Devices
            // 
            this.listBox_Devices.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBox_Devices.FormattingEnabled = true;
            this.listBox_Devices.ItemHeight = 12;
            this.listBox_Devices.Location = new System.Drawing.Point(0, 0);
            this.listBox_Devices.Name = "listBox_Devices";
            this.listBox_Devices.Size = new System.Drawing.Size(152, 427);
            this.listBox_Devices.TabIndex = 6;
            this.listBox_Devices.SelectedIndexChanged += new System.EventHandler(this.listBox_Devices_SelectedIndexChanged);
            // 
            // tabControl_Setting
            // 
            this.tabControl_Setting.Controls.Add(this.tabPage_ServerSetting);
            this.tabControl_Setting.Controls.Add(this.tabPage_IPSetting);
            this.tabControl_Setting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Setting.Location = new System.Drawing.Point(152, 0);
            this.tabControl_Setting.Name = "tabControl_Setting";
            this.tabControl_Setting.SelectedIndex = 0;
            this.tabControl_Setting.Size = new System.Drawing.Size(416, 427);
            this.tabControl_Setting.TabIndex = 7;
            // 
            // tabPage_ServerSetting
            // 
            this.tabPage_ServerSetting.Controls.Add(this.button_RefreshServerInfo);
            this.tabPage_ServerSetting.Controls.Add(this.button_ServerSetting);
            this.tabPage_ServerSetting.Controls.Add(this.textBox_Port);
            this.tabPage_ServerSetting.Controls.Add(this.textBox_Host);
            this.tabPage_ServerSetting.Controls.Add(this.label3);
            this.tabPage_ServerSetting.Controls.Add(this.label2);
            this.tabPage_ServerSetting.Location = new System.Drawing.Point(4, 22);
            this.tabPage_ServerSetting.Name = "tabPage_ServerSetting";
            this.tabPage_ServerSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ServerSetting.Size = new System.Drawing.Size(408, 401);
            this.tabPage_ServerSetting.TabIndex = 0;
            this.tabPage_ServerSetting.Text = "Server Setting";
            this.tabPage_ServerSetting.UseVisualStyleBackColor = true;
            // 
            // button_RefreshServerInfo
            // 
            this.button_RefreshServerInfo.Location = new System.Drawing.Point(95, 208);
            this.button_RefreshServerInfo.Name = "button_RefreshServerInfo";
            this.button_RefreshServerInfo.Size = new System.Drawing.Size(75, 23);
            this.button_RefreshServerInfo.TabIndex = 39;
            this.button_RefreshServerInfo.Text = "Refresh";
            this.button_RefreshServerInfo.UseVisualStyleBackColor = true;
            this.button_RefreshServerInfo.Click += new System.EventHandler(this.button_RefreshServerInfo_Click);
            // 
            // button_ServerSetting
            // 
            this.button_ServerSetting.Location = new System.Drawing.Point(181, 208);
            this.button_ServerSetting.Name = "button_ServerSetting";
            this.button_ServerSetting.Size = new System.Drawing.Size(75, 23);
            this.button_ServerSetting.TabIndex = 38;
            this.button_ServerSetting.Text = "Setting";
            this.button_ServerSetting.UseVisualStyleBackColor = true;
            this.button_ServerSetting.Click += new System.EventHandler(this.button_ServerSetting_Click);
            // 
            // textBox_Port
            // 
            this.textBox_Port.Location = new System.Drawing.Point(97, 83);
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(157, 21);
            this.textBox_Port.TabIndex = 37;
            // 
            // textBox_Host
            // 
            this.textBox_Host.Location = new System.Drawing.Point(97, 32);
            this.textBox_Host.Name = "textBox_Host";
            this.textBox_Host.Size = new System.Drawing.Size(157, 21);
            this.textBox_Host.TabIndex = 36;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(26, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 35;
            this.label3.Text = "Port:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(26, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 34;
            this.label2.Text = "Host:";
            // 
            // tabPage_IPSetting
            // 
            this.tabPage_IPSetting.Controls.Add(this.btn_IPRefresh);
            this.tabPage_IPSetting.Controls.Add(this.btn_IPSetting);
            this.tabPage_IPSetting.Controls.Add(this.textBox_DNS);
            this.tabPage_IPSetting.Controls.Add(this.label12);
            this.tabPage_IPSetting.Controls.Add(this.textBox_Gate);
            this.tabPage_IPSetting.Controls.Add(this.label11);
            this.tabPage_IPSetting.Controls.Add(this.textBox_Mask);
            this.tabPage_IPSetting.Controls.Add(this.label10);
            this.tabPage_IPSetting.Controls.Add(this.textBox_IP);
            this.tabPage_IPSetting.Controls.Add(this.CheckBox_autoMode);
            this.tabPage_IPSetting.Controls.Add(this.label9);
            this.tabPage_IPSetting.Controls.Add(this.label8);
            this.tabPage_IPSetting.Location = new System.Drawing.Point(4, 22);
            this.tabPage_IPSetting.Name = "tabPage_IPSetting";
            this.tabPage_IPSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_IPSetting.Size = new System.Drawing.Size(408, 401);
            this.tabPage_IPSetting.TabIndex = 1;
            this.tabPage_IPSetting.Text = "IP Setting";
            this.tabPage_IPSetting.UseVisualStyleBackColor = true;
            // 
            // btn_IPRefresh
            // 
            this.btn_IPRefresh.Location = new System.Drawing.Point(101, 241);
            this.btn_IPRefresh.Name = "btn_IPRefresh";
            this.btn_IPRefresh.Size = new System.Drawing.Size(75, 23);
            this.btn_IPRefresh.TabIndex = 37;
            this.btn_IPRefresh.Text = "Refresh";
            this.btn_IPRefresh.UseVisualStyleBackColor = true;
            this.btn_IPRefresh.Click += new System.EventHandler(this.btn_IPRefresh_Click);
            // 
            // btn_IPSetting
            // 
            this.btn_IPSetting.Location = new System.Drawing.Point(187, 241);
            this.btn_IPSetting.Name = "btn_IPSetting";
            this.btn_IPSetting.Size = new System.Drawing.Size(75, 23);
            this.btn_IPSetting.TabIndex = 36;
            this.btn_IPSetting.Text = "Setting";
            this.btn_IPSetting.UseVisualStyleBackColor = true;
            this.btn_IPSetting.Click += new System.EventHandler(this.btn_IPSetting_Click);
            // 
            // textBox_DNS
            // 
            this.textBox_DNS.Location = new System.Drawing.Point(99, 187);
            this.textBox_DNS.Name = "textBox_DNS";
            this.textBox_DNS.Size = new System.Drawing.Size(161, 21);
            this.textBox_DNS.TabIndex = 35;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(30, 190);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 12);
            this.label12.TabIndex = 34;
            this.label12.Text = "DNS";
            // 
            // textBox_Gate
            // 
            this.textBox_Gate.Location = new System.Drawing.Point(99, 140);
            this.textBox_Gate.Name = "textBox_Gate";
            this.textBox_Gate.Size = new System.Drawing.Size(161, 21);
            this.textBox_Gate.TabIndex = 33;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(30, 143);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 32;
            this.label11.Text = "Gate";
            // 
            // textBox_Mask
            // 
            this.textBox_Mask.Location = new System.Drawing.Point(99, 94);
            this.textBox_Mask.Name = "textBox_Mask";
            this.textBox_Mask.Size = new System.Drawing.Size(161, 21);
            this.textBox_Mask.TabIndex = 31;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(30, 97);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 30;
            this.label10.Text = "Mask";
            // 
            // textBox_IP
            // 
            this.textBox_IP.Location = new System.Drawing.Point(99, 51);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(161, 21);
            this.textBox_IP.TabIndex = 29;
            // 
            // CheckBox_autoMode
            // 
            this.CheckBox_autoMode.AutoSize = true;
            this.CheckBox_autoMode.Location = new System.Drawing.Point(99, 20);
            this.CheckBox_autoMode.Name = "CheckBox_autoMode";
            this.CheckBox_autoMode.Size = new System.Drawing.Size(60, 16);
            this.CheckBox_autoMode.TabIndex = 28;
            this.CheckBox_autoMode.Text = "Enable";
            this.CheckBox_autoMode.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(30, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 27;
            this.label9.Text = "IP";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(30, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 26;
            this.label8.Text = "Auto mode";
            // 
            // listBox_Tips
            // 
            this.listBox_Tips.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBox_Tips.FormattingEnabled = true;
            this.listBox_Tips.ItemHeight = 12;
            this.listBox_Tips.Location = new System.Drawing.Point(152, 315);
            this.listBox_Tips.Name = "listBox_Tips";
            this.listBox_Tips.Size = new System.Drawing.Size(416, 112);
            this.listBox_Tips.TabIndex = 8;
            // 
            // Form_Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 427);
            this.Controls.Add(this.listBox_Tips);
            this.Controls.Add(this.tabControl_Setting);
            this.Controls.Add(this.listBox_Devices);
            this.Name = "Form_Setting";
            this.Text = "LocalSetting";
            this.Load += new System.EventHandler(this.Form_Setting_Load);
            this.tabControl_Setting.ResumeLayout(false);
            this.tabPage_ServerSetting.ResumeLayout(false);
            this.tabPage_ServerSetting.PerformLayout();
            this.tabPage_IPSetting.ResumeLayout(false);
            this.tabPage_IPSetting.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Devices;
        private System.Windows.Forms.TabControl tabControl_Setting;
        private System.Windows.Forms.TabPage tabPage_ServerSetting;
        private System.Windows.Forms.TabPage tabPage_IPSetting;
        private System.Windows.Forms.ListBox listBox_Tips;
        private System.Windows.Forms.Button btn_IPRefresh;
        private System.Windows.Forms.Button btn_IPSetting;
        private System.Windows.Forms.TextBox textBox_DNS;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_Gate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_Mask;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_IP;
        private System.Windows.Forms.CheckBox CheckBox_autoMode;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_Port;
        private System.Windows.Forms.TextBox textBox_Host;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_RefreshServerInfo;
        private System.Windows.Forms.Button button_ServerSetting;
    }
}

