using System;
using System.Windows.Forms;

namespace MySqlDbStructureClone
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Create_DB());
        }
    }
}
