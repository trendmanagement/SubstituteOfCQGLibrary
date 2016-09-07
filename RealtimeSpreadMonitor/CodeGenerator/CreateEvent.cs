using System;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateEvent(EventInfo einfo, bool isInterface)
        {
            UpdateRegion(RegionType.Events);

            string public_ = !isInterface ? "public " : string.Empty;
            File.WriteLine(Indent1 + public_ + "event " + einfo.EventHandlerType.Name + " " + einfo.Name + ";" + Environment.NewLine);
        }
    }
}