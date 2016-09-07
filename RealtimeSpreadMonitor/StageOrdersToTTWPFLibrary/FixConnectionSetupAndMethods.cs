using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix;
using QuickFix.Fields;
using StageOrdersToTTWPFLibrary.View;
using StageOrdersToTTWPFLibrary.Model;
using StageOrdersToTTWPFLibrary.ViewModel;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;


namespace StageOrdersToTTWPFLibrary
{
    public class FixConnectionSetupAndMethods
    {
        private QuickFix.Transport.SocketInitiator initiator = null;

        private ConnectionViewModel _connectionViewModel = null;
        internal void setConnectionViewModel(ConnectionViewModel connectionViewModel)
        {
            this._connectionViewModel = connectionViewModel;
        }

        private OrderViewModel _orderViewModel = null;
        internal void setOrderViewModel(OrderViewModel orderViewModel)
        {
            this._orderViewModel = orderViewModel;
        }

        private FixConnectionSystem _fixConnectionSystem = null;
        internal FixConnectionSystem getFixConnectionSystem()
        {
            return _fixConnectionSystem;
        }

        public event Action<QuickFix.FIX42.NewOrderSingle> OrderEvent;

        private ConcurrentDictionary<string, QuickFix.FIX42.NewOrderSingle> orderHashSet =
            new ConcurrentDictionary<string, QuickFix.FIX42.NewOrderSingle>();

        private ConcurrentDictionary<string, decimal> orderPriceHashSet =
            new ConcurrentDictionary<string, decimal>();

        internal void setupFixConnection()
        {
            //SessionSettings settings = new SessionSettings("D:\\RealtimeSpread\\RealtimeSpreadMonitor\\StageOrdersFIXConfigFiles\\stageorders.cfg");

            string dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "StageOrdersFIXConfigFiles\\stageorders.cfg");

            SessionSettings settings = new SessionSettings(dir);
            _fixConnectionSystem = new FixConnectionSystem(settings);
            IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
            ILogFactory logFactory = new FileLogFactory(settings);
            //QuickFix.SocketInitiatorThread acceptor = new SocketInitiatorThread(
            initiator = new QuickFix.Transport.SocketInitiator(
                _fixConnectionSystem,
                storeFactory,
                settings,
                logFactory);

            _fixConnectionSystem.Initiator = initiator;



            _fixConnectionSystem.dataRequestMessageReturned
                += new Action<QuickFix.FIX42.MarketDataSnapshotFullRefresh>(marketDataReturned);


            //QuickFix.SocketInitiatorThread initiatorThread = new QuickFix.SocketInitiatorThread(
            //    initiator,
            //    storeFactory,
            //    settings,
            //    logFactory);


            //initiator.
            //myApp.
            //while (true)
            //{
            //    System.Console.WriteLine("o hai");
            //    System.Threading.Thread.Sleep(1000);
            //}
            //initiator.Stop();

            //_fixConnectionSystem.LogonEvent += new Action(delegate() { _fixConnectionSystem.IsConnected = true; });
            //_fixConnectionSystem.LogoutEvent += new Action(delegate() { _fixConnectionSystem.IsConnected = false; });
        }

