using System;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateCtors(Type type, bool eventsChecking)
        {
            UpdateRegion(RegionType.Constructors);

            ConstructorInfo[] cinfos = type.GetConstructors();

            // Add public constructors
            foreach (ConstructorInfo cinfo in cinfos)
            {
                CreateCtor(cinfo, eventsChecking);
            }

            if (cinfos.Length == 0 && !type.IsValueType)
            {
                // Add a default internal costructor
                CreateCtor(type.Name, eventsChecking);
            }
        }

        static void CreateCtor(ConstructorInfo cinfo, bool eventsChecking)
        {
            CreateMethodSignature(cinfo);

            File.WriteLine(Indent2 + "thisObjUnqKey = Guid.NewGuid().ToString(\"D\");");
            File.WriteLine(Indent2 + "string name = \"" + cinfo.DeclaringType + "\";");
            File.WriteLine(Indent2 + "string v = (string)CQG.ExecuteTheQuery(QueryInfo.QueryType.Constructor, thisObjUnqKey, name);");

            CtorEnd(eventsChecking);
        }

        static void CreateCtor(string typeName, bool eventsChecking)
        {
            File.WriteLine(Indent1 + "internal " + typeName + "()");
            File.WriteLine(Indent1 + "{");

            File.WriteLine(Indent2 + "thisObjUnqKey = Guid.NewGuid().ToString(\"D\");");

            CtorEnd(eventsChecking);
        }

        static void CtorEnd(bool eventsChecking)
        {
            if (eventsChecking)
            {
                File.WriteLine(Indent2 + "eventCheckingTimer = new System.Timers.Timer();");
                File.WriteLine(Indent2 + "eventCheckingTimer.Interval = 30;");
                File.WriteLine(Indent2 + "eventCheckingTimer.Elapsed += eventCheckingTimer_Tick;");
                File.WriteLine(Indent2 + "eventCheckingTimer.AutoReset = true;");
                File.WriteLine(Indent2 + "eventCheckingTimer.Enabled = true;");
            }

            MemberEnd();
        }
    }
}