using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RealtimeSpreadMonitor.Forms
{
    public partial class OptionStartupProgress : Form
    {
        //private const int PROGRESS_BAR_REFRESH = 1000;
        //private int progress = 0;

        //private BackgroundWorker backgroundWorkerProgressBar;

        delegate void ThreadSafeProgressBarUpdate(double progress);

        public OptionStartupProgress()
        {
            InitializeComponent();

            //initializeBackgroundWorkerProgressBar();
        }

//         private void initializeBackgroundWorkerProgressBar()
//         {
//             try
//             {
//                 backgroundWorkerProgressBar = new BackgroundWorker();
//                 backgroundWorkerProgressBar.WorkerReportsProgress = true;
//                 backgroundWorkerProgressBar.WorkerSupportsCancellation = true;
// 
//                 backgroundWorkerProgressBar.DoWork +=
//                     new DoWorkEventHandler(setupBackgroundWorkerProgressBar);
// 
//                 backgroundWorkerProgressBar.ProgressChanged +=
//                     new ProgressChangedEventHandler(
//                         backgroundWorkerProgressBar_ProgressChanged);
// 
//                 backgroundWorkerProgressBar.RunWorkerAsync();
//             }
//             catch (Exception ex)
//             {
//                 TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
//             }
//         }
// 
//         private void setupBackgroundWorkerProgressBar(object sender,
//             DoWorkEventArgs e)
//         {
//             try
//             {
//                 while (true)
//                 {
//                     backgroundWorkerProgressBar.ReportProgress(0);
//                     System.Threading.Thread.Sleep(PROGRESS_BAR_REFRESH);
//                 }
//             }
//             catch (Exception ex)
//             {
//                 TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
//             }
//         }
// 
//         private void backgroundWorkerProgressBar_ProgressChanged(object sender,
//             ProgressChangedEventArgs e)
//         {
//             try
//             {
//                 updateProgressBar(progress);
        //             }
//             catch (Exception ex)
//             {
//                 TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
//             }
//         }
// 
        public void updateProgressBar(double progress)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    ThreadSafeProgressBarUpdate d = new ThreadSafeProgressBarUpdate(updateProgressBarValue);

                    this.Invoke(d, progress);
                }
                else
                {
                    updateProgressBarValue(progress);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }



//         public void passProgressValue(double progress)
//         {
//             //this.progress = (int)(progress * 100);
// 
//             //updateProgressBar((int)(progress * 100));
// 
//             pgbOptionStartup.Value = (int)(progress * 100);
//         }

        private void updateProgressBarValue(double progress)
        {
            //pgbOptionStartup.Value = progressLocal;

            pgbOptionStartup.Value = (int)(progress * 100);
        }

        private void OptionStartupProgress_Load(object sender, EventArgs e)
        {

        }
    }
}
