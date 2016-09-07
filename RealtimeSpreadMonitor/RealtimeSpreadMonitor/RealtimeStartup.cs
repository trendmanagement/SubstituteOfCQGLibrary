using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RealtimeSpreadMonitor.Forms;

namespace RealtimeSpreadMonitor
{
    static class RealtimeStartup
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            InitializationParms initializationParms = new InitializationParms();
            //initializationParms.tmlSystemRunType = startupType;

            //array<bool> createPersistence = new array<bool>(3);
            InitializationForm initializationForm = new InitializationForm(initializationParms);

            //System.Windows.Forms.Application.Run(initializationForm);
            initializationForm.ShowDialog();

            if (initializationParms.runLiveSystem)
            {
                Application.Run(new OptionRealtimeStartup(initializationParms));
            }
        }
    }
}
