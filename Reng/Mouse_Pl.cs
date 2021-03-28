using System.Drawing;
using System.Windows.Forms;

namespace Reng
{
    public partial class Mouse_Pl : UserControl
    {
        public Mouse_Pl()
        {
            InitializeComponent();
        }
        public string LblText
        {
            get { return lblColor.Text; }
            set { lblColor.Text = value; }
        }
        public Color lblBackColor
        {
            get { return lblColor.BackColor; }
            set
            {
                lblColor.BackColor = value;
                if (lblBackColor.R * 0.2126 + lblBackColor.G * 0.7152 + lblBackColor.B * 0.0722 > 255 / 2)
                {
                    lblColor.ForeColor = Color.Black;
                }
                else
                {
                    lblColor.ForeColor = Color.Gainsboro;
                }
            }
        }
        public Image image
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }

    }
}
