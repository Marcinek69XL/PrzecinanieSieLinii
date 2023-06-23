using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
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

        static class IsLinesCrossedMessage
        {
            internal static readonly string Yes = "Tak";
            internal static readonly string No = "Nie";
        }

        static class CrossOrOverlappingMessage
        {
            internal static readonly string Cross = "Proste się przecinają";
            internal static readonly string NotCross = "Proste współliniowe";
            internal static readonly string Overlapping = "Proste współliniowe";
        }

        Label labelA, labelB, labelC, labelD;

        private bool _loadConfigAndValuesUserAnswer;

        private Config _config;
        private Values _values;

        private IInsertionController _insertionController;
        private IScaleController _scaleController;
        private IConfigController<Config> _configControllerSettings;
        private IConfigController<Values> _configControllerValues;

        public Form1()
        {
            InitializeComponent();

            StartUpData();

            _insertionController = new InsertionContoller();
            _scaleController = new ScaleController(0.98f, 30);
            _configControllerSettings = new ConfigController<Config>("config.json");
            _configControllerValues = new ConfigController<Values>("values.json");

            _loadConfigAndValuesUserAnswer = false;

            LoadConfig();
            LoadValues();

            //InitCheckboxes();
            SetButtonsFontBlackOrWhite();
        }

        private void SetButtonsFontBlackOrWhite()
        {
            var btns = new List<Button>() {btnColor1, btnColor2, btnAxisColor, btnColorNet};

            foreach (var btn in btns)
            {
                SetButtonFontBlackOrWhite(btn);
            }
        }

        private void LoadValues()
        {
            try
            {
                _values = _configControllerValues.LoadConfig();
            }
            catch
            {
                CustomMessageBox.Error("Napotkano problem z wczytaniem zapisanych ostatnich wartości");
            }


            if (_values == null)
                _values = new Values();
            else
            {
                _loadConfigAndValuesUserAnswer = true;
                ApplyValues();
            }
        }

        private void ApplyValues()
        {
            AxTextBox.Text = Convert.ToString(_values.A.X, CultureInfo.CurrentCulture);
            AyTextBox.Text = Convert.ToString(_values.A.Y, CultureInfo.CurrentCulture);
            BxTextBox.Text = Convert.ToString(_values.B.X, CultureInfo.CurrentCulture);
            ByTextBox.Text = Convert.ToString(_values.B.Y, CultureInfo.CurrentCulture);
            CxTextBox.Text = Convert.ToString(_values.C.X, CultureInfo.CurrentCulture);
            CyTextBox.Text = Convert.ToString(_values.C.Y, CultureInfo.CurrentCulture);
            DxTextBox.Text = Convert.ToString(_values.D.X, CultureInfo.CurrentCulture);
            DyTextBox.Text = Convert.ToString(_values.D.Y, CultureInfo.CurrentCulture);
        }

        private void SetButtonFontBlackOrWhite(Button btn)
        {
            var backgroundBtnColor = btn.BackColor;

            // Sprawdzenie jasności koloru tła
            var brightness = (backgroundBtnColor.R + backgroundBtnColor.G + backgroundBtnColor.B) / 3;

            if (brightness < 128) // Kolor tła jest ciemny
                btn.ForeColor = Color.White;
            else // Kolor tła jest jasny
                btn.ForeColor = Color.Black;
        }

        private void InitCheckboxes()
        {
            var cbs = new List<CheckBox>() { cbNet, cbLine2, cbLine1, cbAxis, cbGridlines };

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
                try
                {
                    _config = _configControllerSettings.LoadConfig();
                }
                catch
                {
                    CustomMessageBox.Error("Napotkano problem z wczytaniem zapisanego configu");
                }
                

                if (_config == null)
                    _config = new Config();
                else
                {
                    _loadConfigAndValuesUserAnswer = true;
                    ApplyConfig(_config);
                }
                   
            }
            else
                _config = new Config();
        }

        private void ApplyConfig(Config config)
        {
            cbAxis.Checked = config.AxisIsVisibility;
            cbLine1.Checked = config.Line1IsVisibility;
            cbLine2.Checked = config.Line2IsVisibility;
            cbNet.Checked = config.NetVisibility;
            cbGridlines.Checked = config.GridlinesVisibility;

            btnAxisColor.BackColor = config.AxisColor;
            btnColor1.BackColor = config.Line1Color;
            btnColor2.BackColor = config.Line2Color;
            btnColorNet.BackColor = config.NetColor;

            trackBarAxis.Value = config.AxisWidth;
            trackBar1.Value = config.Line1Width;
            trackBar2.Value = config.Line2Width;
            trackBarNet.Value = config.NetWidth;
            trackBarPoint.Value = config.PointSize;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
                return;

            InitPoints();

            _values.A.X = double.Parse(AxTextBox.Text);
            _values.A.Y = double.Parse(AyTextBox.Text);
            _values.B.X = double.Parse(BxTextBox.Text);
            _values.B.Y = double.Parse(ByTextBox.Text);
            _values.C.X = double.Parse(CxTextBox.Text);
            _values.C.Y = double.Parse(CyTextBox.Text);
            _values.D.X = double.Parse(DxTextBox.Text);
            _values.D.Y = double.Parse(DyTextBox.Text);

            Draw();

            bool przecinajaSie = _insertionController.CzySiePrzecinaja(_values.A, _values.B, _values.C, _values.D);
            var crossPoints = _insertionController.WyznaczPunktPrzeciecia(_values.A, _values.B, _values.C, _values.D);


            MyPoint firstCrossingPoint = null;
            MyPoint secondCrossingPoint = null;
            var isCrossing = false;
            var isCollinear = false;

            if (crossPoints.Count == 1 && przecinajaSie)
            {
                firstCrossingPoint = crossPoints.First();
                isCrossing = true;
                CustomMessageBox.Info(
                    "Odcinki się przecinają. Przecięcie następuje w punkcie. Współrzędne punktu przecinającego: (" + Round(crossPoints[0].X) + " ; " + Round(crossPoints[0].Y) + ")");
            }
            
            else if (crossPoints.Count == 2)
            {
                isCollinear = true;
                secondCrossingPoint = crossPoints[1];
                CustomMessageBox.Info("Współniniowe");
            }
            else
            {
                isCrossing = false;
                isCollinear = false;
                CustomMessageBox.Info("Linie nie przecinają się");
            }
            
            CreateASummary(firstCrossingPoint, secondCrossingPoint, isCrossing, isCollinear);
        }

        private void InitPoints()
        {
            _values.A = new MyPoint();
            _values.B = new MyPoint();
            _values.C = new MyPoint();
            _values.D = new MyPoint();
        }

        private void Draw()
        {
            if(_values == null)
                return;
            if (_values.A == null || _values.B == null || _values.C == null || _values.D == null)
                return;;

            var currentHeight = pictureBox1.Height;
            var currentWidth = pictureBox1.Width;

            var g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);

            if (_config.NetVisibility)
                DrawNet(g);
            
            if (_config.AxisIsVisibility)
                DrawAxis(g, _config.AxisColor);

            var pointsToDraw = _scaleController.ComplexScale(_values.A, _values.B, _values.C, _values.D, currentWidth, currentHeight);

            var drawA = pointsToDraw[0];
            var drawB = pointsToDraw[1];
            var drawC = pointsToDraw[2];
            var drawD = pointsToDraw[3];

            if (_config.Line1IsVisibility)
                DrawLine(g, drawA, drawB, _config.Line1Color, _config.Line1Width);

            if (_config.Line2IsVisibility)
                DrawLine(g, drawC, drawD, _config.Line2Color, _config.Line2Width);

            if (_config.PointsVisibily)
            {
                DrawPoint(g, drawA, _config.PointSize, _config.Line1Color);
                DrawPoint(g, drawB, _config.PointSize, _config.Line1Color);
                DrawPoint(g, drawC, _config.PointSize, _config.Line2Color);
                DrawPoint(g, drawD, _config.PointSize, _config.Line2Color);
            }
            
            DrawLabels(g, drawA, drawB, drawC, drawD);

            if (_config.GridlinesVisibility)
                Gridlines(g, drawA, drawB, drawC, drawD);
        }

        private void Gridlines(Graphics graphics, MyPoint drawA, MyPoint drawB, MyPoint drawC, MyPoint drawD)
        {
            var pen = new Pen(Color.Black);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            // Pionowe
            graphics.DrawLine(pen, (float)drawA.X, (float)drawA.Y, (float)drawA.X, pictureBox1.Height / 2);
            graphics.DrawLine(pen, (float)drawB.X, (float)drawB.Y, (float)drawB.X, pictureBox1.Height / 2);
            graphics.DrawLine(pen, (float)drawC.X, (float)drawC.Y, (float)drawC.X, pictureBox1.Height / 2);
            graphics.DrawLine(pen, (float)drawD.X, (float)drawD.Y, (float)drawD.X, pictureBox1.Height / 2);

            // Poziome
            graphics.DrawLine(pen, (float)drawA.X, (float)drawA.Y, pictureBox1.Width / 2, (float)drawA.Y);
            graphics.DrawLine(pen, (float)drawB.X, (float)drawB.Y, pictureBox1.Width / 2, (float)drawB.Y);
            graphics.DrawLine(pen, (float)drawC.X, (float)drawC.Y, pictureBox1.Width / 2, (float)drawC.Y);
            graphics.DrawLine(pen, (float)drawD.X, (float)drawD.Y, pictureBox1.Width / 2, (float)drawD.Y);

        }

        private void DrawPoint(Graphics g, MyPoint point, int pointSize, Color color)
        {
            // Narysowanie punktu
            var pointX = point.X - pointSize / 2;
            var pointY = point.Y - pointSize / 2;
            Brush pointBrush = new SolidBrush(color);
            g.FillEllipse(pointBrush, new Rectangle((int)pointX, (int)pointY, pointSize, pointSize));
        }

        private void DrawLabels(Graphics g, MyPoint drawA, MyPoint drawB, MyPoint drawC, MyPoint drawD)
        {
            labelA?.Dispose();
            labelB?.Dispose();
            labelC?.Dispose();
            labelD?.Dispose();

            labelA = new Label();
            labelA.Text = $"({_values.A.X},{_values.A.Y})";

            labelB = new Label();
            labelB.Text = $"({_values.B.X},{_values.B.Y})";

            labelC = new Label();
            labelC.Text = $"({_values.C.X},{_values.C.Y})";

            labelD = new Label();
            labelD.Text = $"({_values.D.X},{_values.D.Y})";

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

        private void DrawLine(Graphics g, MyPoint from, MyPoint to, Color lineColor, float lineWidth)
        {
            // Narysowanie prostej między punktami A i B
            var pen = new Pen(lineColor, lineWidth);

            g.DrawLine(pen, (float)from.X, (float)from.Y, (float)to.X, (float)to.Y);
        }

        private void DrawNet(Graphics g)
        {
            int rozpietoscX = 20;
            int rozpietoscY = 20;

            var pen = new Pen(_config.NetColor, _config.NetWidth);
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

            DrawTransparentText(g, "y", new Font("Arial", 16), new Rectangle(pictureBox1.Width / 2,0, 30,30), Color.Black);
            DrawTransparentText(g, "x", new Font("Arial", 16), new Rectangle(pictureBox1.Width - 30, pictureBox1.Height / 2, 30, 30), Color.Black);
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

                // Ustawienie koloru napisu na etykiecie
                SetButtonFontBlackOrWhite(btn);
              

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
                _config.AxisIsVisibility = cb.Checked;
            else if (cb.Name == "cbNet")
                _config.NetVisibility = cb.Checked;
            else if (cb.Name == "cbGridlines")
                _config.GridlinesVisibility = cb.Checked;

            Draw();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(_config.SaveConfig)
                _configControllerSettings.SaveConfig(_config);
            if(_config.SaveValues)
                _configControllerValues.SaveConfig(_values);
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            panelLineSaveSettings.Enabled = rbYes.Checked;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            Draw();
        }

        private void trackBarPoint_ValueChanged(object sender, EventArgs e)
        {
            _config.PointSize = trackBarPoint.Value;
            Draw();
        }

        private void rbOdcYes_CheckedChanged(object sender, EventArgs e)
        {
            _config.SaveValues = rbOdcYes.Checked;
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

        private void label20_Click(object sender, EventArgs e)
        {

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

        private void CreateASummary(MyPoint p1, MyPoint p2, bool isCroosing, bool isCollinear)
        {
            gbSummary.Visible = true;
            panelPunktPrzeciecia.Visible = isCroosing;
            lbIsCrossing.Text = isCroosing ? IsLinesCrossedMessage.Yes : IsLinesCrossedMessage.No;

            if (isCollinear)
                lbPunktPrzeciecia.Text = CrossOrOverlappingMessage.Overlapping;
            else if (isCroosing)
                lbPunktPrzeciecia.Text = CrossOrOverlappingMessage.Cross;
            else
                lbPunktPrzeciecia.Text = CrossOrOverlappingMessage.NotCross;

            lbP1.Text = CreateAResultMessage(p1, 1);
            lbP2.Text = CreateAResultMessage(p2, 2);

        }

        /// <summary>
        /// index 1 to P1, index 2 to P2... 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string CreateAResultMessage(MyPoint point, int index)
        {
            if (point == null)
                return string.Empty;

            var roundValue1 = Round(point.X);
            var roundValue2 = Round(point.Y);
            return $"P{index} = ( {roundValue1} ; {roundValue2} )";
        }

        private double Round(double input)
        {
            return Math.Round(input, 2, MidpointRounding.AwayFromZero);
        }
    }
}
