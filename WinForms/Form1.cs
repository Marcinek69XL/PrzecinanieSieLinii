using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WinForms
{
    public partial class Form1 : Form
    {
        int Ax, Ay, Bx, By, Cx, Cy, Dx, Dy;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ax = int.Parse(AxTextBox.Text);
            Ay = int.Parse(AyTextBox.Text);
            Bx = int.Parse(BxTextBox.Text);
            By = int.Parse(ByTextBox.Text);
            Cx = int.Parse(CxTextBox.Text);
            Cy = int.Parse(CyTextBox.Text);
            Dy = int.Parse(DxTextBox.Text);
            Dy = int.Parse(DyTextBox.Text);

            Graphics g = pictureBox1.CreateGraphics();
            int rozpietoscX = 20;
            int rozpietoscY = 20;

            for (int y = 0; y < pictureBox1.Height; y += rozpietoscY)
            {
                g.DrawLine(Pens.Black, 0, y, pictureBox1.Width, y);
            }

            for (int x = 0; x < pictureBox1.Width; x += rozpietoscX)
            {
                g.DrawLine(Pens.Black, x, 0, x, pictureBox1.Height);
            }

            bool przecinajaSie = czySiePrzecinaja();
            if (przecinajaSie) MessageBox.Show("Linie przecinają się");
            else MessageBox.Show("Linie nie przecinają się");
        }

        private bool czySiePrzecinaja()
        {
            int v1 = iloczynWektorowy(Ax, Ay, Bx, By, Cx, Cy);
            int v2 = iloczynWektorowy(Ax, Ay, Bx, By, Dx, Dy);
            int v3 = iloczynWektorowy(Cx, Cy, Dx, Dy, Ax, Ay);
            int v4 = iloczynWektorowy(Cx, Cy, Dx, Dy, Bx, By);

            //sprawdzenie czy się przecinają
            if ((v3 > 0 && v4 < 0 || v3 < 0 && v4 > 0) && (v1 > 0 && v2 < 0 || v1 < 0 && v2 > 0)) return true;

            //sprawdzenie czy koniec odcinka leży na drugim
            if (v1 == 0 && sprawdz(Ax, Ay, Bx, By, Cx, Cy)) return true;
            if (v2 == 0 && sprawdz(Ax, Ay, Bx, By, Dx, Dy)) return true;
            if (v3 == 0 && sprawdz(Cx, Cy, Dx, Dy, Ax, Ay)) return true;
            if (v4 == 0 && sprawdz(Cx, Cy, Dx, Dy, Bx, By)) return true;

            return false;
        }

        private int iloczynWektorowy(int Xa, int Xb, int Ya, int Yb, int Za, int Zb)
        {
            int x1 = Za - Xa;
            int y1 = Zb - Xb;

            int x2 = Ya - Xa;
            int y2 = Yb - Xb;

            return (x1 * y2) - (x2 * y1);
        }

        private bool sprawdz(int Xa, int Xb, int Ya, int Yb, int Za, int Zb)
        {
            return Math.Min(Xa, Ya) <= Za && Zb <= Math.Max(Xa, Ya) && Math.Min(Xb, Yb) <= Zb && Zb <= Math.Max(Xb, Yb);
        }

        /*private void Rysuj()
        {
            var e = new Graphics();

            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));
            e.Graphics.DrawLine(pen, 20, 10, 300, 100);
        }*/

        private void tylkoLiczby(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            /*if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }*/
        }

        private void BxTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            tylkoLiczby(e);
        }

        private void ByTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            tylkoLiczby(e);
        }

        private void CxTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            tylkoLiczby(e);
        }

        private void CyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            tylkoLiczby(e);
        }

        private void DxTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            tylkoLiczby(e);
        }

        private void DyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            tylkoLiczby(e);
        }

        private void AyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            tylkoLiczby(e);
        }

        private void AxTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            tylkoLiczby(e);
        }
    }
}
