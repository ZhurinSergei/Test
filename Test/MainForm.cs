using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ContourSearch.Enabled = false;
            Point.Image = Image.FromFile("Point.png");
            Point.Width = Point.Image.Width;
            Point.Height = Point.Image.Height;
        }

        private double[,] grayImage;

        private void OpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files(*.bmp;*.jpeg;*.tiff)|*.bmp;*.jpeg;*.tiff";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                Bitmap bmp = (Bitmap) pictureBox1.Image;

                if (bmp.Width > 2560 || bmp.Height > 1080)
                {
                    MessageBox.Show("Maximum size image 2560x1080");
                    return;
                }

                var pixels = new byte[bmp.Width, bmp.Height, 3];
                for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    pixels[x, y, 0] = pixel.R;
                    pixels[x, y, 1] = pixel.G;
                    pixels[x, y, 2] = pixel.B;
                }

                grayImage = ImageProccesing.Grayscale(pixels);

                ContourSearch.Enabled = true;
            }
        }

        private void Identify(object sender, EventArgs e)
        {
            Сoordinate[] contourСoordinates = ImageProccesing.AlgorithmBeetle(grayImage);

            Figure figure = new Figure(contourСoordinates);
            List<Сoordinate> controlPoints = figure.SearchControlPoints();

            string str;
            if (controlPoints.Count == 1)      str = "The picture shows a circle"   + Environment.NewLine;
            else if (controlPoints.Count == 4) str = "The picture shows a triangle" + Environment.NewLine;
            else if (controlPoints.Count == 5) str = "The picture shows a square"   + Environment.NewLine;
            else                               str = "The figure is not defined"    + Environment.NewLine;

            str += "Angular points - " + Environment.NewLine;
            for (int i = 0; i < controlPoints.Count - 1; i++)
            {
                str += "(" + controlPoints[i].x + ", " + controlPoints[i].y + "), " + Environment.NewLine;
            }
            str += "Geometric center - (" + controlPoints[controlPoints.Count - 1].x + ", " +
                   controlPoints[controlPoints.Count - 1].y + "), " + Environment.NewLine;

            if (controlPoints.Count == 1)
            {
                str += "Radius - " + figure.DistanceBetweenPoints(controlPoints[0], contourСoordinates[0]);
            }
            else if (controlPoints.Count > 3)
            {
                str += "Side length - " + figure.DistanceBetweenPoints(controlPoints[0], controlPoints[1]);
            }

            textBox1.Text = str;

            PrintPoint(controlPoints);
            PrintText(controlPoints);

            pictureBox1.Image = pictureBox1.Image;
            //pictureBox1.Image.Save("Image.bmp");
        }

        private void PrintText(List<Сoordinate> crds)
        {
            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {
                Font drawFont = new Font("Arial", 8);
                SolidBrush drawBrush = new SolidBrush(Color.Red);

                foreach (var crd in crds)
                {
                    g.DrawString("(" + crd.x + "," + crd.y + ")", drawFont, drawBrush, new Point(crd.x, crd.y + 10));
                }
            }
        }

        private void PrintPoint(List<Сoordinate> crds)
        {
            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {
                foreach (var crd in crds)
                {
                    int pointX = crd.x - (int) Math.Floor((double) Point.Width / 2);
                    int pointY = crd.y - (int) Math.Floor((double) Point.Height / 2);
                    Point point = new Point(pointX, pointY);

                    g.DrawImage(Point.Image, point);
                }
            }
        }
    }
}