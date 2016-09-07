using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix;
using QuickFix.Fields;
using StageOrdersToTTWPFLibrary.View;


namespace StageOrdersToTTWPFLibrary
{
    internal class FixConnectionSystem : QuickFix.MessageCracker, QuickFix.IApplication
    {
        

        //private StageOrdersMainView _stagingOrderView = null;

        //public QuickFix.SessionID ActiveSessionID { get; set; }
        public QuickFix.SessionSettings MySessionSettings { get; set; }

        private Session _orderSession = null;
        internal Session orderSession
        {
            get { return _orderSession; }
            set { _orderSession = value; }
        }

        private Session _priceSession = null;
        internal Session priceSession
        {
            get { return _priceSession; }
            set { _priceSession = value; }
        }

        //private bool _IsConnected;
        //internal bool IsConnected
        //{
        //    get { return _IsConnected; }
        //    set { _IsConnected = value; }
        //}

        private bool initiatorRunning;
        internal bool InitiatorRunning
        {
            get { return initiatorRunning; }
            set { initiatorRunning = value; }
        }

        public FixConnectionSystem(SessionSettings settings) //StageOrdersMainView stagingOrderView)
        {
            //ActiveSessionID = null;
            MySessionSettings = settings;

            //HashSet<QuickFix.SessionID> sidset = settings.GetSessions();

            //orderSession = Session.LookupSession(sidset.ElementAt(1).
        }

        private QuickFix.IInitiator _initiator = null;
        public QuickFix.IInitiator Initiator
        {
            set
            {
                if (_initiator != null)
                    throw new Exception("You already set the initiator");
                _initiator = value;
            }
            get
            {
                if (_initiator == null)
                    throw new Exception("You didn't provide an initiator");
                return _initiator;
            }
        }

        /// <summary>
        /// Triggered on any message sent or received (arg1: isIncoming)
        /// </summary>
        public event Action<QuickFix.Message, bool> MessageEvent;

        public event Action<QuickFix.FIX42.ExecutionReport> Fix42ExecReportEvent;

        public event Action OrderSessionLogonEvent;
        public event Action OrderSessionLogoutEvent;

        public event Action PriceSessionLogonEvent;
        public event Action PriceSessionLogoutEvent;

        public event Action<QuickFix.FIX42.MarketDataSnapshotFullRefresh> dataRequestMessageReturned;

        public void Start()
        {
            //Trace.WriteLine("QFApp::Start() called");
            
            if (Initiator.IsStopped)
                Initiator.Start();
            //else
            //    Trace.WriteLine("(already started)");

            InitiatorRunning = true;
        }

        public void Stop()
        {
            InitiatorRunning = false;

            //Trace.WriteLine("QFApp::Stop() called");
            Initiator.Stop();
        }


        public void ToApp(Message message, SessionID sessionID)
        {
            //try
            //{
            //    bool possDupFlag = false;
            //    if (message.Header.IsSetField(QuickFix.Fields.Tags.PossDupFlag))
            //    {
            //        possDupFlag = QuickFix.Fields.Converters.BoolConverter.Convert(
            //            message.Header.GetField(QuickFix.Fields.Tags.PossDupFlag)); /// FIXME
            //    }
            //    if (possDupFlag)
            //        throw new DoNotSend();
            //}
            //catch (FieldNotFoundException)
            //{ }

            Console.WriteLine();
            Console.WriteLine("OUT: " + message.ToString());
        
        }

        public void OnCreate(SessionID sessionID)
        {
            //Console.WriteLine("Logon - " + sessionID.ToString());


            HashSet<QuickFix.SessionID> sidset = MySessionSettings.GetSessions();


            if (sessionID.TargetCompID.CompareTo(sidset.ElementAt(0).TargetCompID) == 0)
            {
                orderSession = Session.LookupSession(sessionID);
            }
            else
            {
                priceSession = Session.LookupSession(sessionID);
            }

            //_session = Session.LookupSession(sessionID);
        }

