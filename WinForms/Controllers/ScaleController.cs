using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms.Controllers
{
    interface IScaleController
    {
        /// <summary>
        /// Wywalamy minusy, bo jak mamy niby je narysowac...
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <returns></returns>
        List<MyPoint> ScaleReduceMinusNumbers(MyPoint A, MyPoint B, MyPoint C, MyPoint D);
        /// <summary>
        /// Zwiekszamy skale zeby ktos nie podal punktu P = (1,2)... nie narysujemy czegos takiego ;p
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        List<MyPoint> ScaleIncreasePoints(MyPoint A, MyPoint B, MyPoint C, MyPoint D, double w, double h);
        /// <summary>
        /// Nie narysujemy cos w stylu P = (999999,22222), więc skalowanie w dół
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        List<MyPoint> ScaleDicreasePoints(MyPoint A, MyPoint B, MyPoint C, MyPoint D, double w, double h);
        /// <summary>
        /// Skalowanie
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        List<MyPoint> ComplexScale(MyPoint A, MyPoint B, MyPoint C, MyPoint D, double w, double h);

        List<MyPoint> ComplexScalePointLabels(MyPoint A, MyPoint B, MyPoint C, MyPoint D, double w, double h);
        double GetMinXValue(MyPoint A, MyPoint B, MyPoint C, MyPoint D);
        double GetMinYValue(MyPoint A, MyPoint B, MyPoint C, MyPoint D);
        double GetMaxXValue(MyPoint A, MyPoint B, MyPoint C, MyPoint D);
        double GetMaxYValue(MyPoint A, MyPoint B, MyPoint C, MyPoint D);
        int SpecialRounding(int input);
    }
    public class ScaleController : IScaleController
    {
        private readonly float _scale;
        private readonly float _padding;
        /// <summary>
        /// scale - ponizej 1 dajemy, inna wartość niema sensu, padding po to żeby z lewej i z góry był jakiś odstęp. Ale nie mam pewnosci czy ten padding nie psuje
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="padding"></param>
        public ScaleController(float scale, float padding)
        {
            _scale = scale;
            _padding = padding;
        }

        /// <summary>
        /// Nie jesteśmy w stanie narysować punktów o ujemnych współrzędnych, więc stosujemy stosowne przesunięcie na plusowe wartości
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<MyPoint> ScaleReduceMinusNumbers(MyPoint A, MyPoint B, MyPoint C, MyPoint D)
        {
            var minX = GetMinXValue(A, B, C, D);
            var minY = GetMinYValue(A, B, C, D);

            var min = Math.Min(minX, minY);

            if (min >= 0)
                return new List<MyPoint>() { A, B, C, D };

            A.X += Math.Abs(min);
            A.Y += Math.Abs(min);
            B.X += Math.Abs(min);
            B.Y += Math.Abs(min);
            C.X += Math.Abs(min);
            C.Y += Math.Abs(min);
            D.X += Math.Abs(min);
            D.Y += Math.Abs(min);

            return new List<MyPoint>() { A, B, C, D };
        }

        public List<MyPoint> ScaleIncreasePoints(MyPoint A, MyPoint B, MyPoint C, MyPoint D, double w, double h)
        {
            if (w <= 0 || h <= 0)
                throw new ArgumentException("w and h cannot be lower or equal 0");

            if (GetMinXValue(A, B, C, D) < 0)
                throw new ArgumentException("x cannot be lower than 0");

            if (GetMinYValue(A, B, C, D) < 0)
                throw new ArgumentException("y cannot be lower than 0");

            /* Sprawdzenie czy scalowanie jest wg konieczne */
            var maxX = GetMaxXValue(A, B, C, D);
            var maxY = GetMaxYValue(A, B, C, D);

            if (maxY >= h || maxX >= w)
                return new List<MyPoint>() { A, B, C, D };

            var scaleRatioX = maxX / w;
            var scaleRatioY = maxY / h;

            var points = new List<MyPoint>() { A, B, C, D };

            for (int i = 0; i < points.Count; i++)
            {
                points[i].X = (points[i].X / scaleRatioX) * _scale;
                points[i].Y = (points[i].Y / scaleRatioY) * _scale;
            }

            return points;
        }

        /// <summary>
        /// Skalowanie punktów tak żeby mieściły się w tej kontrolce w której jest rysowana, w to wight tej kontrolki, h analogicznie
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public List<MyPoint> ScaleDicreasePoints(MyPoint A, MyPoint B, MyPoint C, MyPoint D, double w, double h)
        {
            if (w <= 0 || h <= 0)
                throw new ArgumentException("w and h cannot be lower or equal 0");

            if (GetMinXValue(A, B, C, D) < 0)
                throw new ArgumentException("x cannot be lower than 0");

            if (GetMinYValue(A, B, C, D) < 0)
                throw new ArgumentException("y cannot be lower than 0");

            /* Sprawdzenie czy scalowanie jest wg konieczne */
            var maxX = GetMaxXValue(A, B, C, D);
            var maxY = GetMaxYValue(A, B, C, D);

            /* Skalowanie nie konieczne */
            if ((maxX < w) && (maxY < h))
                return new List<MyPoint>() { A, B, C, D };

            var scaleRatioX = maxX / w;
            var scaleRatioY = maxY / h;

            var points = new List<MyPoint>() { A, B, C, D };

            for (int i = 0; i < points.Count; i++)
            {
                points[i].X = (points[i].X / scaleRatioX) * _scale;
                points[i].Y = (points[i].Y / scaleRatioY) * _scale;
            }

            return points;
        }

        public List<MyPoint> ComplexScale(MyPoint A, MyPoint B, MyPoint C, MyPoint D, double w, double h)
        {
            var aClone = A.Clone() as MyPoint;          
            var bClone = B.Clone() as MyPoint;          
            var cClone = C.Clone() as MyPoint;          
            var dClone = D.Clone() as MyPoint;

            var a = ScaleReduceMinusNumbers(aClone, bClone, cClone, dClone);
            var b = ScaleDicreasePoints(a[0], a[1], a[2], a[3], w, h);
            var c = ScaleIncreasePoints(b[0], b[1], b[2], b[3], w, h);

            c.ForEach(p => p.X += _padding);
            c.ForEach(p => p.Y += _padding);

            var b1 = ScaleDicreasePoints(a[0], a[1], a[2], a[3], w, h);
            var c2 = ScaleIncreasePoints(b1[0], b1[1], b1[2], b1[3], w, h);

            return c2;
        }

        /// <summary>
        /// Uzywana do odpowiedniego rozmieszczenia tych napisow wartości pkt na wykresie, np (423,421)
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public List<MyPoint> ComplexScalePointLabels(MyPoint A, MyPoint B, MyPoint C, MyPoint D, double w, double h)
        {
            var aClone = A.Clone() as MyPoint;
            var bClone = B.Clone() as MyPoint;
            var cClone = C.Clone() as MyPoint;
            var dClone = D.Clone() as MyPoint;

            var points = new List<MyPoint>() {aClone, bClone, cClone, dClone};

            for (var i = 0; i < points.Count; i++)
            {
                /* Zeby nie wychodzilo po prawej stronie poza wykres */
                if (w / points[i].X < 2)
                    points[i].X = points[i].X - 90;
     
                /* Jesli nad osią X, to wyswietlaj poniżej linii, jak pod, to powyżej */
                if (h / points[i].Y < 2)
                {
                    points[i].Y = points[i].Y - 20;
                }
                else
                {
                    points[i].Y = points[i].Y + 20;
                }
            }

            return points;
        }

        public double GetMinXValue(MyPoint A, MyPoint B, MyPoint C, MyPoint D)
        {
            return Math.Min(Math.Min(A.X, B.X), Math.Min(C.X, D.X));
        }
        public double GetMinYValue(MyPoint A, MyPoint B, MyPoint C, MyPoint D)
        {
            return Math.Min(Math.Min(A.Y, B.Y), Math.Min(C.Y, D.Y));
        }
        public double GetMaxXValue(MyPoint A, MyPoint B, MyPoint C, MyPoint D)
        {
            return Math.Max(Math.Max(A.X, B.X), Math.Max(C.X, D.X));
        }
        public double GetMaxYValue(MyPoint A, MyPoint B, MyPoint C, MyPoint D)
        {
            return Math.Max(Math.Max(A.Y, B.Y), Math.Max(C.Y, D.Y));
        }

        public int SpecialRounding(int input)
        {
            int lastDigit = Math.Abs(input) % 10; // Pobranie ostatniej cyfry z wartości bezwzględnej liczby

            if (lastDigit < 5)
            {
                // Zaokrąglenie w dół do najbliższej dziesiątki
                return (input / 10) * 10;
            }
            else
            {
                // Zaokrąglenie w górę do najbliższej dziesiątki
                return ((input / 10) + Math.Sign(input)) * 10;
            }
        }
    }
}
