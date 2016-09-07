using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace RealtimeSpreadMonitor.FormManipulation
{
    class CommonFormManipulation
    {
        internal static Color plUpDownColor(double value)
        {
            if (value >= 0)
            {
                return Color.LawnGreen;
            }
            else
            {
                return Color.Red;
            }
        }

        
    }
}
