using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteServer
{

    
    public partial class myButtonCheck : UserControl
    {
        public enum CheckStyle
        {
            style1 = 0,
            style2 = 1,
        };
        private CheckStyle checkStyle;
        public myButtonCheck()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.BackColor = Color.Transparent;

            this.Cursor = Cursors.Hand;
            this.Size = new Size(87, 27);
            checkStyle = CheckStyle.style1;
        }

        /// <summary>
        /// 样式
        /// </summary>
        public CheckStyle CheckStyleX
        {
            set { checkStyle = value; this.Invalidate(); }
            get { return checkStyle; }
        }

        bool isCheck = false;

        public bool Checked
        {
            set { isCheck = value; this.Invalidate(); }
            get { return isCheck; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap bitMapOn = null;
            Bitmap bitMapOff = null;

            if(checkStyle == CheckStyle.style1)
            {
                bitMapOn = global::RemoteServer.Properties.Resources.btncheckon1;
                bitMapOff = global::RemoteServer.Properties.Resources.btncheckoff1;
            } 
            else if (checkStyle == CheckStyle.style2)
            {
                this.Size = new Size(21, 22);
                bitMapOn = global::RemoteServer.Properties.Resources.addBtn1;
                bitMapOff = global::RemoteServer.Properties.Resources.addBtn;
            }
            

            Graphics g = e.Graphics;
            Rectangle rec = new Rectangle(0, 0, this.Size.Width, this.Size.Height);

            if (isCheck)
            {
                g.DrawImage(bitMapOn, rec);
            }
            else
            {
                g.DrawImage(bitMapOff, rec);
            }
        }
        private void myButtonCheck_Click(object sender, EventArgs e)
        {
            isCheck = !isCheck;
            this.Invalidate();
            
        }
    }
}
