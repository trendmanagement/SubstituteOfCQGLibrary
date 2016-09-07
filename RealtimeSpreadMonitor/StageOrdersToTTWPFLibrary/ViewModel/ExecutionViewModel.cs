using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StageOrdersToTTWPFLibrary.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace StageOrdersToTTWPFLibrary.ViewModel
{
    class ExecutionViewModel : ViewModelBase
    {
        private FixConnectionSystem _fixConnectionSystem;

        private Object _executionsLock = new Object();
        public ObservableCollection<ExecutionRecord> Executions { get; set; }

        public ExecutionViewModel(FixConnectionSystem fixConnectionSystem)
        {
            _fixConnectionSystem = fixConnectionSystem;
            Executions = new ObservableCollection<ExecutionRecord>();

            _fixConnectionSystem.Fix42ExecReportEvent += new Action<QuickFix.FIX42.ExecutionReport>(HandleExecutionReport);
        }

        public void HandleExecutionReport(QuickFix.FIX42.ExecutionReport msg)
        {
            try
            {
                string execId = msg.ExecID.Obj;
                string transType = FixEnumTranslator.Translate(msg.ExecTransType);
                string execType = FixEnumTranslator.Translate(msg.ExecType);

                Trace.WriteLine("EVM: Handling ExecutionReport: " + execId + " / " + transType + " / " + execType);

                String securityType = "";
                if (msg.IsSetField(167))
                {
                    securityType = FixEnumTranslator.Translate(msg.SecurityType);
                }

                String putOrCall = "";
                if (msg.IsSetField(201))
                {
                    putOrCall = FixEnumTranslator.Translate(msg.PutOrCall);
                }

                decimal strikePrice = 0;
                if (msg.IsSetField(202))
                {
                    strikePrice = msg.StrikePrice.Obj;
                }

                ExecutionRecord exRec = new ExecutionRecord(
                    msg.ExecID.Obj,
                    msg.OrderID.Obj,
                    transType,
                    execType,
                    msg.Symbol.Obj,
                    FixEnumTranslator.Translate(msg.Side),
                    securityType,
                    putOrCall,
                    strikePrice);

                SmartDispatcher.Invoke(new Action<ExecutionRecord>(AddExecution), exRec);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        public void AddExecution(ExecutionRecord r)
        {
            try
            {
                //Trace.WriteLine("add execution");
                //Executions.Add(r);

                Executions.Insert(0, r);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }
    }
}
