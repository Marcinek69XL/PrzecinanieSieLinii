using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    public static class Extensions
    {
        public static bool TryToDecimal(this TextBox tb, out decimal _result)
        {
            if (string.IsNullOrEmpty(tb.Text))
            {
                tb.BackColor = Color.Red;
                _result = Decimal.Zero;;
                return false;
            }

            if (decimal.TryParse(tb.Text, out decimal result))
            {
                tb.BackColor = Color.White;
                _result = result;
                return true;
            }
            else
            {
                _result = Decimal.Zero;
                return false;
            }
        }

        public static DialogResult Info(this MessageBox mb, string question)
        {
            return MessageBox.Show(question, "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
