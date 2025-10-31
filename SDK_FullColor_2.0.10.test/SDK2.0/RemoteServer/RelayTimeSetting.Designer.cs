
namespace RemoteServer
{
    partial class RelayTimeSetting
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mySetButton1 = new RemoteServer.mySetButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.checkLable = new System.Windows.Forms.Label();
            this.useswitch = new System.Windows.Forms.Label();
            this.useswitchBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AllowDrop = true;
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 124F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 167F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.mySetButton1, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(397, 43);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(132, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "断开时间";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(5, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "连接时间";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mySetButton1
            // 
            this.mySetButton1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mySetButton1.BackColor = System.Drawing.Color.Transparent;
            this.mySetButton1.Checked = false;
            this.mySetButton1.CheckStyleX = RemoteServer.mySetButton.CheckStyle.style2;
            this.mySetButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mySetButton1.Location = new System.Drawing.Point(302, 10);
            this.mySetButton1.Name = "mySetButton1";
            this.mySetButton1.Size = new System.Drawing.Size(21, 22);
            this.mySetButton1.TabIndex = 2;
            this.mySetButton1.Click += new System.EventHandler(this.mySetButton1_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(22, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(428, 208);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 267);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "继电器名称：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(114, 264);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 5;
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(268, 317);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 6;
            this.OKBtn.Text = "确定";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(349, 317);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 6;
            this.CancelBtn.Text = "取消";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // checkLable
            // 
            this.checkLable.AutoSize = true;
            this.checkLable.Location = new System.Drawing.Point(33, 236);
            this.checkLable.Name = "checkLable";
            this.checkLable.Size = new System.Drawing.Size(0, 12);
            this.checkLable.TabIndex = 7;
            // 
            // useswitch
            // 
            this.useswitch.AutoSize = true;
            this.useswitch.Location = new System.Drawing.Point(255, 267);
            this.useswitch.Name = "useswitch";
            this.useswitch.Size = new System.Drawing.Size(89, 12);
            this.useswitch.TabIndex = 4;
            this.useswitch.Text = "同步显示屏状态";
            // 
            // useswitchBox
            // 
            this.useswitchBox.AutoSize = true;
            this.useswitchBox.Location = new System.Drawing.Point(346, 266);
            this.useswitchBox.Name = "useswitchBox";
            this.useswitchBox.Size = new System.Drawing.Size(15, 14);
            this.useswitchBox.TabIndex = 8;
            this.useswitchBox.UseVisualStyleBackColor = true;
            // 
            // RelayTimeSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.useswitchBox);
            this.Controls.Add(this.checkLable);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.useswitch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.flowLayoutPanel1);
            this.MaximumSize = new System.Drawing.Size(500, 400);
            this.Name = "RelayTimeSetting";
            this.Text = "RelayTimeSetting";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RelayTimeSetting_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private mySetButton mySetButton1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label checkLable;
        private System.Windows.Forms.Label useswitch;
        private System.Windows.Forms.CheckBox useswitchBox;
    }
}