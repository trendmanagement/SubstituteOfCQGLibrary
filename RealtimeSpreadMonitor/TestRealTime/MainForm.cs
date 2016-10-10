using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using FakeCQG.Internal;
using FakeCQG.Internal.Models;
using FakeCQG.Internal.Handshaking;

namespace TestRealTime
{
    public partial class MainForm : Form
    {
        private FakeCQG.CQGCELClass cqgcel;

        public MainForm()
        {
            InitializeComponent();
            Subscriber.ListenForHanshaking();
            Core.LogChange += CQG_LogChange;
            AsyncTaskListener.Updated += AsyncTaskListener_Updated;
        }

        private void AsyncTaskListener_Updated(string msg)
        {
            Action action = new Action(() =>
                {
                    if (!string.IsNullOrWhiteSpace(msg))
                    {
                        richTextBoxLog.Text += msg + "\n";
                        richTextBoxLog.Select(richTextBoxLog.Text.Length, richTextBoxLog.Text.Length);
                        richTextBoxLog.ScrollToCaret();
                    }
                });
            try
            {
                Invoke(action);
            }
            catch (ObjectDisposedException)
            {
                // User closed the form
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //cqg = new FakeCQG.CQG();
            Task.Run(() => { cqgcel = new FakeCQG.CQGCELClass(); });
        }

        private void CQG_LogChange(string message)
        {
            Task.Run(() => { AsyncTaskListener.LogMessage(message); });
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            bool test = Core.AnswerHelper.CheckAnswerAsync("key").GetAwaiter().GetResult();
            AsyncTaskListener.LogMessage(test.ToString());
            Core.QueryHelper.ReadQueries();
        }

        private void checkBoxAuto_CheckedChanged(object sender, EventArgs e)
        {
            buttonRead.Enabled = !checkBoxAuto.Checked;
        }

        private void buttonClean_Click(object sender, EventArgs e)
        {
            Core.QueryHelper.ClearQueriesListAsync();
        }

        private void buttonCall_Click(object sender, EventArgs e)
        {
            bool isStarted = cqgcel.IsStarted;
            cqgcel.Shutdown();
        }
    }
}
