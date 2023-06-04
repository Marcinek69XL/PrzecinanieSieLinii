using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms
{
    public class MyPoint
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }


        public MyPoint(decimal X, decimal Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public MyPoint()
        {
            
        }
    }
}