        internal void marketDataReturned(QuickFix.FIX42.MarketDataSnapshotFullRefresh m)
        {
            String id = m.MDReqID.ToString();

            QuickFix.FIX42.NewOrderSingle order = orderHashSet[id];



            //get price from market snapshot

            var bid = new QuickFix.FIX42.MarketDataSnapshotFullRefresh.NoMDEntriesGroup();
            bool setBid = false;

            //m.GetGroup(1, bid);

            var offer = new QuickFix.FIX42.MarketDataSnapshotFullRefresh.NoMDEntriesGroup();
            bool setOffer = false;

            //m.GetGroup(2, offer);

            for (int grpIndex = 1; grpIndex <= m.GetInt(Tags.NoMDEntries); grpIndex += 1)
            {
                if (grpIndex == 1)
                {
                    m.GetGroup(grpIndex, bid);
                    setBid = true;
                }
                else
                {
                    m.GetGroup(grpIndex, offer);
                    setOffer = true;
                }
            }

            decimal price = 0;

            //if (order.Side.Obj == Side.BUY)

            switch (order.Side.Obj)
            {
                case QuickFix.Fields.Side.BUY:
                    Console.Write("buy");

                    if (setBid)
                    {
                        price = bid.GetDecimal(Tags.MDEntryPx);
                    }

                    break;
                case QuickFix.Fields.Side.SELL:
                    Console.Write("sell");

                    if (setOffer)
                    {
                        price = offer.GetDecimal(Tags.MDEntryPx);
                    }

                    break;
            }

            if (!setBid && !setOffer)
            {
                price = orderPriceHashSet[id];
            }

            order.Price = new Price(price);

            if (OrderEvent != null)
            {
                OrderEvent(order);
            }

            if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsOrderConnected)
            {
                Thread sendToTargetThreading = new Thread(new ParameterizedThreadStart(sendToOrderToTarget));
                sendToTargetThreading.IsBackground = true;
                sendToTargetThreading.Start(order);

                //QuickFix.Session.SendToTarget(order, _fixConnectionSystem.orderSession.SessionID);
            }
        }

        private void sendToOrderToTarget(Object orderObject)
        {
            QuickFix.FIX42.NewOrderSingle order = (QuickFix.FIX42.NewOrderSingle)orderObject;            

            QuickFix.Session.SendToTarget(order, _fixConnectionSystem.orderSession.SessionID);
        }

        internal void logonFixConnection()
        {
            _fixConnectionSystem.Start();
        }

        internal void shutdownFixConnection()
        {
            _fixConnectionSystem.Stop();
        }

        internal void sendOrder(List<StageOrdersToTTWPFLibrary.Model.OrderModel> contractListToStage)
        {
            //_fixConnectionSystem.
            createStagedOrder(contractListToStage);
        }

        internal void sendOptionOrder()
        {
            //_fixConnectionSystem.
            //createStagedOptionOrder();
        }

        internal void sendDefRequest(List<StageOrdersToTTWPFLibrary.Model.OrderModel> contractListToStage)
        {
            //_fixConnectionSystem.createDefRequest();
            if (contractListToStage.Count > 0)
            {
                createSecDefRequest(contractListToStage[0]);
            }
        }

        private void createSecDefRequest(Model.OrderModel orderModel)
        {
            QuickFix.FIX42.SecurityDefinitionRequest order = new QuickFix.FIX42.SecurityDefinitionRequest();
                //new ClOrdID(DateTime.Now.ToOADate().ToString()),
                //new HandlInst('3'),
                //new Symbol("ES"),
                //new Side(Side.BUY),
                //new TransactTime(DateTime.Now),
                //new OrdType(OrdType.LIMIT));

            order.Symbol = new Symbol(orderModel.underlyingExchangeSymbol);

            order.SecurityExchange = new SecurityExchange(orderModel.underlyingExchange);

            order.SecurityType = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.securityType);

            if (orderModel.securityType == Enums.SECURITY_TYPE.OPTION)
            {
                order.PutOrCall = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.optionType);
            }

            order.StrikePrice = new StrikePrice(orderModel.optionStrikePrice); //new decimal(207000));

            order.MaturityMonthYear = new MaturityMonthYear(orderModel.maturityMonthYear);

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

            //QuickFix.FIX42.NewOrderSingle order = (QuickFix.FIX42.NewOrderSingle)orderObject;

            QuickFix.Session.SendToTarget(order, _fixConnectionSystem.orderSession.SessionID);
        }


        internal void createStagedOrder(List<StageOrdersToTTWPFLibrary.Model.OrderModel> contractListToStage)
        {
            for (int i = 0; i < contractListToStage.Count; i++)
            {
                if (contractListToStage[i].securityType == Enums.SECURITY_TYPE.FUTURE)
                {
                    StringBuilder messagId = new StringBuilder();
                    messagId.Append(contractListToStage[i].broker_18220);
                    messagId.Append(contractListToStage[i].acct);
                    //messagId.Append("_");
                    messagId.Append(contractListToStage[i].underlyingExchangeSymbol);
                    messagId.Append("_");
                    messagId.Append(contractListToStage[i].underlyingExchange);
                    messagId.Append("_");
                    messagId.Append(contractListToStage[i].securityType);
                    messagId.Append("_");
                    messagId.Append(contractListToStage[i].maturityMonthYear);
                    messagId.Append("_");

                    double tempTimeStamp = DateTime.Now.ToOADate();
                    int timeStampInt = (int)((tempTimeStamp - Math.Floor(tempTimeStamp)) * 100000000);
                    String timeStamp = timeStampInt.ToString();
                    messagId.Append(timeStamp);

                    

                    //QuickFix.FIX42.NewOrderSingle newOrder
                    //    = createStagedFutureOrder(contractListToStage[i]);


                    //QuickFix.FIX42.NewOrderSingle newOrder
                    //    = createHeldFutureOrder(contractListToStage[i]);

                    //QuickFix.FIX42.NewOrderSingle newOrder = null;



                    if (contractListToStage[i].orderPlacementType ==
                        (int)StageOrdersToTTWPFLibrary.Enums.ORDER_PLACEMENT_TYPE.STAGE_AND_HELD)
                    {
                        contractListToStage[i].messageId = messagId.ToString() + "STAGE";

                        QuickFix.FIX42.NewOrderSingle newStagedOrder
                            = createStagedFutureOrder(contractListToStage[i]);

                        if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsOrderConnected)
                        {
                            Thread sendToTargetThreading = new Thread(new ParameterizedThreadStart(sendToOrderToTarget));
                            sendToTargetThreading.IsBackground = true;
                            sendToTargetThreading.Start(newStagedOrder);
                        }

                        contractListToStage[i].messageId = messagId.ToString() + "HELD";

                        QuickFix.FIX42.NewOrderSingle newHeldOrder
                            = createHeldFutureOrder(contractListToStage[i]);

                        if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsOrderConnected)
                        {
                            Thread sendToTargetThreading = new Thread(new ParameterizedThreadStart(sendToOrderToTarget));
                            sendToTargetThreading.IsBackground = true;
                            sendToTargetThreading.Start(newHeldOrder);
                        }
                    }
                    else if (contractListToStage[i].orderPlacementType ==
                        (int)StageOrdersToTTWPFLibrary.Enums.ORDER_PLACEMENT_TYPE.STAGE)
                    {
                        contractListToStage[i].messageId = messagId.ToString() + "STAGE";

                        QuickFix.FIX42.NewOrderSingle newOrder
                            = createStagedFutureOrder(contractListToStage[i]);

                        if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsOrderConnected)
                        {
                            Thread sendToTargetThreading = new Thread(new ParameterizedThreadStart(sendToOrderToTarget));
                            sendToTargetThreading.IsBackground = true;
                            sendToTargetThreading.Start(newOrder);
                        }
                    }
                    else
                    {
                        contractListToStage[i].messageId = messagId.ToString() + "HELD";

                        QuickFix.FIX42.NewOrderSingle newOrder
                            = createHeldFutureOrder(contractListToStage[i]);

                        if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsOrderConnected)
                        {
                            Thread sendToTargetThreading = new Thread(new ParameterizedThreadStart(sendToOrderToTarget));
                            sendToTargetThreading.IsBackground = true;
                            sendToTargetThreading.Start(newOrder);
                        }
                    }

                    //orderHashSet.AddOrUpdate(
                    //    messagId.ToString(),
                    //    newOrder,
                    //    (oldKey, oldValue) => newOrder);

                    //orderPriceHashSet.AddOrUpdate(
                    //    messagId.ToString(),
                    //    contractListToStage[i].orderPrice,
                    //    (oldKey, oldValue) => contractListToStage[i].orderPrice);

                    //sendFuturesMarketDataRequest(contractListToStage[i]);

                    //newOrder.Price = new Price(contractListToStage[i].orderPrice);

                    
                }
                else
                {



                    StringBuilder messagId = new StringBuilder();
                    messagId.Append(contractListToStage[i].broker_18220);
                    messagId.Append(contractListToStage[i].acct);
                    //messagId.Append("_");
                    messagId.Append(contractListToStage[i].underlyingExchangeSymbol);
                    messagId.Append("_");
                    messagId.Append(contractListToStage[i].underlyingExchange);
                    messagId.Append("_");
                    messagId.Append(contractListToStage[i].securityType);
                    messagId.Append("_");
                    messagId.Append(contractListToStage[i].optionType);
                    messagId.Append("_");
                    messagId.Append(contractListToStage[i].optionStrikePrice);
                    messagId.Append("_");
                    messagId.Append(contractListToStage[i].maturityMonthYear);
                    messagId.Append("_");

                    double tempTimeStamp = DateTime.Now.ToOADate();
                    int timeStampInt = (int)((tempTimeStamp - Math.Floor(tempTimeStamp)) * 100000000);
                    String timeStamp = timeStampInt.ToString();
                    messagId.Append(timeStamp);

                    //contractListToStage[i].messageId = messagId.ToString();

                    //QuickFix.FIX42.NewOrderSingle newOrder
                    //    = createStagedOptionOrder(contractListToStage[i]);

                    //QuickFix.FIX42.NewOrderSingle newOrder = null;

                    if (contractListToStage[i].orderPlacementType ==
                        (int)StageOrdersToTTWPFLibrary.Enums.ORDER_PLACEMENT_TYPE.STAGE_AND_HELD)
                    {
                        contractListToStage[i].messageId = messagId.ToString() + "STAGE";

                        QuickFix.FIX42.NewOrderSingle newStagedOrder
                            = createStagedOptionOrder(contractListToStage[i]);

                        if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsOrderConnected)
                        {
                            Thread sendToTargetThreading = new Thread(new ParameterizedThreadStart(sendToOrderToTarget));
                            sendToTargetThreading.IsBackground = true;
                            sendToTargetThreading.Start(newStagedOrder);
                        }

                        contractListToStage[i].messageId = messagId.ToString() + "HELD";

                        QuickFix.FIX42.NewOrderSingle newHeldOrder
                            = createHeldOptionOrder(contractListToStage[i]);

                        if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsOrderConnected)
                        {
                            Thread sendToTargetThreading = new Thread(new ParameterizedThreadStart(sendToOrderToTarget));
                            sendToTargetThreading.IsBackground = true;
                            sendToTargetThreading.Start(newHeldOrder);
                        }
                    }
                    else if(contractListToStage[i].orderPlacementType == 
                        (int)StageOrdersToTTWPFLibrary.Enums.ORDER_PLACEMENT_TYPE.STAGE)
                    {
                        contractListToStage[i].messageId = messagId.ToString() + "STAGE";

                        QuickFix.FIX42.NewOrderSingle newOrder
                            = createStagedOptionOrder(contractListToStage[i]);

                        if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsOrderConnected)
                        {
                            Thread sendToTargetThreading = new Thread(new ParameterizedThreadStart(sendToOrderToTarget));
                            sendToTargetThreading.IsBackground = true;
                            sendToTargetThreading.Start(newOrder);
                        }
                    }
                    else
                    {
                        contractListToStage[i].messageId = messagId.ToString() + "HELD";

                        QuickFix.FIX42.NewOrderSingle newOrder
                            = createHeldOptionOrder(contractListToStage[i]);

                        if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsOrderConnected)
                        {
                            Thread sendToTargetThreading = new Thread(new ParameterizedThreadStart(sendToOrderToTarget));
                            sendToTargetThreading.IsBackground = true;
                            sendToTargetThreading.Start(newOrder);
                        }
                    }

                    

                    

                    //orderHashSet.AddOrUpdate(
                    //    messagId.ToString(),
                    //    newOrder,
                    //    (oldKey, oldValue) => newOrder);

                    //orderPriceHashSet.AddOrUpdate(
                    //    messagId.ToString(),
                    //    contractListToStage[i].orderPrice,
                    //    (oldKey, oldValue) => contractListToStage[i].orderPrice);

                    //sendOptionsMarketDataRequest(contractListToStage[i]);



                    //newOrder.Price = new Price(contractListToStage[i].orderPrice);

                    
                }
            }
        }

        private void sendFuturesMarketDataRequest(Model.OrderModel orderModel)
        {
            var mdrq = new QuickFix.FIX42.MarketDataRequest(
                new MDReqID(orderModel.messageId),  //DateTime.Now.ToOADate().ToString()),
                new SubscriptionRequestType(SubscriptionRequestType.SNAPSHOT),
                new MarketDepth(1)
                );

            mdrq.AggregatedBook =
                new AggregatedBook(AggregatedBook.ONE_BOOK_ENTRY_PER_SIDE_PER_PRICE);

            QuickFix.FIX42.MarketDataRequest.NoMDEntryTypesGroup entryTypeGroups =
                new QuickFix.FIX42.MarketDataRequest.NoMDEntryTypesGroup();

            entryTypeGroups.MDEntryType = new MDEntryType(MDEntryType.BID);

            mdrq.AddGroup(entryTypeGroups);

            entryTypeGroups.MDEntryType = new MDEntryType(MDEntryType.OFFER);

            mdrq.AddGroup(entryTypeGroups);


            QuickFix.FIX42.MarketDataRequest.NoRelatedSymGroup symGroup = new QuickFix.FIX42.MarketDataRequest.NoRelatedSymGroup();
            //symGroup.Symbol = new Symbol("FOO1");
            //symGroup.SecurityID = new SecurityID("secid1");

            symGroup.Symbol = new Symbol(orderModel.underlyingExchangeSymbol);

            symGroup.SecurityType = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.securityType);

            symGroup.SecurityExchange = new SecurityExchange(orderModel.underlyingExchange);

            symGroup.MaturityMonthYear = new MaturityMonthYear(orderModel.maturityMonthYear);



            mdrq.AddGroup(symGroup);



            //mdrq.MDUpdateType = new MDUpdateType(MDEntryType.BID);
            //mdrq.MDUpdateType = new MDUpdateType(MDEntryType.OFFER);

            //mdrq.NoRelatedSym = new NoRelatedSym(1);


            if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsPriceConnected)
            {
                Thread sendToTargetThreading = new Thread(new ParameterizedThreadStart(sendToMarketDataRequestToTarget));
                sendToTargetThreading.IsBackground = true;
                sendToTargetThreading.Start(mdrq);

                //QuickFix.Session.SendToTarget(mdrq, _fixConnectionSystem.priceSession.SessionID);
            }

        }

        private void sendToMarketDataRequestToTarget(Object marketDataRequestObject)
        {
            QuickFix.FIX42.MarketDataRequest maketDataRequest 
                = (QuickFix.FIX42.MarketDataRequest)marketDataRequestObject;

            QuickFix.Session.SendToTarget(maketDataRequest, _fixConnectionSystem.priceSession.SessionID);
        }

        private void sendOptionsMarketDataRequest(Model.OrderModel orderModel)
        {
            var mdrq = new QuickFix.FIX42.MarketDataRequest(
                new MDReqID(orderModel.messageId),  //DateTime.Now.ToOADate().ToString()),
                new SubscriptionRequestType(SubscriptionRequestType.SNAPSHOT),
                new MarketDepth(1)
                );

            mdrq.AggregatedBook =
                new AggregatedBook(AggregatedBook.ONE_BOOK_ENTRY_PER_SIDE_PER_PRICE);

            QuickFix.FIX42.MarketDataRequest.NoMDEntryTypesGroup entryTypeGroups =
                new QuickFix.FIX42.MarketDataRequest.NoMDEntryTypesGroup();

            entryTypeGroups.MDEntryType = new MDEntryType(MDEntryType.BID);

            mdrq.AddGroup(entryTypeGroups);

            entryTypeGroups.MDEntryType = new MDEntryType(MDEntryType.OFFER);

            mdrq.AddGroup(entryTypeGroups);


            QuickFix.FIX42.MarketDataRequest.NoRelatedSymGroup symGroup = new QuickFix.FIX42.MarketDataRequest.NoRelatedSymGroup();
            //symGroup.Symbol = new Symbol("FOO1");
            //symGroup.SecurityID = new SecurityID("secid1");

            symGroup.Symbol = new Symbol(orderModel.underlyingExchangeSymbol);

            symGroup.SecurityType = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.securityType);

            symGroup.SecurityExchange = new SecurityExchange(orderModel.underlyingExchange);

            symGroup.MaturityMonthYear = new MaturityMonthYear(orderModel.maturityMonthYear);


            symGroup.PutOrCall = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.optionType);

            symGroup.StrikePrice = new StrikePrice(orderModel.optionStrikePrice);  //new StrikePrice(new decimal(207000));


            //*************
            //order.SecurityType = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.securityType);

            //order.SecurityExchange = new SecurityExchange(orderModel.underlyingExchange);

            //order.PutOrCall = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.optionType);

            //order.StrikePrice = new StrikePrice(orderModel.optionStrikePrice);  //new StrikePrice(new decimal(207000));

            //order.MaturityMonthYear = new MaturityMonthYear(orderModel.maturityMonthYear);
            //*************

            mdrq.AddGroup(symGroup);



            //mdrq.MDUpdateType = new MDUpdateType(MDEntryType.BID);
            //mdrq.MDUpdateType = new MDUpdateType(MDEntryType.OFFER);

            //mdrq.NoRelatedSym = new NoRelatedSym(1);


            //if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsPriceConnected)
            //{
            //    QuickFix.Session.SendToTarget(mdrq, _fixConnectionSystem.priceSession.SessionID);
            //}

            if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsPriceConnected)
            {
                Thread sendToTargetThreading = new Thread(new ParameterizedThreadStart(sendToMarketDataRequestToTarget));
                sendToTargetThreading.IsBackground = true;
                sendToTargetThreading.Start(mdrq);

                //QuickFix.Session.SendToTarget(mdrq, _fixConnectionSystem.priceSession.SessionID);
            }

        }

        private QuickFix.FIX42.NewOrderSingle createHeldFutureOrder(Model.OrderModel orderModel)
        {


            var order = new QuickFix.FIX42.NewOrderSingle(
                new ClOrdID(orderModel.messageId),
                new HandlInst('1'),
                new Symbol(orderModel.underlyingExchangeSymbol),
                StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.side),
                new TransactTime(DateTime.Now),
                new OrdType(OrdType.LIMIT));

            //if(order.Side.Obj == Side.BUY)

            //switch (order.Side.Obj)
            //{
            //    case QuickFix.Fields.Side.BUY: 
            //        Console.Write("t");
            //        break;
            //    case QuickFix.Fields.Side.SELL: 
            //        Console.Write("t");
            //        break;
            //}



            order.SecurityType = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.securityType);

            order.SecurityExchange = new SecurityExchange(orderModel.underlyingExchange);

            order.SetField(new StringField(18203, orderModel.underlyingGateway));

            order.MaturityMonthYear = new MaturityMonthYear(orderModel.maturityMonthYear);

            order.OrderQty = new OrderQty(orderModel.orderQty);

            //order.Price = new Price(new decimal(207175));

            order.SetField(new CharField(11028, 'Y'));

            //order.SetField(new StringField(50, "TML1"));

            //order.SetField(new StringField(16142, "TML NORTH VANCOUVER"));

            order.SetField(new StringField(18220, orderModel.broker_18220));

            //order.SetField(new StringField(16106, orderModel.stagedOrderMessage));

            order.SetField(new CharField(16111, 'I'));

            order.Account = new Account(orderModel.acct);

            order.Price = new Price(orderModel.orderPrice);   //new Price(new decimal(208725));  //new Price(orderModel.orderPrice);

            order.ExecInst = new ExecInst("5");


            fillOrderInfoForICE(order, orderModel);


            //OrderRecord r = new OrderRecord(order);
            //lock (_orderViewModel.ordersLock)
            //{
            //    _orderViewModel.Orders.Add(r);
            //}

            //if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsOrderConnected)
            //{
            //    QuickFix.Session.SendToTarget(order, _fixConnectionSystem.orderSession.SessionID);
            //}

            return order;
        }

        private QuickFix.FIX42.NewOrderSingle createHeldOptionOrder(Model.OrderModel orderModel)
        {
            var order = new QuickFix.FIX42.NewOrderSingle(
                new ClOrdID(orderModel.messageId),
                new HandlInst('1'),
                new Symbol(orderModel.underlyingExchangeSymbol),
                StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.side),
                new TransactTime(DateTime.Now),
                new OrdType(OrdType.LIMIT));

            order.SecurityType = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.securityType);

            order.SecurityExchange = new SecurityExchange(orderModel.underlyingExchange);

            order.SetField(new StringField(18203, orderModel.underlyingGateway));

            order.PutOrCall = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.optionType);

            order.StrikePrice = new StrikePrice(orderModel.optionStrikePrice);  //new StrikePrice(new decimal(207000));

            order.MaturityMonthYear = new MaturityMonthYear(orderModel.maturityMonthYear);

            order.OrderQty = new OrderQty(orderModel.orderQty);


            order.SetField(new CharField(11028, 'Y'));

            //order.SetField(new StringField(50, "TML1"));

            //order.SetField(new StringField(16142, "TML NORTH VANCOUVER"));

            order.SetField(new StringField(18220, orderModel.broker_18220));

            //order.SetField(new StringField(16106, orderModel.stagedOrderMessage));

            order.SetField(new CharField(16111, 'I'));

            order.Account = new Account(orderModel.acct);

            order.Price = new Price(orderModel.orderPrice);

            order.ExecInst = new ExecInst("5");


            fillOrderInfoForICE(order, orderModel);


            //OrderRecord r = new OrderRecord(order);
            //lock (_orderViewModel.ordersLock)
            //{
            //    _orderViewModel.Orders.Add(r);
            //}

            //if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsOrderConnected)
            //{
            //    QuickFix.Session.SendToTarget(order, _fixConnectionSystem.orderSession.SessionID);
            //}

            return order;
        }

        private QuickFix.FIX42.NewOrderSingle createStagedFutureOrder(Model.OrderModel orderModel)
        {


            var order = new QuickFix.FIX42.NewOrderSingle(
                new ClOrdID(orderModel.messageId),
                new HandlInst('3'),
                new Symbol(orderModel.underlyingExchangeSymbol),
                StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.side),
                new TransactTime(DateTime.Now),
                new OrdType(OrdType.LIMIT));

            //if(order.Side.Obj == Side.BUY)

            //switch (order.Side.Obj)
            //{
            //    case QuickFix.Fields.Side.BUY: 
            //        Console.Write("t");
            //        break;
            //    case QuickFix.Fields.Side.SELL: 
            //        Console.Write("t");
            //        break;
            //}

            

            order.SecurityType = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.securityType);

            order.SecurityExchange = new SecurityExchange(orderModel.underlyingExchange);

            order.SetField(new StringField(18203, orderModel.underlyingGateway));

            order.MaturityMonthYear = new MaturityMonthYear(orderModel.maturityMonthYear);

            order.OrderQty = new OrderQty(orderModel.orderQty);

            //order.Price = new Price(new decimal(207175));

            order.SetField(new CharField(11028, 'Y'));

            //order.SetField(new StringField(50, "TML1"));

            //order.SetField(new StringField(16142, "TML NORTH VANCOUVER"));

            order.SetField(new StringField(18220, orderModel.broker_18220));

            order.SetField(new StringField(16106, orderModel.stagedOrderMessage));

            order.SetField(new CharField(16111, 'I'));

            order.Account = new Account(orderModel.acct);

            order.Price = new Price(0);


            fillOrderInfoForICE(order, orderModel);

            
            return order;
        }

        private QuickFix.FIX42.NewOrderSingle createStagedOptionOrder(Model.OrderModel orderModel)
        {
            var order = new QuickFix.FIX42.NewOrderSingle(
                new ClOrdID(orderModel.messageId),
                new HandlInst('3'),
                new Symbol(orderModel.underlyingExchangeSymbol),
                StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.side),
                new TransactTime(DateTime.Now),
                new OrdType(OrdType.LIMIT));

            order.SecurityType = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.securityType);

            order.SecurityExchange = new SecurityExchange(orderModel.underlyingExchange);

            order.SetField(new StringField(18203, orderModel.underlyingGateway));

            order.PutOrCall = StageOrdersToTTWPFLibrary.FixEnumTranslator.ToField(orderModel.optionType);

            order.StrikePrice = new StrikePrice(orderModel.optionStrikePrice);  //new StrikePrice(new decimal(207000));

            order.MaturityMonthYear = new MaturityMonthYear(orderModel.maturityMonthYear);

            order.OrderQty = new OrderQty(orderModel.orderQty);

            
            order.SetField(new CharField(11028, 'Y'));

            //order.SetField(new StringField(50, "TML1"));

            //order.SetField(new StringField(16142, "TML NORTH VANCOUVER"));

            order.SetField(new StringField(18220, orderModel.broker_18220));

            order.SetField(new StringField(16106, orderModel.stagedOrderMessage));

            order.SetField(new CharField(16111, 'I'));

            order.Account = new Account(orderModel.acct);

            order.Price = new Price(0);


            fillOrderInfoForICE(order, orderModel);

            //OrderRecord r = new OrderRecord(order);
            //lock (_orderViewModel.ordersLock)
            //{
            //    _orderViewModel.Orders.Add(r);
            //}

            //if (_fixConnectionSystem.InitiatorRunning && _connectionViewModel.IsOrderConnected)
            //{
            //    QuickFix.Session.SendToTarget(order, _fixConnectionSystem.orderSession.SessionID);
            //}

            return order;
        }

        private void fillOrderInfoForICE(QuickFix.FIX42.NewOrderSingle order, OrderModel orderModel)
        {
            if (orderModel.TAG47_Rule80A.useTag)
            {
                order.Rule80A = new Rule80A(orderModel.TAG47_Rule80A.tagCharValue);
            }

            if (orderModel.TAG204_CustomerOrFirm.useTag)
            {
                order.CustomerOrFirm = new CustomerOrFirm(orderModel.TAG204_CustomerOrFirm.tagIntValue);
            }

            if (orderModel.TAG18205_TTAccountType.useTag)
            {
                order.SetField(new StringField(18205, orderModel.TAG18205_TTAccountType.tagStringValue));
            }

            if (orderModel.TAG440_ClearingAccount.useTag)
            {
                order.ClearingAccount = new ClearingAccount(orderModel.TAG440_ClearingAccount.tagStringValue);
            }

            if (orderModel.TAG16102_FFT2.useTag)
            {
                order.SetField(new StringField(16102, orderModel.TAG16102_FFT2.tagStringValue));
            }
        }
    }
}
