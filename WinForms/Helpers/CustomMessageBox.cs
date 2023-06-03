using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    public class CustomMessageBox
    {
        public static DialogResult Info(string infoMessage)
        {
            return 
                MessageBox.Show(infoMessage,
                    "Informacja",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
        }

        public static DialogResult Error(string errMessage)
        {
            return
                MessageBox.Show(errMessage,
                    "Błąd",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }
    }
}