        public void OnLogon(SessionID sessionID) { 
            //Console.WriteLine("Logon - " + sessionID.ToString());

            //this.ActiveSessionID = sessionID;

            //Session tempSession = Session.LookupSession(sessionID);
            //tempSession.

            //IsConnected = true;

            HashSet<QuickFix.SessionID> sessionIDSet = MySessionSettings.GetSessions();

            //sessionIDSet.ElementAt(0).SenderLocationID

            if (sessionID.TargetCompID.CompareTo(sessionIDSet.ElementAt(0).TargetCompID) == 0)
            {
                //orderSession = Session.LookupSession(sessionID);

                if (OrderSessionLogonEvent != null)
                    OrderSessionLogonEvent();
            }
            else
            {
                //priceSession = Session.LookupSession(sessionID);


                if (PriceSessionLogonEvent != null)
                    PriceSessionLogonEvent();
            }

            
        }

        public void OnLogout(SessionID sessionID) {
            Console.WriteLine("Logout - " + sessionID.ToString());

            //IsConnected = false;

            HashSet<QuickFix.SessionID> sidset = MySessionSettings.GetSessions();


            if (sessionID.TargetCompID.CompareTo(sidset.ElementAt(0).TargetCompID) == 0)
            {
                //orderSession = Session.LookupSession(sessionID);

                if (OrderSessionLogoutEvent != null)
                    OrderSessionLogoutEvent();
            }
            else
            {
                //priceSession = Session.LookupSession(sessionID);


                if (PriceSessionLogoutEvent != null)
                    PriceSessionLogoutEvent();
            }
        }

        public void FromAdmin(Message message, SessionID sessionID) {
            Console.WriteLine("IN:  " + message.ToString());

            if (MessageEvent != null && InitiatorRunning)
                MessageEvent(message, true);
        }
        public void ToAdmin(Message message, SessionID sessionID)
        {
            

            if (isMessageOfType(message, MsgType.LOGON))
            {

                addLogonField((QuickFix.FIX42.Logon)message, sessionID);
            }

            Console.WriteLine("OUT:  " + message.ToString());

            if (MessageEvent != null && InitiatorRunning)
                MessageEvent(message, false);
        }

        public void FromApp(Message message, SessionID sessionID)
        {
            Console.WriteLine("IN:  " + message.ToString());
            try
            {
                Crack(message, sessionID);
            }
            catch (Exception ex)
            {
                Console.WriteLine("==Cracker exception==");
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.StackTrace);
            }
        }

        #region MessageCracker handlers

        public void OnMessage(QuickFix.FIX42.BusinessMessageReject m, SessionID s)
        {
            Console.WriteLine("Received BusinessMessageReject Message");

            //if (Fix42ExecReportEvent != null)
            //    Fix42ExecReportEvent(m);
        }

        public void OnMessage(QuickFix.FIX42.ExecutionReport m, SessionID s)
        {
            Console.WriteLine("Received execution report");

            if (Fix42ExecReportEvent != null)
                Fix42ExecReportEvent(m);
        }

        public void OnMessage(QuickFix.FIX42.OrderCancelReject m, SessionID s)
        {
            Console.WriteLine("Received order cancel reject");
        }

        public void OnMessage(QuickFix.FIX42.Reject m, SessionID s)
        {
            Console.WriteLine("General Reject");
        }

        public void OnMessage(QuickFix.FIX42.SecurityDefinition m, SessionID s)
        {
            Console.WriteLine("Security Definition");
        }

        public void OnMessage(QuickFix.FIX42.MarketDataRequestReject m, SessionID s)
        {
            Console.WriteLine("Data Request Reject");
        }

        public void OnMessage(QuickFix.FIX42.MarketDataSnapshotFullRefresh m, SessionID s)
        {
            Console.WriteLine("Data Request");

            if (dataRequestMessageReturned != null)
                dataRequestMessageReturned(m);
        }

        #endregion

        private bool isMessageOfType(Message message, String type)
        {
            try
            {
                //MsgType.LOGON
                //message.Header.GetField
                return type.CompareTo(message.Header.GetField(new MsgType()).getValue()) == 0;
            }
            catch (Exception e)
            {
                logErrorToSessionLog(message, e);
                return false;
            }
        }

