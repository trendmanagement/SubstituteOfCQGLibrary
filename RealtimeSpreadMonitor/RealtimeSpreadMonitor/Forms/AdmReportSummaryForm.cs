using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace RealtimeSpreadMonitor.Forms
{
    public partial class AdmReportSummaryForm : Form
    {
        delegate void ThreadSafeHideUnhideContractSummaryLiveDataDelegate(int row, bool visible);

        private OptionStrategy[] optionStrategies;
        private Instrument[] instruments;

        //private OptionSpreadManager optionSpreadManager;

        private List<ADMPositionImportWeb> admSummaryFieldsList;

        //private ADM_IMPORT_FILE_TYPES fileImportType;

        private int instrumentSelectedIdx;

        public AdmReportSummaryForm(
            OptionStrategy[] optionStrategies,
            Instrument[] instrumentArray,
            OptionSpreadManager optionSpreadManager)
        {
            InitializeComponent();
            
            this.optionStrategies = optionStrategies;
            this.instruments = instrumentArray;
            
        }



        public void fillFileDateTimeLabel(DateTime fileDateTime)
        {
            StringBuilder lastFileWriteTime = new StringBuilder();

            lastFileWriteTime.Append("ADM FILE LAST LOADED ");

            lastFileWriteTime.Append(
                fileDateTime.ToString("MMM dd yyyy HH:mm:ss", DateTimeFormatInfo.InvariantInfo));

            lblLatestADMFileDate.Text = lastFileWriteTime.ToString();
                
        }

        public void setupAdmInputSummaryGrid(List<ADMPositionImportWeb> admSummaryFieldsList,
            int instrumentSelectedIdx)
        {
#if DEBUG
            try
#endif
            {
                this.instrumentSelectedIdx = instrumentSelectedIdx;

                //fileImportType = ADM_IMPORT_FILE_TYPES.ADM_WEB_POSITIONS;

                this.admSummaryFieldsList = admSummaryFieldsList;

                Type admSummaryFieldDisplayedTypes = typeof(ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED);
                Array admSummaryFieldDisplayedTypeArray = Enum.GetNames(admSummaryFieldDisplayedTypes);

                admInputSummaryGrid.ColumnCount = admSummaryFieldDisplayedTypeArray.Length;  // admSummaryFieldTypeArray.Length;

                admInputSummaryGrid.EnableHeadersVisualStyles = false;

                DataGridViewCellStyle colTotalPortStyle = admInputSummaryGrid.ColumnHeadersDefaultCellStyle;
                colTotalPortStyle.BackColor = Color.Black;
                colTotalPortStyle.ForeColor = Color.White;

                DataGridViewCellStyle rowTotalPortStyle = admInputSummaryGrid.RowHeadersDefaultCellStyle;
                rowTotalPortStyle.BackColor = Color.Black;
                rowTotalPortStyle.ForeColor = Color.White;

                for (int i = 0; i < admInputSummaryGrid.ColumnCount; i++)
                {
                    admInputSummaryGrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    admInputSummaryGrid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                //int col = 0;

                for (int admCount = 0; admCount < admSummaryFieldDisplayedTypeArray.Length; admCount++)
                {


                    admInputSummaryGrid.Columns[admCount].HeaderCell.Value 
                        = admSummaryFieldDisplayedTypeArray.GetValue(admCount).ToString().Replace('_',' ');
                }



                admInputSummaryGrid.RowCount = admSummaryFieldsList.Count;

                //int col = 0;



                for (int admRowCounter = 0; admRowCounter < admSummaryFieldsList.Count; admRowCounter++)
                {
                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.RecordType].Value
                        = admSummaryFieldsList[admRowCounter].RecordType;


                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.Office].Value
                        = admSummaryFieldsList[admRowCounter].POFFIC;

                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.Acct].Value
                        = admSummaryFieldsList[admRowCounter].PACCT;


                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.Description].Value
                        = admSummaryFieldsList[admRowCounter].Description;

                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.LongQuantity].Value
                        = (int)admSummaryFieldsList[admRowCounter].LongQuantity;

                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.ShortQuantity].Value
                        = (int)admSummaryFieldsList[admRowCounter].ShortQuantity;


                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.TradeDate].Value
                        = admSummaryFieldsList[admRowCounter].TradeDate;

                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.TradePrice].Value
                        = admSummaryFieldsList[admRowCounter].TradePrice;

                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.WeightedPrice].Value
                        = admSummaryFieldsList[admRowCounter].WeightedPrice;
                   

                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.SettledPrice].Value
                        = admSummaryFieldsList[admRowCounter].SettledPrice;


                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.SettledValue].Value
                        = admSummaryFieldsList[admRowCounter].SettledValue;

                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.Currency].Value
                        = admSummaryFieldsList[admRowCounter].Currency;

                    //                     admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_FIELDS.PSUBTY].Value
                    //                         = admSummaryFieldsList[admRowCounter].PSUBTY;

                    //                     admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_FIELDS.PEXCH].Value
                    //                         = admSummaryFieldsList[admRowCounter].PEXCH;

                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.PFC].Value
                        = admSummaryFieldsList[admRowCounter].PFC;

                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.Strike].Value
                        = admSummaryFieldsList[admRowCounter].aDMStrike;

                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.PCTYM].Value
                        = admSummaryFieldsList[admRowCounter].PCTYM;

                    admInputSummaryGrid.Rows[admRowCounter].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.INSTRUMENT_ID].Value
                        = admSummaryFieldsList[admRowCounter].instrumentArrayIdx;
                }

                updateSelectedInstrumentFromTreeADM(instrumentSelectedIdx);
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        public void highlightRow(int row, bool removeHighlight)
        {
            if (removeHighlight)
            {
                for (int colCount = 0; colCount < admInputSummaryGrid.Columns.Count; colCount++)
                {
                    admInputSummaryGrid.Rows[row].Cells[colCount].Style.BackColor = Color.White;
                }
            }
            else
            {
                //admInputSummaryGrid.Rows[row].DefaultCellStyle.BackColor = Color.GreenYellow;

                for (int colCount = 0; colCount < admInputSummaryGrid.Columns.Count; colCount++)
                {
                    admInputSummaryGrid.Rows[row].Cells[colCount].Style.BackColor = Color.GreenYellow;
                }

                //if (admSummaryFieldsList[row].notAllocated != 0)
                //{
                //    admInputSummaryGrid.Rows[row].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.Not_Allocated].Style.BackColor = Color.Red;
                //}
                //else
                //{
                //    admInputSummaryGrid.Rows[row].Cells[(int)ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED.Not_Allocated].Style.BackColor = Color.GreenYellow;
                //}
            }

            admInputSummaryGrid.Rows[row].Selected = false;
        }

        //public void fillFieldNotAllocated(int row, int col)
        //{
        //    admInputSummaryGrid.Rows[row].Cells[col].Value = admSummaryFieldsList[row].notAllocated;
            
        //    if(admSummaryFieldsList[row].notAllocated != 0)
        //    {
        //        admInputSummaryGrid.Rows[row].Cells[col].Style.BackColor = Color.Red;
        //    }
        //    else
        //    {
        //        admInputSummaryGrid.Rows[row].Cells[col].Style.BackColor = Color.GreenYellow;
        //    }

        //    //DefaultCellStyle.BackColor = Color.GreenYellow;
        //    //admInputSummaryGrid.Rows[row].Selected = false;
        //}


        private void admInputSummaryGrid_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void AdmReportSummaryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
