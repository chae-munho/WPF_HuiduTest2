namespace RemoteServer
{
    partial class InitForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OK = new System.Windows.Forms.Button();
            this.Cancle = new System.Windows.Forms.Button();
            this.lab_Port = new System.Windows.Forms.Label();
            this.lab_Tips = new System.Windows.Forms.Label();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.label_IP = new System.Windows.Forms.Label();
            this.radioButtonSever = new System.Windows.Forms.RadioButton();
            this.radioButtonClient = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(63, 207);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 0;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancle
            // 
            this.Cancle.Location = new System.Drawing.Point(159, 206);
            this.Cancle.Name = "Cancle";
            this.Cancle.Size = new System.Drawing.Size(75, 23);
            this.Cancle.TabIndex = 1;
            this.Cancle.Text = "Cancle";
            this.Cancle.UseVisualStyleBackColor = true;
            this.Cancle.Click += new System.EventHandler(this.Cancle_Click);
            // 
            // lab_Port
            // 
            this.lab_Port.Location = new System.Drawing.Point(41, 119);
            this.lab_Port.Name = "lab_Port";
            this.lab_Port.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lab_Port.Size = new System.Drawing.Size(89, 12);
            this.lab_Port.TabIndex = 2;
            this.lab_Port.Text = "Port:";
            // 
            // lab_Tips
            // 
            this.lab_Tips.Location = new System.Drawing.Point(36, 158);
            this.lab_Tips.Name = "lab_Tips";
            this.lab_Tips.Size = new System.Drawing.Size(261, 45);
            this.lab_Tips.TabIndex = 2;
            this.lab_Tips.Text = "端口被占用";
            this.lab_Tips.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lab_Tips.Visible = false;
            // 
            // textBox_Port
            // 
            this.textBox_Port.Location = new System.Drawing.Point(136, 112);
            this.textBox_Port.MaxLength = 5;
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(161, 21);
            this.textBox_Port.TabIndex = 4;
            this.textBox_Port.Text = "10001";
            this.textBox_Port.WordWrap = false;
            this.textBox_Port.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_Port_KeyPress);
            // 
            // textBox_IP
            // 
            this.textBox_IP.Enabled = false;
            this.textBox_IP.Location = new System.Drawing.Point(136, 65);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(161, 21);
            this.textBox_IP.TabIndex = 31;
            this.textBox_IP.Text = "192.168.1.24";
            // 
            // label_IP
            // 
            this.label_IP.Location = new System.Drawing.Point(41, 70);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(89, 12);
            this.label_IP.TabIndex = 30;
            this.label_IP.Text = "Device IP:";
            // 
            // radioButtonSever
            // 
            this.radioButtonSever.AutoSize = true;
            this.radioButtonSever.Checked = true;
            this.radioButtonSever.Location = new System.Drawing.Point(26, 27);
            this.radioButtonSever.Name = "radioButtonSever";
            this.radioButtonSever.Size = new System.Drawing.Size(77, 16);
            this.radioButtonSever.TabIndex = 32;
            this.radioButtonSever.TabStop = true;
            this.radioButtonSever.Text = "As Server";
            this.radioButtonSever.UseVisualStyleBackColor = true;
            this.radioButtonSever.CheckedChanged += new System.EventHandler(this.radioButtonSever_CheckedChanged);
            // 
            // radioButtonClient
            // 
            this.radioButtonClient.AutoSize = true;
            this.radioButtonClient.Location = new System.Drawing.Point(177, 27);
            this.radioButtonClient.Name = "radioButtonClient";
            this.radioButtonClient.Size = new System.Drawing.Size(77, 16);
            this.radioButtonClient.TabIndex = 32;
            this.radioButtonClient.TabStop = true;
            this.radioButtonClient.Text = "As Client";
            this.radioButtonClient.UseVisualStyleBackColor = true;
            this.radioButtonClient.CheckedChanged += new System.EventHandler(this.radioButtonClient_CheckedChanged);
            // 
            // InitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 245);
            this.Controls.Add(this.radioButtonClient);
            this.Controls.Add(this.radioButtonSever);
            this.Controls.Add(this.textBox_IP);
            this.Controls.Add(this.label_IP);
            this.Controls.Add(this.textBox_Port);
            this.Controls.Add(this.lab_Tips);
            this.Controls.Add(this.lab_Port);
            this.Controls.Add(this.Cancle);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InitForm";
            this.Text = "InitForm";
            this.Load += new System.EventHandler(this.InitForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancle;
        private System.Windows.Forms.Label lab_Port;
        private System.Windows.Forms.Label lab_Tips;
        private System.Windows.Forms.TextBox textBox_Port;
        private System.Windows.Forms.TextBox textBox_IP;
        private System.Windows.Forms.Label label_IP;
        private System.Windows.Forms.RadioButton radioButtonSever;
        private System.Windows.Forms.RadioButton radioButtonClient;
    }
}