        private void addLogonField(QuickFix.FIX42.Logon message, SessionID sessionID)
        {
            //message.RawData
            //		System.out.println("sessionID.getSessionQualifier() " + sessionID.getSessionQualifier());
            message.Header.SetField(new RawDataLength(8));
            
            //if (sessionID.TargetSubID.CompareTo("ORDER") == 0)
            //{
                message.Header.SetField(new RawData(sessionID.SessionQualifier));
            //}
            //else
            //{
            //    message.Header.SetField(new RawData(sessionID.SessionQualifier));
            //}
        }

        private void logErrorToSessionLog(Message message, Exception e)
        {
            //LogUtil.logThrowable(QuickFix.MessageCracker. MessageUtils.getSessionID(message), e.getMessage(), e);
        }

        //internal void createStagedOrder(List<StageOrdersToTTWPFLibrary.Model.OrderModel> contractListToStage)
        //{
        //    for(int i=0; i < contractListToStage.Count; i++)
        //    {
        //        if (contractListToStage[i].securityType == Enums.SECURITY_TYPE.FUTURE)
        //        {
        //            sendStagedFutureOrder(contractListToStage[i]);
        //        }
        //        else
        //        {
        //            sendStagedOptionOrder(contractListToStage[i]);
        //        }
        //    }
        //}

        //private void sendStagedFutureOrder(Model.OrderModel orderModel)
        //{
            

        //    var order = new QuickFix.FIX42.NewOrderSingle(
        //        new ClOrdID(DateTime.Now.ToOADate().ToString()),
        //        new HandlInst('3'),
        //        new Symbol(orderModel.underlyingExchangeSymbol),
        //        StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.side),
        //        new TransactTime(DateTime.Now),
        //        new OrdType(OrdType.MARKET));

        //    order.SecurityType = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.securityType);

        //    order.SecurityExchange = new SecurityExchange(orderModel.underlyingExchange);

        //    order.MaturityMonthYear = new MaturityMonthYear(orderModel.maturityMonthYear);

        //    order.OrderQty = new OrderQty(orderModel.orderQty);

        //    //order.Price = new Price(new decimal(207175));

        //    order.SetField(new StringField(16106, orderModel.stagedOrderMessage));

        //    order.SetField(new CharField(16111, 'I'));

        //    order.Account = new Account("tmqrexodts");

        //    OrderRecord r = new OrderRecord(order);
        //    lock (_ordersLock)
        //    {
        //        Orders.Add(r);
        //    }

        //    if (InitiatorRunning && IsConnected)
        //    {
        //        QuickFix.Session.SendToTarget(order, _session.SessionID);
        //    }
        //}

        //internal void sendStagedOptionOrder(Model.OrderModel orderModel)
        //{
        //    var order = new QuickFix.FIX42.NewOrderSingle(
        //        new ClOrdID(DateTime.Now.ToOADate().ToString()),
        //        new HandlInst('3'),
        //        new Symbol(orderModel.underlyingExchangeSymbol),
        //        StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.side),
        //        new TransactTime(DateTime.Now),
        //        new OrdType(OrdType.MARKET));
            
        //    order.SecurityType = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.securityType);

        //    order.SecurityExchange = new SecurityExchange(orderModel.underlyingExchange);

        //    order.PutOrCall = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.optionType);

        //    order.StrikePrice = new StrikePrice(orderModel.optionStrikePrice);  //new StrikePrice(new decimal(207000));

        //    order.MaturityMonthYear = new MaturityMonthYear(orderModel.maturityMonthYear);

        //    order.OrderQty = new OrderQty(orderModel.orderQty);

        //    //order.Price = new Price(new decimal(1300));

        //    order.SetField(new StringField(16106, orderModel.stagedOrderMessage));

        //    order.SetField(new CharField(16111, 'I'));

        //    order.Account = new Account("tmqrexodts");



        //    QuickFix.Session.SendToTarget(order, _session.SessionID);
        //}

        //internal void createStagedOrderXXX()
        //{
        //    var order = new QuickFix.FIX42.NewOrderSingle(
        //        new ClOrdID(DateTime.Now.ToOADate().ToString()),
        //        new HandlInst('3'),
        //        new Symbol("ES"),
        //        new Side(Side.BUY),
        //        new TransactTime(DateTime.Now),
        //        new OrdType(OrdType.LIMIT));

