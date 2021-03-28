//https://github.com/Diyari-Kurd
//https://icons8.com/ //Icons & Cursor

using Reng.Properties;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Reng
{
    public partial class Form_Main : Form
    {
        private int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        private int screenHeight = Screen.PrimaryScreen.Bounds.Height;

        [DllImport("User32.dll")]
        private static extern Int32 SetForegroundWindow(int hWnd);

        public Form_Main()
        {
            InitializeComponent();
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            this.Size = new Size(screenWidth, screenHeight);
        }

        private void capture()
        {
            try
            {
                SetForegroundWindow(Handle.ToInt32());
                captureBm = new Bitmap(screenWidth, screenHeight, PixelFormat.Format32bppArgb);
                Rectangle Rectangle = Screen.AllScreens[0].Bounds;
                Graphics Graphics = Graphics.FromImage(captureBm);

                Graphics.CopyFromScreen(Rectangle.Left, Rectangle.Top, 0, 0, Rectangle.Size);

                this.Opacity = 100;
                this.WindowState = FormWindowState.Maximized;
                this.BackgroundImage = captureBm;
                this.Controls.Add(_Pl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private Cursor BlackCur = new Cursor(new MemoryStream(Resources.target_Black));
        private Cursor WhiteCur = new Cursor(new MemoryStream(Resources.target_White));
        private void ChangeCursorColor(Color backColor)
        {
            if (backColor.R * 0.2126 + backColor.G * 0.7152 + backColor.B * 0.0722 > 255 / 2)
            {
                if (this.Cursor != BlackCur)
                    this.Cursor = BlackCur;
            }
            else
            {
                if (this.Cursor != WhiteCur)
                    this.Cursor = WhiteCur;
            }
        }
        private Color Pclr;
        private Bitmap captureBm;

        private Mouse_Pl _Pl = new Mouse_Pl();
        private void Form_Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.BackgroundImage != null)
            {

                if (e.X + 180 > screenWidth && e.Y + 80 < screenHeight)
                {
                    _Pl.Location = new Point(e.X - 180, e.Y + 30);
                }
                else if (e.Y + 110 > screenHeight && e.X + 180 < screenWidth)
                {
                    _Pl.Location = new Point(e.X, e.Y - 110);
                }
                else if (e.X + 180 > screenWidth && e.Y + 80 > screenHeight)
                {
                    _Pl.Location = new Point(e.X - 200, e.Y - 110);
                }
                else
                {
                    _Pl.Location = new Point(e.X, e.Y + 30);
                }
                try
                {

                    Pclr = captureBm.GetPixel(e.X, e.Y);
                    if (rGBToolStripMenuItem.Checked)
                    {
                        _Pl.LblText = $"R: {Pclr.R} G: {Pclr.G} B: {Pclr.B}";
                    }
                    else if (hexToolStripMenuItem.Checked)
                    {
                        _Pl.LblText = "#" + Pclr.R.ToString("X2") + Pclr.G.ToString("X2") + Pclr.B.ToString("X2");
                    }
                    _Pl.lblBackColor = Pclr;
                    ChangeCursorColor(Pclr);
                    _Pl.image = captureBm.Clone(new Rectangle(new Point(e.X - 5, e.Y), new Size(10, 10)), captureBm.PixelFormat);

                }
                catch { }
            }
        }


        private void Form_Main_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                if (rGBToolStripMenuItem.Checked)
                {
                    Clipboard.SetText($"{Pclr.R},{Pclr.G},{Pclr.B}");
                }
                else if (hexToolStripMenuItem.Checked)
                {
                    Clipboard.SetText("#" + Pclr.R.ToString("X2") + Pclr.G.ToString("X2") + Pclr.B.ToString("X2"));
                }
                this.Opacity = 0;
                this.BackgroundImage = null;
                captureBm = null;
                WindowState = FormWindowState.Minimized;
            }
        }

        #region NotifyIcon & ToolStripMenuItem
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                capture();

        }

        private void CheckBoxs(ToolStripMenuItem item)
        {

            foreach (ToolStripMenuItem itm in contextMenuStrip.Items.OfType<ToolStripMenuItem>())
            {
                itm.Checked = false;
            }

            item.Checked = true;
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void rGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckBoxs(rGBToolStripMenuItem);
        }

        private void hexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckBoxs(hexToolStripMenuItem);
        }

        #endregion

    }
}
