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
    public partial class mySetButton : UserControl
    {

        public enum CheckStyle
        {
            style1 = 0,
            style2 = 1,
            style3 = 2,
        };

        public CheckStyle CheckStyleX
        {
            set { checkStyle = value; this.Invalidate(); }
            get { return checkStyle; }
        }

        private CheckStyle checkStyle;

        public mySetButton()
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
        bool isEnter = false;

        public bool Checked
        {
            set { isEnter = value; this.Invalidate(); }
            get { return isEnter; }
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap bitMapOn = null;
            Bitmap bitMapOff = null;
            
            if (checkStyle == CheckStyle.style1)
            {
                bitMapOn = global::RemoteServer.Properties.Resources.set;
                bitMapOff = global::RemoteServer.Properties.Resources.set1;
            }
            else if (checkStyle == CheckStyle.style2)
            {
                this.Size = new Size(21, 22);
                bitMapOn = global::RemoteServer.Properties.Resources.addBtn1;
                bitMapOff = global::RemoteServer.Properties.Resources.addBtn;
            }
            else if (checkStyle == CheckStyle.style3)
            {
                this.Size = new Size(21, 22);
                bitMapOn = global::RemoteServer.Properties.Resources.delete1;
                bitMapOff = global::RemoteServer.Properties.Resources.delete;
            }

            Graphics g = e.Graphics;
            Rectangle rec = new Rectangle(0, 0, this.Size.Width, this.Size.Height);

            if (isEnter)
            {
                g.DrawImage(bitMapOn, rec);
            }
            else
            {
                g.DrawImage(bitMapOff, rec);
            }
        }
        private void mouse_enter(object sender, EventArgs e)
        {
            isEnter = !isEnter;
            this.Invalidate();

        }

        private void mouse_leave(object sender, EventArgs e)
        {
            isEnter = !isEnter;
            this.Invalidate();

        }
    }
}