        //    order.SecurityType = new SecurityType(SecurityType.FUTURE);

        //    order.SecurityExchange = new SecurityExchange("CME");

        //    order.MaturityMonthYear = new MaturityMonthYear("201412");

        //    order.OrderQty = new OrderQty(2);

        //    order.Price = new Price(new decimal(207175));

        //        //new HandlInst()
        //        //new ClOrdID("1234"),
        //        //new Symbol("AAPL"),
        //        //new Side(Side.BUY),
        //        //new TransactTime(DateTime.Now),
        //        //new OrdType(OrdType.LIMIT));

        //    order.SetField(new StringField(16106, "StagedOrderMsg"));

        //    order.SetField(new CharField(16111, 'I'));

        //    order.Account = new Account("tmqrexodts");


        //    if (InitiatorRunning && IsConnected)
        //    {
        //        QuickFix.Session.SendToTarget(order, _session.SessionID);
        //    }
        //}

        //internal void createStagedOptionOrder()
        //{
        //    var order = new QuickFix.FIX42.NewOrderSingle(
        //        new ClOrdID(DateTime.Now.ToOADate().ToString()),
        //        new HandlInst('3'),
        //        new Symbol("ES"),
        //        new Side(Side.BUY),
        //        new TransactTime(DateTime.Now),
        //        new OrdType(OrdType.LIMIT));



        //    order.SecurityExchange = new SecurityExchange("CME");

        //    order.SecurityType = new SecurityType("OPT");
            
        //    order.PutOrCall = new PutOrCall(PutOrCall.CALL);

        //    order.StrikePrice = new StrikePrice(new decimal(207000));

        //    order.MaturityMonthYear = new MaturityMonthYear("201501");

        //    order.OrderQty = new OrderQty(2);

        //    order.Price = new Price(new decimal(1300));

        //    //new HandlInst()
        //    //new ClOrdID("1234"),
        //    //new Symbol("AAPL"),
        //    //new Side(Side.BUY),
        //    //new TransactTime(DateTime.Now),
        //    //new OrdType(OrdType.LIMIT));

        //    order.SetField(new StringField(16106, "StagedOrderMsg"));

        //    order.SetField(new CharField(16111, 'I'));

        //    order.Account = new Account("tmqrexodts");



        //    QuickFix.Session.SendToTarget(order, _session.SessionID);
        //}

        internal void createDefRequest()
        {
            QuickFix.FIX42.SecurityDefinitionRequest order = new QuickFix.FIX42.SecurityDefinitionRequest();
                //new ClOrdID(DateTime.Now.ToOADate().ToString()),
                //new HandlInst('3'),
                //new Symbol("ES"),
                //new Side(Side.BUY),
                //new TransactTime(DateTime.Now),
                //new OrdType(OrdType.LIMIT));

            order.Symbol = new Symbol("ES");

            order.SecurityExchange = new SecurityExchange("CME");

            order.SecurityType = new SecurityType("OPT");

            order.PutOrCall = new PutOrCall(1);

            order.StrikePrice = new StrikePrice(207000); //new decimal(207000));

            order.MaturityMonthYear = new MaturityMonthYear("201501");

            //order.OrderQty = new OrderQty(2);

            //order.Price = new Price(new decimal(1300));

            //new HandlInst()
            //new ClOrdID("1234"),
            //new Symbol("AAPL"),
            //new Side(Side.BUY),
            //new TransactTime(DateTime.Now),
            //new OrdType(OrdType.LIMIT));

            //order.SetField(new StringField(16106, "StagedOrderMsg"));

            //order.SetField(new CharField(16111, 'I'));

            //order.Account = new Account("tmqrexodts");



            //QuickFix.Session.SendToTarget(order, _session.SessionID);
        }

    //    public void OnMessage(
    //QuickFix.FIX42.NewOrderSingle ord,
    //SessionID sessionID)
    //    {
    //        ProcessOrder(ord.Price, ord.OrderQty, ord.Account);
    //    }

    //    public void OnMessage(
    //        QuickFix.FIX44.SecurityDefinition secDef,
    //        SessionID sessionID)
    //    {
    //        GotSecDef(secDef);
    //    }


    }
}
