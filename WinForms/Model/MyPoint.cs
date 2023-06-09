using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms
{
    public class MyPoint : ICloneable
    {
        public double X { get; set; }
        public double Y { get; set; }


        public MyPoint(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public MyPoint()
        {
            
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
