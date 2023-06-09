using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;
using WinForms.Controllers;
using Label = System.Windows.Forms.Label;

namespace WinForms
{
    public partial class Form1 : Form
    {
        private MyPoint A, B, C, D;
        Label labelA, labelB, labelC, labelD;

        private IInsertionController _insertionController;
        private IScaleController _scaleController;

        public Form1()
        {
            InitializeComponent();

            StartUpData();

            _insertionController = new InsertionContoller();
            _scaleController = new ScaleController();

            A = new MyPoint(0,0);
            B = new MyPoint(0,0);
            C = new MyPoint(0,0);
            D = new MyPoint(0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
                return;
            
            A.X = double.Parse(AxTextBox.Text);
            A.Y = double.Parse(AyTextBox.Text);
            B.X = double.Parse(BxTextBox.Text);
            B.Y = double.Parse(ByTextBox.Text);
            C.X = double.Parse(CxTextBox.Text);
            C.Y = double.Parse(CyTextBox.Text);
            D.X = double.Parse(DxTextBox.Text);
            D.Y = double.Parse(DyTextBox.Text);

            Draw();

            bool przecinajaSie = _insertionController.CzySiePrzecinaja(A, B, C, D);
            
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

            var currentWidth = pictureBox1.Width;
            var currentHeight = pictureBox1.Height;

            g.Clear(Color.White);
            for (int y = 0; y < pictureBox1.Height; y += rozpietoscY)
            {
                g.DrawLine(Pens.Black, 0, y, pictureBox1.Width, y);
            }

            for (int x = 0; x < pictureBox1.Width; x += rozpietoscX)
            {
                g.DrawLine(Pens.Black, x, 0, x, pictureBox1.Height);
            }

            float arrowSize = 10;
            float lineWidth = 5;

            // Ustawienie grubości linii
            Pen axisPen = new Pen(Color.Black, lineWidth);

            // Początek i koniec osi X
            PointF startX = new PointF(arrowSize, currentHeight / 2);
            PointF endX = new PointF(currentWidth - arrowSize, currentHeight / 2);

            // Początek i koniec osi Y
            PointF startY = new PointF(currentWidth / 2, currentHeight - arrowSize);
            PointF endY = new PointF(currentWidth / 2, arrowSize);

            // Narysuj osie X i Y z grubością linii
            g.DrawLine(axisPen, startX, endX);
            g.DrawLine(axisPen, startY, endY);

            // Narysuj strzałki na końcach osi
            DrawArrow(g, startX, endX, arrowSize, lineWidth);
            DrawArrow(g, startY, endY, arrowSize, lineWidth);



            var pointsToDraw = _scaleController.ComplexScale(A, B, C, D, currentWidth, currentHeight);

            var drawA = pointsToDraw[0];
            var drawB = pointsToDraw[1];
            var drawC = pointsToDraw[2];
            var drawD = pointsToDraw[3];

            // Narysowanie prostej między punktami A i B
            Pen linePen1 = new Pen(Color.Red, 5);
            Pen linePen2 = new Pen(Color.Green, 5);

            g.DrawLine(linePen1, (float)drawA.X, (float) drawA.Y, (float)drawB.X, (float)drawB.Y);
            g.DrawLine(linePen2, (float)drawC.X, (float)drawC.Y, (float)drawD.X, (float)drawD.Y);


            labelA?.Dispose();
            labelB?.Dispose();
            labelC?.Dispose();
            labelD?.Dispose();

            labelA = new Label();
            labelA.Text = $"({A.X},{A.Y})";

            labelB = new Label();
            labelB.Text = $"({B.X},{B.Y})";

            labelC = new Label();
            labelC.Text = $"({C.X},{C.Y})";

            labelD = new Label();
            labelD.Text = $"({D.X},{D.Y})";

            var pictureBoxLocX = pictureBox1.Location.X;
            var pictureBoxLocY = pictureBox1.Location.Y;

            labelA.Location = new Point((int) drawA.X + pictureBoxLocX, (int) drawA.Y + pictureBoxLocY);
            this.Controls.Add(labelA);
            labelB.Location = new Point((int)drawB.X + pictureBoxLocX, (int)drawB.Y + pictureBoxLocY);
            this.Controls.Add(labelB);
            labelC.Location = new Point((int)drawC.X + pictureBoxLocX, (int)drawC.Y + pictureBoxLocY);
            this.Controls.Add(labelC);
            labelD.Location = new Point((int)drawD.X + pictureBoxLocX, (int)drawD.Y + pictureBoxLocY);
            this.Controls.Add(labelD);

            DrawTransparentText(g, labelA.Text, new Font("Arial", 12), new Rectangle((int) drawA.X, (int)drawA.Y, 100, 30), Color.Black);
            DrawTransparentText(g, labelB.Text, new Font("Arial", 12), new Rectangle((int)drawB.X, (int)drawB.Y, 100, 30), Color.Black);
            DrawTransparentText(g, labelC.Text, new Font("Arial", 12), new Rectangle((int)drawC.X, (int)drawC.Y, 100, 30), Color.Black);
            DrawTransparentText(g, labelD.Text, new Font("Arial", 12), new Rectangle((int)drawD.X, (int)drawD.Y, 100, 30), Color.Black);

            //labelA.BringToFront();
            //labelB.BringToFront();
            //labelC.BringToFront();
            //labelD.BringToFront();
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
                if (!control.TryTodouble(out _))
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

        private void StartUpData()
        {
            AxTextBox.Text = "552";
            BxTextBox.Text = "42";
            CxTextBox.Text = "55";
            DxTextBox.Text = "222";
            AyTextBox.Text = "79";
            ByTextBox.Text = "340";
            CyTextBox.Text = "155";
            DyTextBox.Text = "444";
        }


        private void DrawArrow(Graphics graphics, PointF startPoint, PointF endPoint, float arrowSize, float lineWidth)
        {
            float angle = (float)Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X);
            PointF[] arrowPoints = new PointF[3];
            arrowPoints[0] = new PointF(endPoint.X - arrowSize * (float)Math.Cos(angle - Math.PI / 6),
                endPoint.Y - arrowSize * (float)Math.Sin(angle - Math.PI / 6));
            arrowPoints[1] = endPoint;
            arrowPoints[2] = new PointF(endPoint.X - arrowSize * (float)Math.Cos(angle + Math.PI / 6),
                endPoint.Y - arrowSize * (float)Math.Sin(angle + Math.PI / 6));

            using (Pen arrowPen = new Pen(Color.Black, lineWidth))
            {
                graphics.DrawLines(arrowPen, arrowPoints);
            }
        }

        private void DrawTransparentText(Graphics graphics, string text, Font font, Rectangle bounds, Color textColor)
        {
            TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter |
                                    TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

            using (SolidBrush brush = new SolidBrush(textColor))
            {
                TextRenderer.DrawText(graphics, text, font, bounds, textColor, flags);
            }
        }

    }
}
