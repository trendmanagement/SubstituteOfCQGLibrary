using RealtimeSpreadMonitor.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealtimeSpreadMonitor.FormManipulation
{
    internal class StatusAndConnectedUpdates
    {
        private OptionRealtimeMonitor form;
        private OptionSpreadManager optionSpreadManager;

        internal StatusAndConnectedUpdates(OptionRealtimeMonitor form, 
            OptionSpreadManager optionSpreadManager)
        {
            this.form = form;
            this.optionSpreadManager = optionSpreadManager;
        }

        internal void checkUpdateStatus(DataGridView gridView,
            int row, int col, OptionSpreadExpression optionSpreadExpression)            
        {
            Color backColor;

            //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
            if(optionSpreadExpression.instrument.eodAnalysisAtInstrument)
            {
                backColor = Color.Plum;

            }
            else if (optionSpreadExpression.minutesSinceLastUpdate > TradingSystemConstants.MINUTES_STALE_LIVE_UPDATE)
            {
                backColor = Color.Yellow;
            }
            else
            {
                backColor = Color.LightSkyBlue;

            }

            markLiveAsConnected(gridView, row,
                                    col,
                                    true, backColor);

        }


        delegate void ThreadSafeMarkAsConnectedDelegate(DataGridView gridView, int row, int col, bool connected, Color backgroundColor);

        internal void markLiveAsConnected(DataGridView gridView, int row, int col, bool connected, Color backgroundColor)
        {
            try
            {
                if (form.InvokeRequired)
                {
                    ThreadSafeMarkAsConnectedDelegate d = new ThreadSafeMarkAsConnectedDelegate(threadSafeMarkAsConnected);

                    form.Invoke(d, gridView, row, col, connected, backgroundColor);
                }
                else
                {
                    threadSafeMarkAsConnected(gridView, row, col, connected, backgroundColor);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        internal void threadSafeMarkAsConnected(DataGridView gridView, int row, int col, bool connected, Color backgroundColor)
        {
            try
            {
                if (gridView.Rows[row].Cells[col].Style.BackColor != backgroundColor)
                {
                    gridView.Rows[row].Cells[col].Style.BackColor = backgroundColor;
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }
    }
}
