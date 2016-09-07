using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StageOrdersToTTWPFLibrary.Model;
using System.Diagnostics;

namespace StageOrdersToTTWPFLibrary.ViewModel
{
    class OrderViewModel : ViewModelBase
    {

        private delegate void ThreadSafeDisplayOrder(QuickFix.FIX42.NewOrderSingle orderSingle);

        private FixConnectionSystem _fixConnectionSystem;
        //private ICustomFixStrategy _strategy = null;

        private Object _ordersLock = new Object();        

        public ObservableCollection<OrderRecord> Orders { get; set; }

        public OrderViewModel(FixConnectionSetupAndMethods fixConnSetupAndMethods,
            FixConnectionSystem fixConnectionSystem)
        {
            this._fixConnectionSystem = fixConnectionSystem;

            Orders = new ObservableCollection<OrderRecord>();

            _fixConnectionSystem.Fix42ExecReportEvent += new Action<QuickFix.FIX42.ExecutionReport>(HandleExecutionReport);

            fixConnSetupAndMethods.OrderEvent += new Action<QuickFix.FIX42.NewOrderSingle>(displayOrder);
        }

        public void displayOrder(QuickFix.FIX42.NewOrderSingle orderSingle)
        {
            //if (this.InvokeRequired)
            //{
            //    ThreadSafeMoveSplitterDistance d =
            //        new ThreadSafeMoveSplitterDistance(threadSafeMoveSplitter);

            //    this.Invoke(d, 0);
            //}
            //else
            //{
            //    threadSafeMoveSplitter(0);
            //}

            //ThreadSafeDisplayOrder d = new ThreadSafeDisplayOrder(displayOrderThreadSafe);

            //this.d.invoke()

            SmartDispatcher.Invoke(displayOrderThreadSafe, orderSingle);
        }

        private void displayOrderThreadSafe(QuickFix.FIX42.NewOrderSingle orderSingle)
        {
            try
            {
                //Trace.WriteLine(String.Format("Send New Order: Type={0} Side={1} Symbol=[{2}] Qty=[{3}] LimitPrice=[{4}] TIF={5}",
                //    this.OrderType.ToString(), side.ToString(), this.Symbol,
                //    this.OrderQtyString, this.LimitPriceString, this.TimeInForce.ToString()));

                //Dictionary<int, string> customFieldsDict = new Dictionary<int, string>();
                //foreach (CustomFieldRecord cfr in this.CustomFields)
                //    customFieldsDict[cfr.Tag] = cfr.Value;

                //int orderQty = int.Parse(this.OrderQtyString);
                //decimal limitPrice = decimal.Parse(this.LimitPriceString);

                //QuickFix.FIX42.NewOrderSingle nos = MessageCreator42.NewOrderSingle(
                //    customFieldsDict,
                //    this.OrderType, side, this.Symbol, orderQty, this.TimeInForce, limitPrice);

                OrderRecord r = new OrderRecord(orderSingle);
                lock (_ordersLock)
                {
                    //Orders.Add(r);
                    Orders.Insert(0, r);
                }

                //_qfapp.Send(nos);

            }
            catch (Exception e)
            {
                Trace.WriteLine("Failed to send order\n" + e.ToString());
            }
        }

        public void HandleExecutionReport(QuickFix.FIX42.ExecutionReport msg)
        {
            try
            {
                if (msg.IsSetField(11) && msg.ClOrdID != null)
                {
                    string clOrdId = msg.ClOrdID.Obj;


                    string status = FixEnumTranslator.Translate(msg.OrdStatus);

                    //Trace.WriteLine("OVM: Handling ExecutionReport: " + clOrdId + " / " + status);

                    lock (_ordersLock)
                    {
                        foreach (OrderRecord r in Orders)
                        {
                            if (r.ClOrdID == clOrdId)
                            {
                                r.Status = status;
                                if (msg.IsSetLastPx())
                                    r.Price = msg.LastPx.Obj;
                                if (msg.IsSetOrderID())
                                    r.OrderID = msg.OrderID.Obj;

                                return;
                            }
                        }
                    }

                    //Trace.WriteLine("OVM: No order corresponds to ClOrdID '" + clOrdId + "'");
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        //public void CancelOrder(OrderRecord or)
        //{
        //    try
        //    {
        //        QuickFix.FIX42.OrderCancelRequest ocq = new QuickFix.FIX42.OrderCancelRequest(
        //            new QuickFix.Fields.OrigClOrdID(or.ClOrdID),
        //            MessageCreator42.GenerateClOrdID(),
        //            new QuickFix.Fields.Symbol(or.Symbol),
        //            or.OriginalNOS.Side,
        //            new QuickFix.Fields.TransactTime(DateTime.Now));

        //        ocq.OrderID = new QuickFix.Fields.OrderID(or.OrderID);

        //        _strategy.ProcessOrderCancelRequest(or.OriginalNOS, ocq);

        //        _qfapp.Send(ocq);
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.WriteLine(e.ToString());
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="or"></param>
        ///// <param name="newQty"></param>
        ///// <param name="newPrice">ignored if not applicable for order type</param>
        ///// <param name="customFields">other custom fields to be added</param>
        //public void CancelReplaceOrder(OrderRecord or, int newQty, decimal newPrice, Dictionary<int, string> customFields)
        //{
        //    try
        //    {
        //        QuickFix.FIX42.OrderCancelReplaceRequest ocrq = MessageCreator42.OrderCancelReplaceRequest(
        //            customFields, or.OriginalNOS, newQty, newPrice);

        //        ocrq.OrderID = new QuickFix.Fields.OrderID(or.OrderID);

        //        _strategy.ProcessOrderCancelReplaceRequest(or.OriginalNOS, ocrq);

        //        _qfapp.Send(ocrq);
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.WriteLine(e.ToString());
        //    }
        //}
    }
}
