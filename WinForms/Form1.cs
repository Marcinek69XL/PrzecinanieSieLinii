using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;
using WinForms.Controllers;
using WinForms.Model;
using Label = System.Windows.Forms.Label;

namespace WinForms
{
    public partial class Form1 : Form
    {
        static class CheckBoxState
        {
            internal static readonly string On = "Widoczne";
            internal static readonly string Off = "Ukryte";
        }

        private MyPoint A, B, C, D;
        Label labelA, labelB, labelC, labelD;

        private Config _config;

        private IInsertionController _insertionController;
        private IScaleController _scaleController;
        private IConfigController _configController;

        public Form1()
        {
            InitializeComponent();

            StartUpData();

            _insertionController = new InsertionContoller();
            _scaleController = new ScaleController();
            _configController = new ConfigController("config.json");

            A = null;
            B = null;
            C = null;
            D = null;

            LoadConfig();
            InitCheckboxes();
        }

        private void InitCheckboxes()
        {
            var cbs = new List<CheckBox>() { cbNet, cbLine2, cbLine1, cbAxis };

            foreach (var cb in cbs)
            {
                if (cb.Checked)
                    cb.Text = CheckBoxState.On;
                else
                    cb.Text = CheckBoxState.Off;
            }
        }

        private void LoadConfig()
        {
            if (CustomMessageBox.QuestionYesNo("Czy wczytaj poprzedni zapisany stan aplikacji?") == DialogResult.Yes)
            {
                _config = _configController.LoadConfig();

                if (_config == null)
                    _config = new Config();
                else
                    ApplyConfig(_config);
            }
            else
                _config = new Config();
        }

        private void ApplyConfig(Config config)
        {
            cbAxis.Checked = config.AxisColorVisibility;
            cbLine1.Checked = config.Line1IsVisibility;
            cbLine2.Checked = config.Line2IsVisibility;
            cbNet.Checked = config.NetVisibility;

            btnAxisColor.BackColor = config.AxisColor;
            btnColor1.BackColor = config.Line1Color;
            btnColor2.BackColor = config.Line2Color;
            btnColorNet.BackColor = config.NetColor;

            trackBarAxis.Value = config.AxisWidth;
            trackBar1.Value = config.Line1Width;
            trackBar2.Value = config.Line2Width;
            trackBarNet.Value = config.NetWidth;
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

            var currentHeight = pictureBox1.Height;
            var currentWidth = pictureBox1.Width;

            Graphics g = pictureBox1.CreateGraphics();

            DrawNet(g);

            DrawAxis(g, _config.AxisColor);

            var pointsToDraw = _scaleController.ComplexScale(A, B, C, D, currentWidth, currentHeight);

            var drawA = pointsToDraw[0];
            var drawB = pointsToDraw[1];
            var drawC = pointsToDraw[2];
            var drawD = pointsToDraw[3];

            DrawLines(g, drawA, drawB, drawC, drawD);
            DrawLabels(g, drawA, drawB, drawC, drawD);
        }

        private void DrawLabels(Graphics g, MyPoint drawA, MyPoint drawB, MyPoint drawC, MyPoint drawD)
        {
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

            var drawLabelsLocalizactions = _scaleController.ComplexScalePointLabels(drawA, drawB, drawC, drawD, pictureBox1.Width, pictureBox1.Height);

            var drawLabelALoc = drawLabelsLocalizactions[0];
            var drawLabelBLoc = drawLabelsLocalizactions[1];
            var drawLabelCLoc = drawLabelsLocalizactions[2];
            var drawLabelDLoc = drawLabelsLocalizactions[3];

            DrawTransparentText(g, labelA.Text, new Font("Arial", 12), new Rectangle((int)drawLabelALoc.X, (int)drawLabelALoc.Y, 100, 30), Color.Black);
            DrawTransparentText(g, labelB.Text, new Font("Arial", 12), new Rectangle((int)drawLabelBLoc.X, (int)drawLabelBLoc.Y, 100, 30), Color.Black);
            DrawTransparentText(g, labelC.Text, new Font("Arial", 12), new Rectangle((int)drawLabelCLoc.X, (int)drawLabelCLoc.Y, 100, 30), Color.Black);
            DrawTransparentText(g, labelD.Text, new Font("Arial", 12), new Rectangle((int)drawLabelDLoc.X, (int)drawLabelDLoc.Y, 100, 30), Color.Black);
        }

        private void DrawLines(Graphics g, MyPoint drawA, MyPoint drawB, MyPoint drawC, MyPoint drawD)
        {
            // Narysowanie prostej między punktami A i B
            Pen linePen1 = new Pen(_config.Line1Color, _config.Line1Width);
            Pen linePen2 = new Pen(_config.Line2Color, _config.Line2Width);

            g.DrawLine(linePen1, (float)drawA.X, (float)drawA.Y, (float)drawB.X, (float)drawB.Y);
            g.DrawLine(linePen2, (float)drawC.X, (float)drawC.Y, (float)drawD.X, (float)drawD.Y);
        }

        private void DrawNet(Graphics g)
        {
            int rozpietoscX = 20;
            int rozpietoscY = 20;

            g.Clear(Color.White);

            var pen = new Pen(_config.NetColor);
            for (int y = 0; y < pictureBox1.Height; y += rozpietoscY)
            {
                g.DrawLine(pen, 0, y, pictureBox1.Width, y);
            }

            for (int x = 0; x < pictureBox1.Width; x += rozpietoscX)
            {
                g.DrawLine(pen, x, 0, x, pictureBox1.Height);
            }
        }

        private void DrawAxis(Graphics g, Color axisColor)
        {
            float arrowSize = 10;
            float lineWidth = _config.AxisWidth;
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
            DrawArrow(g, startX, endX, arrowSize, lineWidth, _config.AxisColor);
            DrawArrow(g, startY, endY, arrowSize, lineWidth, _config.AxisColor);
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                var btn = sender as Button;
                var color = btn.BackColor = colorDialog1.Color;

                if (btn.Name == "btnColor1")
                    _config.Line1Color = color;
                else if (btn.Name == "btnColor2")
                    _config.Line2Color = color;
                else if (btn.Name == "btnColorNet")
                    _config.NetColor = color;
                else
                    _config.AxisColor = color;

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
            _config.Line1Width = trackBar1.Value;
            Draw();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            _config.Line2Width = trackBar2.Value;
            Draw();
        }

        private void trackBarAxis_ValueChanged(object sender, EventArgs e)
        {
            _config.AxisWidth = trackBarAxis.Value;
            Draw();
        }

        private void trackBarNet_Scroll(object sender, EventArgs e)
        {
            _config.NetWidth = trackBarNet.Value;
            Draw();
        }

        private void cb_Click(object sender, EventArgs e)
        {
            var cb = sender as CheckBox;

            if (cb.CheckState == CheckState.Checked)
                cb.Text = CheckBoxState.On;
            else
                cb.Text = CheckBoxState.Off;

            if (cb.Name == "cbLine1")
                _config.Line1IsVisibility = cb.Checked;
            else if (cb.Name == "cbLine2")
                _config.Line2IsVisibility = cb.Checked;
            else if (cb.Name == "cbAxis")
                _config.AxisColorVisibility = cb.Checked;
            else if (cb.Name == "cbNet")
                _config.NetVisibility = cb.Checked;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _configController.SaveConfig(_config);
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            panelLineSaveSettings.Enabled = rbYes.Checked;
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
