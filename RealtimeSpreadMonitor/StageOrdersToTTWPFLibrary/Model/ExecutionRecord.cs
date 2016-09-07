using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageOrdersToTTWPFLibrary.Model
{
    class ExecutionRecord : NotifyPropertyChangedBase
    {
        public ExecutionRecord(string pExecID, string pOrderID, string pExecTransType, 
            string pExecType, string pSymbol, string pSide,
            string pSecurityType, string pCallOrPut, decimal pStrike)
        {
            ExecID = pExecID;
            OrderID = pOrderID;
            ExecTransType = pExecTransType;
            ExecType = pExecType;
            Symbol = pSymbol;
            Side = pSide;

            SecurityType = pSecurityType;
            CallOrPut = pCallOrPut;
            Strike = pStrike;

        }

        private string _execID = "";
        private string _orderID = "";
        private string _execTransType = "";
        private string _execType = "";
        private string _symbol = "";
        private string _side = "";

        public string ExecID
        {
            get { return _execID; }
            set { _execID = value; OnPropertyChanged("ExecID"); }
        }

        public string OrderID
        {
            get { return _orderID; }
            set { _orderID = value; OnPropertyChanged("OrderID"); }
        }

        public string ExecTransType
        {
            get { return _execTransType; }
            set { _execTransType = value; OnPropertyChanged("ExecTransType"); }
        }

        public string ExecType
        {
            get { return _execType; }
            set { _execType = value; OnPropertyChanged("ExecType"); }
        }

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; OnPropertyChanged("Symbol"); }
        }

        public string Side
        {
            get { return _side; }
            set { _side = value; OnPropertyChanged("Side"); }
        }

        private string _securityType { get; set; }
        public string SecurityType
        {
            get { return _securityType; }
            set { _securityType = value; OnPropertyChanged("SecurityType"); }
        }

        private string _callOrPut { get; set; }
        public string CallOrPut
        {
            get { return _callOrPut; }
            set { _callOrPut = value; OnPropertyChanged("CallOrPut"); }
        }

        private decimal _strike = 0m;
        public decimal Strike
        {
            get { return _strike; }
            set { _strike = value; OnPropertyChanged("Strike"); }
        }
    }
}
