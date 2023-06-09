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
    }
    public class ScaleController : IScaleController
    {
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
                points[i].X = (points[i].X / scaleRatioX) * 0.99;
                points[i].Y = (points[i].Y / scaleRatioY) * 0.99;
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
                points[i].X = (points[i].X / scaleRatioX) * 0.99;
                points[i].Y = (points[i].Y / scaleRatioY) * 0.99;
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

            return c;
        }

        private double GetMinXValue(MyPoint A, MyPoint B, MyPoint C, MyPoint D)
        {
            return Math.Min(Math.Min(A.X, B.X), Math.Min(C.X, D.X));
        }
        private double GetMinYValue(MyPoint A, MyPoint B, MyPoint C, MyPoint D)
        {
            return Math.Min(Math.Min(A.Y, B.Y), Math.Min(C.Y, D.Y));
        }
        private double GetMaxXValue(MyPoint A, MyPoint B, MyPoint C, MyPoint D)
        {
            return Math.Max(Math.Max(A.X, B.X), Math.Max(C.X, D.X));
        }
        private double GetMaxYValue(MyPoint A, MyPoint B, MyPoint C, MyPoint D)
        {
            return Math.Max(Math.Max(A.Y, B.Y), Math.Max(C.Y, D.Y));
        }
    }
}
