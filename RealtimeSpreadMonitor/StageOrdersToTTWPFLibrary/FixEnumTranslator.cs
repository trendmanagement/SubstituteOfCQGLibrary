using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageOrdersToTTWPFLibrary
{
    /// <summary>
    /// Translate: Convert field to meaningful string
    /// ToEnum: Convert field to a FIXApplication.Enum type
    /// ToField: Convert an enum to a QF field
    /// </summary>
    public static class FixEnumTranslator
    {
        public static string Translate(QuickFix.Fields.OrdStatus ordStatus)
        {
            switch (ordStatus.Obj)
            {
                case '0': return "New";
                case '1': return "PartiallyFilled";
                case '2': return "Filled";
                case '4': return "Canceled";
                case '5': return "Replaced";
                case '8': return "Rejected";
            }
            return "unknown";
        }

        public static string Translate(QuickFix.Fields.OrdType ordType)
        {
            switch (ordType.Obj)
            {
                case '1': return "Market";
                case '2': return "Limit";
            }
            return "unknown";
        }

        public static string Translate(QuickFix.Fields.ExecTransType execTransType)
        {
            switch (execTransType.Obj)
            {
                case QuickFix.Fields.ExecTransType.CANCEL: return "Cancel";
                case QuickFix.Fields.ExecTransType.CORRECT: return "Correct";
                case QuickFix.Fields.ExecTransType.NEW: return "New";
                case QuickFix.Fields.ExecTransType.STATUS: return "Status";
            }
            return "unknown";
        }

        public static string Translate(QuickFix.Fields.ExecType execType)
        {
            switch (execType.Obj)
            {
                case QuickFix.Fields.ExecType.CANCELED: return "Cancelled";
                case QuickFix.Fields.ExecType.FILL: return "Filled";
                case QuickFix.Fields.ExecType.NEW: return "New";
                case QuickFix.Fields.ExecType.REJECTED: return "Rejected";
                case QuickFix.Fields.ExecType.REPLACE: return "Replace";
                case QuickFix.Fields.ExecType.TRADE: return "Trade";
            }
            return "unknown";
        }

        public static string Translate(QuickFix.Fields.Side side)
        {
            switch (side.Obj)
            {
                case QuickFix.Fields.Side.BUY: return "Buy";
                case QuickFix.Fields.Side.SELL: return "Sell";
            }
            return "unknown";
        }

        public static string Translate(QuickFix.Fields.PutOrCall putOrCall)
        {
            switch (putOrCall.Obj)
            {
                case QuickFix.Fields.PutOrCall.CALL: return "Call";
                case QuickFix.Fields.PutOrCall.PUT: return "Put";
            }
            return "unknown";
        }

        public static string Translate(QuickFix.Fields.SecurityType securityType)
        {
            switch (securityType.Obj)
            {
                case QuickFix.Fields.SecurityType.FUTURE: return "Future";
                case QuickFix.Fields.SecurityType.OPTION: return "Option";
            }
            return "unknown";
        }

        /// <summary>
        /// Throws a ArgumentException if field value isn't supported
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Enums.TimeInForce ToEnum(QuickFix.Fields.TimeInForce field)
        {
            switch (field.Obj)
            {
                case QuickFix.Fields.TimeInForce.DAY: return Enums.TimeInForce.Day;
                case QuickFix.Fields.TimeInForce.GOOD_TILL_CANCEL: return Enums.TimeInForce.GoodTillCancel;
            }
            throw new ArgumentException(String.Format("Field value '{0}' not supported", field.Obj));
        }

        /// <summary>
        /// Throws a ArgumentException if param value not supported
        /// </summary>
        /// <param name="enume"></param>
        /// <returns></returns>
        public static QuickFix.Fields.TimeInForce ToField(Enums.TimeInForce enume)
        {
            switch (enume)
            {
                case Enums.TimeInForce.Day:
                    return new QuickFix.Fields.TimeInForce(QuickFix.Fields.TimeInForce.DAY);
                case Enums.TimeInForce.GoodTillCancel:
                    return new QuickFix.Fields.TimeInForce(QuickFix.Fields.TimeInForce.GOOD_TILL_CANCEL);
            }
            throw new ArgumentException(String.Format("Enum value '{0}' not supported", enume.ToString()));
        }

        /// <summary>
        /// Throws a ArgumentException if field value isn't supported
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Enums.Side ToEnum(QuickFix.Fields.Side field)
        {
            switch (field.Obj)
            {
                case QuickFix.Fields.Side.BUY: return Enums.Side.Buy;
                case QuickFix.Fields.Side.SELL: return Enums.Side.Sell;
            }
            throw new ArgumentException(String.Format("Field value '{0}' not supported", field.Obj));
        }

        /// <summary>
        /// Throws a ArgumentException if param value not supported
        /// </summary>
        /// <param name="enume"></param>
        /// <returns></returns>
        public static QuickFix.Fields.Side ToField(Enums.Side enume)
        {
            switch (enume)
            {
                case Enums.Side.Buy:
                    return new QuickFix.Fields.Side(QuickFix.Fields.Side.BUY);
                case Enums.Side.Sell:
                    return new QuickFix.Fields.Side(QuickFix.Fields.Side.SELL);
            }
            throw new ArgumentException(String.Format("Enum value '{0}' not supported", enume.ToString()));
        }

        /// <summary>
        /// Throws a ArgumentException if field value isn't supported
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Enums.OrderType ToEnum(QuickFix.Fields.OrdType field)
        {
            switch (field.Obj)
            {
                case QuickFix.Fields.OrdType.LIMIT: return Enums.OrderType.Limit;
                case QuickFix.Fields.OrdType.MARKET: return Enums.OrderType.Market;
            }
            throw new ArgumentException(String.Format("Field value '{0}' not supported", field.Obj));
        }

        /// <summary>
        /// Throws a ArgumentException if param value not supported
        /// </summary>
        /// <param name="enume"></param>
        /// <returns></returns>
        public static QuickFix.Fields.OrdType ToField(Enums.OrderType enume)
        {
            switch (enume)
            {
                case Enums.OrderType.Limit:
                    return new QuickFix.Fields.OrdType(QuickFix.Fields.OrdType.LIMIT);
                case Enums.OrderType.Market:
                    return new QuickFix.Fields.OrdType(QuickFix.Fields.OrdType.MARKET);
            }
            throw new ArgumentException(String.Format("Enum value '{0}' not supported", enume.ToString()));
        }


        /// <summary>
        /// Throws a ArgumentException if field value isn't supported
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Enums.SECURITY_TYPE ToEnum(QuickFix.Fields.SecurityType field)
        {
            switch (field.Obj)
            {
                case QuickFix.Fields.SecurityType.FUTURE: return Enums.SECURITY_TYPE.FUTURE;
                case QuickFix.Fields.SecurityType.OPTION: return Enums.SECURITY_TYPE.OPTION;
            }
            throw new ArgumentException(String.Format("Field value '{0}' not supported", field.Obj));
        }

        /// <summary>
        /// Throws a ArgumentException if param value not supported
        /// </summary>
        /// <param name="enume"></param>
        /// <returns></returns>
        public static QuickFix.Fields.SecurityType ToField(Enums.SECURITY_TYPE enume)
        {
            switch (enume)
            {
                case Enums.SECURITY_TYPE.FUTURE:
                    return new QuickFix.Fields.SecurityType(QuickFix.Fields.SecurityType.FUTURE);
                case Enums.SECURITY_TYPE.OPTION:
                    return new QuickFix.Fields.SecurityType(QuickFix.Fields.SecurityType.OPTION);
            }
            throw new ArgumentException(String.Format("Enum value '{0}' not supported", enume.ToString()));
        }


        /// <summary>
        /// Throws a ArgumentException if field value isn't supported
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Enums.OPTION_TYPE ToEnum(QuickFix.Fields.PutOrCall field)
        {
            switch (field.Obj)
            {
                case QuickFix.Fields.PutOrCall.CALL: return Enums.OPTION_TYPE.CALL;
                case QuickFix.Fields.PutOrCall.PUT: return Enums.OPTION_TYPE.PUT;
            }
            throw new ArgumentException(String.Format("Field value '{0}' not supported", field.Obj));
        }

        /// <summary>
        /// Throws a ArgumentException if param value not supported
        /// </summary>
        /// <param name="enume"></param>
        /// <returns></returns>
        public static QuickFix.Fields.PutOrCall ToField(Enums.OPTION_TYPE enume)
        {
            switch (enume)
            {
                case Enums.OPTION_TYPE.CALL:
                    return new QuickFix.Fields.PutOrCall(QuickFix.Fields.PutOrCall.CALL);
                case Enums.OPTION_TYPE.PUT:
                    return new QuickFix.Fields.PutOrCall(QuickFix.Fields.PutOrCall.PUT);
            }
            throw new ArgumentException(String.Format("Enum value '{0}' not supported", enume.ToString()));
        }
    }
}
