using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageOrdersToTTWPFLibrary.Enums
{
    public enum Side
    {
        Buy,
        Sell
    }

    public enum OrderType
    {
        Market,
        Limit
    }

    public enum TimeInForce
    {
        Day,
        GoodTillCancel
    }

    public enum SECURITY_TYPE
    {
        FUTURE,
        OPTION
    }

    public enum OPTION_TYPE
    {
        CALL,
        PUT
    }

    public enum ORDER_PLACEMENT_TYPE
    {
        STAGE_AND_HELD,
        STAGE,
        HELD
    }
}
