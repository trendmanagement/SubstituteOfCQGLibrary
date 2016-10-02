using System;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        const int EventCheckingTimerInterval = 30;  // ms

        static void CreateCtors(Type type, bool eventsChecking)
        {
            UpdateRegion(RegionType.Constructors);

            ConstructorInfo[] cinfos = type.GetConstructors();

            // Add public constructors
            foreach (ConstructorInfo cinfo in cinfos)
            {
                CreatePublicCtor(cinfo, eventsChecking);
            }

            if (!type.IsValueType)
            {
                // Add an internal costructor taking data collector object key
                CreateInternalCtor(type.Name, eventsChecking);
            }
        }

        static void CreatePublicCtor(ConstructorInfo cinfo, bool eventsChecking)
        {
            CreateMethodSignature(cinfo);

            File.WriteLine(Indent2 + "string name = \"" + cinfo.DeclaringType + "\";");
            File.WriteLine(Indent2 + "dcObjKey = CQG.CallCtor(name);");

            CtorEnd(eventsChecking);
        }

        static void CreateInternalCtor(string typeName, bool eventsChecking)
        {
            File.WriteLine(Indent1 + "internal " + typeName + "(string dcObjKey)");
            File.WriteLine(Indent1 + "{");

            File.WriteLine(Indent2 + "this.dcObjKey = dcObjKey;");

            CtorEnd(eventsChecking);
        }

        static void CtorEnd(bool eventsChecking)
        {
            if (eventsChecking)
            {
                File.WriteLine(Indent2 + "eventCheckingTimer = new System.Timers.Timer();");
                File.WriteLine(Indent2 + "eventCheckingTimer.Interval = " + EventCheckingTimerInterval.ToString() + ";");
                File.WriteLine(Indent2 + "eventCheckingTimer.Elapsed += eventCheckingTimer_Tick;");
                File.WriteLine(Indent2 + "eventCheckingTimer.AutoReset = false;");
                File.WriteLine(Indent2 + "eventCheckingTimer.Enabled = true;");
            }

            MemberEnd();
        }
    }
}