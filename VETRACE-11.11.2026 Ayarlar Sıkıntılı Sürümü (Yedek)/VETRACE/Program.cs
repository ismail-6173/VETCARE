using System;
using System.Windows.Forms;
using VETRACE;
using VetCare;

namespace VetCare
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (LoginForm giris = new LoginForm())
            {
                if (giris.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new AnaForm());
                }
            }
        }
    }
}
