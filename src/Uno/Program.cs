using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Uno
{
    static class Program
    {
		public static Form MainForm { get; private set; }

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(MainForm = new Games.ServerListe());
        }
    }
}
