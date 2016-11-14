using System;
using System.Windows.Forms;

namespace DataCollectionForRealtime
{
    static class Program
    {
        public static DCMainForm MainForm;
        public static DCMiniMonitor MiniMonitor;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new DCMainForm();
            MiniMonitor = new DCMiniMonitor();
            Application.Run(MainForm);
        }
    }
}
