using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms
{
    public interface IInsertionController
    {
        bool CzySiePrzecinaja(MyPoint A, MyPoint B, MyPoint C, MyPoint D);
    }

    public class InsertionContoller : IInsertionController
    {
        private decimal IloczynWektorowy(decimal Xa, decimal Xb, decimal Ya, decimal Yb, decimal Za, decimal Zb)
        {
            var x1 = Za - Xa;
            var y1 = Zb - Xb;

            var x2 = Ya - Xa;
            var y2 = Yb - Xb;

            return (x1 * y2) - (x2 * y1);
        }

        private bool Sprawdz(decimal Xa, decimal Xb, decimal Ya, decimal Yb, decimal Za, decimal Zb)
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

    }
}
