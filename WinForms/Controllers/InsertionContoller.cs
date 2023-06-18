using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms
{
    public interface IInsertionController
    {
        bool CzySiePrzecinaja(MyPoint A, MyPoint B, MyPoint C, MyPoint D);
        List<MyPoint> WyznaczPunktPrzeciecia(MyPoint A, MyPoint B, MyPoint C, MyPoint D);
    }

    public class InsertionContoller : IInsertionController
    {
        private double IloczynWektorowy(double Xa, double Xb, double Ya, double Yb, double Za, double Zb)
        {
            var x1 = Za - Xa;
            var y1 = Zb - Xb;

            var x2 = Ya - Xa;
            var y2 = Yb - Xb;

            return (x1 * y2) - (x2 * y1);
        }

        private bool Sprawdz(double Xa, double Xb, double Ya, double Yb, double Za, double Zb)
        {
            return Math.Min(Xa, Ya) <= Za && Zb <= Math.Max(Xa, Ya) && Math.Min(Xb, Yb) <= Zb && Zb <= Math.Max(Xb, Yb);
        }

        public bool CzySiePrzecinaja(MyPoint A, MyPoint B, MyPoint C, MyPoint D)
        {
            var v1 = IloczynWektorowy(A.X, A.Y, B.X, B.Y, C.X, C.Y);
            var v2 = IloczynWektorowy(A.X, A.Y, B.X, B.Y, D.X, D.Y);
            var v3 = IloczynWektorowy(C.X, C.Y, D.X, D.Y, A.X, A.Y);
            var v4 = IloczynWektorowy(C.X, C.Y, D.X, D.Y, B.X, B.Y);

            //sprawdzenie czy się przecinają
            if ((v3 > 0 && v4 < 0 || v3 < 0 && v4 > 0) && (v1 > 0 && v2 < 0 || v1 < 0 && v2 > 0)) return true;

            //sprawdzenie czy koniec odcinka leży na drugim
            if (v1 == 0 && Sprawdz(A.X, A.Y, B.X, B.Y, C.X, C.Y)) return true;
            if (v2 == 0 && Sprawdz(A.X, A.Y, B.X, B.Y, D.X, D.Y)) return true;
            if (v3 == 0 && Sprawdz(C.X, C.Y, D.X, D.Y, A.X, A.Y)) return true;
            if (v4 == 0 && Sprawdz(C.X, C.Y, D.X, D.Y, B.X, B.Y)) return true;

            return false;
        }

        public List<MyPoint> WyznaczPunktPrzeciecia(MyPoint A, MyPoint B, MyPoint C, MyPoint D)
        {
            bool odcinkiWspolliniowe = CzySaWspolliniowe(A,B,C,D);
            if (odcinkiWspolliniowe)
            {
                double[] punkty = WyznaczWspolnyOdcinek(A,B,C,D);

                var p1 = new MyPoint(punkty[0], punkty[1]);
                var p2 = new MyPoint(punkty[2], punkty[3]);

                return new List<MyPoint>() {p1, p2};
            }

            //Współczynnik kierunkowy prostej pierwszego odcinka
            double a1 = (B.Y - A.Y) / (B.X - A.X);
            //Wyraz wolny b pierwszego odcinka (y=ax+b => b=y-ax)
            double b1 = A.Y - (a1 * A.X);

            //Współczynnik kierunkowy prostej drugiego odcinka
            double a2 = (D.Y - C.Y) / (D.X - C.X);
            //Wyraz wolny b deugiego odcinka
            double b2 = C.Y - (a2 * C.X);

            //Wyznaczanie wspólnego punktu x oraz y
            double Px = (b2 - b1) / (a1 - a2);
            double Py = ((a1 * b2) - (a2 * b1)) / (a1 - a2);

            return new List<MyPoint> {new MyPoint(Px, Py)};
            //return 
        }

        private bool CzySaWspolliniowe(MyPoint A, MyPoint B, MyPoint C, MyPoint D)
        {
            var tolerance = 0.000000000000001; // 1e - 15

            if (Math.Abs(A.Y - B.Y) < tolerance && Math.Abs(A.Y - C.Y) < tolerance && Math.Abs(A.Y - D.Y) < tolerance) 
                return true;
            return false;
        }

        private double[] WyznaczWspolnyOdcinek(MyPoint A, MyPoint B, MyPoint C, MyPoint D)
        {
            var y = A.Y;
            var punktPoczatkowyX = Math.Max(A.X, C.X);
            var punktKoncowyX = Math.Min(B.X, D.X);

            return new double[] { punktPoczatkowyX, punktKoncowyX, y };
        }
    }
}