#if DEBUG
            try
#endif
            {
                e.Cancel = true;
                this.Visible = false;
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        private void AdmReportSummaryForm_DragEnter(object sender, DragEventArgs e)
        {
#if DEBUG
            try
#endif
            {
                e.Effect = DragDropEffects.All;
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        //private void treeViewInstrumentsADM_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    TreeNode x = treeViewInstrumentsADM.SelectedNode;

        //    if (x != null)
        //    {
        //        //instrumentSelectedIdx = x.Index;

        //        //updateSelectedInstrumentFromTree();

        //        //Thread updateSelectedInstrumentFromTreeThread = new Thread(new ParameterizedThreadStart(updateSelectedInstrumentFromTree));

        //        //updateSelectedInstrumentFromTreeThread.Start();
        //    }
        //}

        public void updateSelectedInstrumentFromTreeADM(int instrumentSelectedIdx)
        {
            this.instrumentSelectedIdx = instrumentSelectedIdx;

            if (instrumentSelectedIdx == instruments.Length)
            {
                for (int admSummaryFieldsListCnt = 0;
                        admSummaryFieldsListCnt < admSummaryFieldsList.Count;
                        admSummaryFieldsListCnt++)
                {
                    hideUnhideADMContractSummaryLiveData(admSummaryFieldsListCnt, true);
                }
            }
            else
            {
                for (int admSummaryFieldsListCnt = 0;
                        admSummaryFieldsListCnt < admSummaryFieldsList.Count;
                        admSummaryFieldsListCnt++)
                {
                    if (instrumentSelectedIdx == admSummaryFieldsList[admSummaryFieldsListCnt].instrumentArrayIdx)
                    {
                        hideUnhideADMContractSummaryLiveData(admSummaryFieldsListCnt, true);
                    }
                    else
                    {
                        hideUnhideADMContractSummaryLiveData(admSummaryFieldsListCnt, false);
                    }
                }
            }
        }

        private void hideUnhideADMContractSummaryLiveData(int row, bool visible)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    ThreadSafeHideUnhideContractSummaryLiveDataDelegate d =
                        new ThreadSafeHideUnhideContractSummaryLiveDataDelegate(threadSafeHideUnhideADMContractSummaryLiveData);

                    this.Invoke(d, row, visible);
                }
                else
                {
                    threadSafeHideUnhideADMContractSummaryLiveData(row, visible);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        private void threadSafeHideUnhideADMContractSummaryLiveData(int row, bool visible)
        {
            try
            {
                int rowToUpdate = row;

                admInputSummaryGrid.Rows[rowToUpdate].Visible = visible;
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        delegate void ThreadSafeDisplayADMInputWithWebPositionsDelegate(OptionSpreadManager optionSpreadManager,
            int contractSummaryInstrumentSelectedIdx);

        public void displayADMInputWithWebPositions(OptionSpreadManager optionSpreadManager,
            int contractSummaryInstrumentSelectedIdx)
        {
            if (this.InvokeRequired)
            {
                ThreadSafeDisplayADMInputWithWebPositionsDelegate d = 
                    new ThreadSafeDisplayADMInputWithWebPositionsDelegate(
                    threadSafeDisplayADMInputWithWebPositions);

                this.Invoke(d, optionSpreadManager, contractSummaryInstrumentSelectedIdx);
            }
            else
            {
                threadSafeDisplayADMInputWithWebPositions(optionSpreadManager, contractSummaryInstrumentSelectedIdx);
            }
        }

        private void threadSafeDisplayADMInputWithWebPositions(OptionSpreadManager optionSpreadManager,
            int contractSummaryInstrumentSelectedIdx)
        {
            this.Show();

            this.BringToFront();

            setupAdmInputSummaryGrid(optionSpreadManager.admPositionImportWebForImportDisplay, contractSummaryInstrumentSelectedIdx);

        }
    }
}
