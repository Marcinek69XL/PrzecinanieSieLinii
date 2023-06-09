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
        static class CheckBoxState
        {
            internal static readonly string On = "Ukryj";
            internal static readonly string Off = "Pokaż";
        }

        private MyPoint A, B, C, D;
        Label labelA, labelB, labelC, labelD;

        /// <summary>
        /// Nie uzywaj do odczytu, uzyj propercje
        /// </summary>
        private Color? _colorLine1;
        private Color ColorLine1
        {
            get
            {
                if (_colorLine1 == null)
                    return Color.Black;
                return _colorLine1.Value;
            }
            set
            {
                _colorLine1 = value;
            }
        }
        /// <summary>
        /// Nie uzywaj do odczytu, uzyj propercje
        /// </summary>
        private Color? _colorLine2;
        private Color ColorLine2
        {
            get
            {
                if (_colorLine2 == null)
                    return Color.Black;
                return _colorLine2.Value;
            }
            set
            {
                _colorLine2 = value;
            }
        }

        /// <summary>
        /// Nie uzywaj do odczytu, uzyj propercje
        /// </summary>
        private Color? _colorAxis;
        private Color ColorAxis 
        {
            get
            {
                if (_colorAxis == null)
                    return Color.Black;
                return _colorAxis.Value;
            }
        }

        /// <summary>
        /// Nie uzywaj do odczytu, uzyj propercje
        /// </summary>
        private Color? _colorNet;
        private Color ColorNet
        {
            get
            {
                if (_colorNet == null)
                    return Color.Black;
                else
                    return _colorNet.Value;
            }
        }

        private float _line1Width;
        private float _line2Width;
        private float _axisWidth;
        private float _netWidth;

        private IInsertionController _insertionController;
        private IScaleController _scaleController;

        public Form1()
        {
            InitializeComponent();

            StartUpData();

            _insertionController = new InsertionContoller();
            _scaleController = new ScaleController();

            A = null;
            B = null;
            C = null;
            D = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
                return;

            InitPoints();

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

        private void InitPoints()
        {
            A = new MyPoint();
            B = new MyPoint();
            C = new MyPoint();
            D = new MyPoint();
        }

        private void Draw()
        {
            if (A == null || B == null || C == null || D == null)
                return;;

            Graphics g = pictureBox1.CreateGraphics();
            int rozpietoscX = 20;
            int rozpietoscY = 20;

            var currentWidth = pictureBox1.Width;
            var currentHeight = pictureBox1.Height;

            g.Clear(Color.White);

            var pen = new Pen(ColorNet);
            for (int y = 0; y < pictureBox1.Height; y += rozpietoscY)
            {
                g.DrawLine(pen, 0, y, pictureBox1.Width, y);
            }

            for (int x = 0; x < pictureBox1.Width; x += rozpietoscX)
            {
                g.DrawLine(pen, x, 0, x, pictureBox1.Height);
            }

            DrawAxis(g, ColorAxis);

            var pointsToDraw = _scaleController.ComplexScale(A, B, C, D, currentWidth, currentHeight);

            var drawA = pointsToDraw[0];
            var drawB = pointsToDraw[1];
            var drawC = pointsToDraw[2];
            var drawD = pointsToDraw[3];

            // Narysowanie prostej między punktami A i B
            Pen linePen1 = new Pen(ColorLine1, _line1Width);
            Pen linePen2 = new Pen(ColorLine2, _line2Width);

            g.DrawLine(linePen1, (float)drawA.X, (float) drawA.Y, (float)drawB.X, (float)drawB.Y);
            g.DrawLine(linePen2, (float)drawC.X, (float)drawC.Y, (float)drawD.X, (float)drawD.Y);

            Console.WriteLine(string.Format("A=({0};{1})", (int)drawA.X, (int)drawA.Y));
            Console.WriteLine(string.Format("B=({0};{1})", (int)drawB.X, (int)drawB.Y));
            Console.WriteLine(string.Format("C=({0};{1})", (int)drawC.X, (int)drawC.Y));
            Console.WriteLine(string.Format("D=({0};{1})", (int)drawD.X, (int)drawD.Y));
            Console.WriteLine(String.Format("H = {0}", currentHeight));
            Console.WriteLine(String.Format("W = {0}", currentWidth));
            Console.WriteLine();

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

            DrawTransparentText(g, labelA.Text, new Font("Arial", 12), new Rectangle((int)drawA.X, (int)drawA.Y, 100, 30), Color.Black);
            DrawTransparentText(g, labelB.Text, new Font("Arial", 12), new Rectangle((int)drawB.X, (int)drawB.Y, 100, 30), Color.Black);
            DrawTransparentText(g, labelC.Text, new Font("Arial", 12), new Rectangle((int)drawC.X, (int)drawC.Y, 100, 30), Color.Black);
            DrawTransparentText(g, labelD.Text, new Font("Arial", 12), new Rectangle((int)drawD.X, (int)drawD.Y, 100, 30), Color.Black);

            //labelA.BringToFront();
            //labelB.BringToFront();
            //labelC.BringToFront();
            //labelD.BringToFront();
        }

        private void DrawAxis(Graphics g, Color axisColor)
        {
            float arrowSize = 10;
            float lineWidth = _axisWidth;
            var currentHeight = pictureBox1.Height;
            var currentWidth = pictureBox1.Width;

            // Ustawienie grubości linii
            Pen axisPen = new Pen(axisColor, lineWidth);

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
            DrawArrow(g, startX, endX, arrowSize, lineWidth, ColorAxis);
            DrawArrow(g, startY, endY, arrowSize, lineWidth, ColorAxis);
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                var btn = sender as Button;
                var color = btn.BackColor = colorDialog1.Color;

                if (btn.Name == "btnColor1")
                    _colorLine1 = color;
                else if (btn.Name == "btnColor2")
                    _colorLine2 = color;
                else if (btn.Name == "btnColorNet")
                    _colorNet = color;
                else
                    _colorAxis = color;

                // Sprawdzenie jasności koloru tła
                var brightness = (color.R + color.G + color.B) / 3;

                // Ustawienie koloru napisu na etykiecie
                if (brightness < 128) // Kolor tła jest ciemny
                    btn.ForeColor = Color.White;
                else // Kolor tła jest jasny
                    btn.ForeColor = Color.Black;

                Draw();
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            _line1Width = trackBar1.Value;
            Draw();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            _line2Width = trackBar2.Value;
            Draw();
        }

        private void trackBarAxis_ValueChanged(object sender, EventArgs e)
        {
            _axisWidth = trackBarAxis.Value;
            Draw();
        }

        private void trackBarNet_Scroll(object sender, EventArgs e)
        {
            _netWidth = trackBarNet.Value;
            Draw();
        }

        private void cb_Click(object sender, EventArgs e)
        {
            var cb = sender as CheckBox;

            cb.Checked = !cb.Checked;

            if (cb.CheckState == CheckState.Checked)
                cb.Text = CheckBoxState.On;
            else
                cb.Text = CheckBoxState.Off;
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
            CxTextBox.Text = "551";
            DxTextBox.Text = "222";
            AyTextBox.Text = "79";
            ByTextBox.Text = "340";
            CyTextBox.Text = "155";
            DyTextBox.Text = "444";
        }


        private void DrawArrow(Graphics graphics, PointF startPoint, PointF endPoint, float arrowSize, float lineWidth, Color axisColor)
        {
            float angle = (float)Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X);
            PointF[] arrowPoints = new PointF[3];
            arrowPoints[0] = new PointF(endPoint.X - arrowSize * (float)Math.Cos(angle - Math.PI / 6),
                endPoint.Y - arrowSize * (float)Math.Sin(angle - Math.PI / 6));
            arrowPoints[1] = endPoint;
            arrowPoints[2] = new PointF(endPoint.X - arrowSize * (float)Math.Cos(angle + Math.PI / 6),
                endPoint.Y - arrowSize * (float)Math.Sin(angle + Math.PI / 6));

            using (Pen arrowPen = new Pen(axisColor, lineWidth))
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
