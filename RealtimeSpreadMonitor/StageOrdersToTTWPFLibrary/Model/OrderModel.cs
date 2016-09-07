using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageOrdersToTTWPFLibrary.Model
{
    public class OrderModel
    {
        private string _messageId;
        public string messageId
        {
            get { return _messageId; }
            set { _messageId = value; }
        }

        private string _cqgSymbol;
        public string cqgSymbol
        {
            get { return _cqgSymbol; }
            set { _cqgSymbol = value; }
        }
        
        private int _optionMonthInt;
        public int optionMonthInt
        {
            get { return _optionMonthInt; }
            set { _optionMonthInt = value; }
        }

        private int _optionYear;
        public int optionYear
        {
            get { return _optionYear; }
            set { _optionYear = value; }
        }

        private decimal _optionStrikePrice;
        public decimal optionStrikePrice
        {
            get { return _optionStrikePrice; }
            set { _optionStrikePrice = value; }
        }

        private int _contractMonthint;
        public int contractMonthint
        {
            get { return _contractMonthint; }
            set { _contractMonthint = value; }
        }

        private int _contractYear;
        public int contractYear
        {
            get { return _contractYear; }
            set { _contractYear = value; }
        }

        private DateTime _expirationDate;
        public DateTime expirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }
        

        private string _underlyingExchangeSymbol;
        public string underlyingExchangeSymbol
        {
            get { return _underlyingExchangeSymbol; }
            set { _underlyingExchangeSymbol = value; }
        }

        private string _underlyingExchange;
        public string underlyingExchange
        {
            get { return _underlyingExchange; }
            set { _underlyingExchange = value; }
        }

        private string _underlyingGateway;
        public string underlyingGateway
        {
            get { return _underlyingGateway; }
            set { _underlyingGateway = value; }
        }

        private int _orderQty;
        public int orderQty
        {
            get { return _orderQty; }
            set { _orderQty = value; }
        }

        private int _lotsTotal;
        public int lotsTotal
        {
            get { return _lotsTotal; }
            set { _lotsTotal = value; }
        }

        private decimal _orderPrice;
        public decimal orderPrice
        {
            get { return _orderPrice; }
            set { _orderPrice = value; }
        }

        private Enums.SECURITY_TYPE _securityType;
        public Enums.SECURITY_TYPE securityType
        {
            get { return _securityType; }
            set { _securityType = value; }
        }

        private Enums.OPTION_TYPE _optionType;
        public Enums.OPTION_TYPE optionType
        {
            get { return _optionType; }
            set { _optionType = value; }
        }

        private Enums.Side _side;
        public Enums.Side side
        {
            get { return _side; }
            set { _side = value; }
        }


        private string _stagedOrderMessage;
        public string stagedOrderMessage
        {
            get { return _stagedOrderMessage; }
            set { _stagedOrderMessage = value; }
        }

        private string _maturityMonthYear;
        public string maturityMonthYear
        {
            get { return _maturityMonthYear; }
            set { _maturityMonthYear = value; }
        }

        private string _broker_18220;
        public string broker_18220
        {
            get { return _broker_18220; }
            set { _broker_18220 = value; }
        }

        private string _acct;
        public string acct
        {
            get { return _acct; }
            set { _acct = value; }
        }

        private int _orderPlacementType;
        public int orderPlacementType
        {
            get { return _orderPlacementType; }
            set { _orderPlacementType = value; }
        }

        //******************

        //format of exchange specific fields
        //FCM, TT Gateway, TT Exchange, TT symbol, TAG 47, TAG 204, TAG 18205 (TT ACCT), TAG 440, TAG 16102         

        public class TAGOBJECT
        {
            public bool useTag = false;
            public string tagStringValue;
            public char tagCharValue;
            public int tagIntValue;
        }

        private TAGOBJECT _TAG47_Rule80A = new TAGOBJECT();
        public TAGOBJECT TAG47_Rule80A
        {
            get { return _TAG47_Rule80A; }
        }

        private TAGOBJECT _TAG204_CustomerOrFirm = new TAGOBJECT();
        public TAGOBJECT TAG204_CustomerOrFirm
        {
            get { return _TAG204_CustomerOrFirm; }
        }

        private TAGOBJECT _TAG18205_TTAccountType = new TAGOBJECT();
        public TAGOBJECT TAG18205_TTAccountType
        {
            get { return _TAG18205_TTAccountType; }
        }

        private TAGOBJECT _TAG440_ClearingAccount = new TAGOBJECT();
        public TAGOBJECT TAG440_ClearingAccount
        {
            get { return _TAG440_ClearingAccount; }
        }

        private TAGOBJECT _TAG16102_FFT2 = new TAGOBJECT();
        public TAGOBJECT TAG16102_FFT2
        {
            get { return _TAG16102_FFT2; }
        }
    }
}
