using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateEvents(EventInfo[] einfos, bool isInterface)
        {
            foreach (EventInfo einfo in SortEvents(einfos))
            {
                CreateEvent(einfo, isInterface);
            }
        }

        static void CreateEvent(EventInfo einfo, bool isInterface)
        {
            UpdateRegion(RegionType.Events);

            string public_ = !isInterface ? "public " : string.Empty;
            File.WriteLine(Indent1 + public_ + "event " + einfo.EventHandlerType.Name + " " + einfo.Name + ";" + Environment.NewLine);

        }

        static IEnumerable<EventInfo> SortEvents(EventInfo[] einfos)
        {
            return einfos.OrderBy(einfo => einfo.EventHandlerType.Name);
        }
    }
}