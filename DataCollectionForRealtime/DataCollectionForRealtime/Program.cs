using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataCollectionForRealtime
{
    static class Program
    {
        public static RealtimeDataManagement mainMonitor;
        public static DCMiniMonitor miniMonitor;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainMonitor = new RealtimeDataManagement();
            miniMonitor = new DCMiniMonitor();
            Application.Run(mainMonitor);
        }
    }
}
