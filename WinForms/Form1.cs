using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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


            g.Clear(Color.White);
            for (int y = 0; y < pictureBox1.Height; y += rozpietoscY)
            {
                g.DrawLine(Pens.Black, 0, y, pictureBox1.Width, y);
            }

            for (int x = 0; x < pictureBox1.Width; x += rozpietoscX)
            {
                g.DrawLine(Pens.Black, x, 0, x, pictureBox1.Height);
            }


            var currentWidth = pictureBox1.Width;
            var currentHeight = pictureBox1.Height;

            // Jak punkty poza skalą planszy, to trzeba zrobic cos w stylu skalowania

            var drawA = new MyPoint(0, 0);
            var drawB = new MyPoint(0, 0);
            var drawC = new MyPoint(0, 0);
            var drawD = new MyPoint(0, 0);

            //Skalowanie "w góre"

            var k = 1.1m;
            var l = 0.9m;

            while (true)
            {
                if (A.X * k > currentWidth || A.Y * k > currentHeight
                                           || B.X * k > currentWidth || B.Y * k > currentHeight
                                           || C.X * k > currentWidth || C.Y * k > currentHeight
                                           || D.X * k > currentWidth || D.Y * k > currentHeight
                   )
                {
                    A.X = l * A.X;
                    B.X = l * B.X;
                    C.X = l * C.X;
                    D.X = l * D.X;

                    A.Y = l * A.Y;
                    B.Y = l * B.Y;
                    C.Y = l * C.Y;
                    D.Y = l * D.Y;
                }
                else
                {
                    break;
                }
            }
            

            // Skalowanie "w dół"
            if (A.X > currentWidth || A.Y > currentHeight 
            || B.X > currentWidth || B.Y > currentHeight
            || C.X > currentWidth || C.Y > currentHeight
            || D.X > currentWidth || D.Y > currentHeight
                )
            {
                var factors = new List<decimal>();
                var scaleH1 = A.X / currentWidth;
                factors.Add(scaleH1);
                var scaleW1 = A.Y / currentHeight;
                factors.Add(scaleW1);

                var scaleH2 = B.X / currentWidth;
                factors.Add(scaleH2);
                var scaleW2 = B.Y / currentHeight;
                factors.Add(scaleW2);

                var scaleH3 = C.X / currentWidth;
                factors.Add(scaleH3);
                var scaleW3 = C.Y / currentHeight;
                factors.Add(scaleW3);

                var scaleH4 = D.X / currentWidth;
                factors.Add(scaleH4);
                var scaleW4 = D.Y / currentHeight;
                factors.Add(scaleW4);

                var factorToApply = factors.Max();

                drawA = new MyPoint
                {
                    X = A.X / factorToApply,
                    Y = A.Y / factorToApply
                };
                drawB = new MyPoint
                {
                    X = B.X / factorToApply,
                    Y = B.Y / factorToApply
                };
                drawC = new MyPoint
                {
                    X = C.X / factorToApply,
                    Y = C.Y / factorToApply
                };
                drawD = new MyPoint
                {
                    X = D.X / factorToApply,
                    Y = D.Y / factorToApply
                };
            }
            else
            {
                // Narysowanie prostej między punktami A i B
                Pen linePen1 = new Pen(Color.Red, 5);
                Pen linePen2 = new Pen(Color.Green, 5);

                g.DrawLine(linePen1, (float) A.X, currentHeight - (float)A.Y, (float)B.X, currentHeight - (float)B.Y);
                g.DrawLine(linePen2, (float) C.X, currentHeight - (float)C.Y, (float)D.X, currentHeight - (float)D.Y);
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
