using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinForms
{
    public partial class Form1 : Form
    {
        private MyPoint A, B, C, D;

        private IInsertionController _controller;

        public Form1()
        {
            InitializeComponent();

            _controller = new InsertionContoller();

            A = new MyPoint(0,0);
            B = new MyPoint(0,0);
            C = new MyPoint(0,0);
            D = new MyPoint(0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
                return;
            
            A.X = decimal.Parse(AxTextBox.Text);
            A.Y = decimal.Parse(AyTextBox.Text);
            B.X = decimal.Parse(BxTextBox.Text);
            B.Y = decimal.Parse(ByTextBox.Text);
            C.X = decimal.Parse(CxTextBox.Text);
            C.Y = decimal.Parse(CyTextBox.Text);
            D.X = decimal.Parse(DxTextBox.Text);
            D.Y = decimal.Parse(DyTextBox.Text);

            Draw();

            bool przecinajaSie = _controller.CzySiePrzecinaja(A, B, C, D);
            
            if (przecinajaSie)
                CustomMessageBox.Info("Linie przecinają się");
            else
                CustomMessageBox.Info("Linie nie przecinają się");
        }

        private void Draw()
        {
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
        }

        private bool ValidateData()
        {
            var controls = new List<TextBox>()
            {
                AxTextBox,
                AyTextBox,
                BxTextBox,
                ByTextBox,
                CxTextBox,
                CyTextBox,
                DxTextBox,
                DyTextBox,
            };

            var ok = true;
            foreach (var control in controls)
            {
                if (!control.TryToDecimal(out _))
                    ok = false;
            }

            if (!ok)
            {
                CustomMessageBox.Error("Wprowadzono błędne dane wejściowe");
                return false;
            }
            return true; 
        }

        /*private void Rysuj()
        {
            var e = new Graphics();

            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));
            e.Graphics.DrawLine(pen, 20, 10, 300, 100);
        }*/

        private void Form1_Resize(object sender, EventArgs e)
        {
            Draw();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            Draw();
        }
    }
}
