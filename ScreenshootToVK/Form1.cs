using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace ScreenshootToVK
{
    public partial class Form1 : Form
    {
        // координаты для выделения
        int selectX; 
        int selectY;
        int selectWidth;
        int selectHeight;
        public System.Drawing.Pen selector;
        bool start = false;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // скрываем форму
            this.Hide();
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            // переменная графики из скрина экрана
            Graphics graphics = Graphics.FromImage(bmp as Image);
            // считываем скриншот
            graphics.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            // memorystream для скриншота
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                pictureBox1.Size = new System.Drawing.Size(this.Width, this.Height);
                pictureBox1.Image = Image.FromStream(ms);
            }
            // показываем форму
            this.Show();
            Cursor = Cursors.Cross;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // если ли картинка
            if (pictureBox1.Image != null)
            {
                return;
            }
            if (start)
            {
                pictureBox1.Refresh();
                selectWidth = e.X - selectX;
                selectHeight = e.Y - selectY;
                pictureBox1.CreateGraphics().DrawRectangle(selector, selectX, selectY, selectWidth, selectHeight);

            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!start)
            {
                if (e.Button == MouseButtons.Left)
                {
                    selectX = e.X;
                    selectY = e.Y;
                    selector = new System.Drawing.Pen(System.Drawing.Color.Red, 2);
                    selector.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                }
                pictureBox1.Refresh();
                start = true;
            }
            else
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }
                if (e.Button == MouseButtons.Left)
                {
                    pictureBox1.Refresh();
                    selectWidth = e.X - selectX;
                    selectHeight = e.Y - selectY;
                    pictureBox1.CreateGraphics().DrawRectangle(selector, selectX, selectY, selectWidth, selectHeight);
                }
                start = false;
                SaveToClipboard();
            }
        }
        private void SaveToClipboard()
        {
            //validate if something selected
            if (selectWidth > 0)
            {

                Rectangle rect = new Rectangle(selectX, selectY, selectWidth, selectHeight);
                //create bitmap with original dimensions
                Bitmap OriginalImage = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);
                //create bitmap with selected dimensions
                Bitmap _img = new Bitmap(selectWidth, selectHeight);
                //create graphic variable
                Graphics g = Graphics.FromImage(_img);
                //set graphic attributes
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);
                //insert image stream into clipboard
                Clipboard.SetImage(_img);
            }
            //End application
            Application.Exit();
        }
    }
}
