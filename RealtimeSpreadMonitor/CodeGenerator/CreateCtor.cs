using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateCtor(ConstructorInfo cinfo, bool eventsChecking)
        {
            UpdateRegion(RegionType.Constructors);

            CreateMethodSignature(cinfo);

            File.WriteLine(Indent2 + "thisObjUnqKey = Guid.NewGuid().ToString(\"D\");");
            File.WriteLine(Indent2 + "string name = \"" + cinfo.DeclaringType + "\";");
            File.WriteLine(Indent2 + "string v = (string)CQG.ExecuteTheQuery(QueryInfo.QueryType.Constructor, thisObjUnqKey, name);");

            if(eventsChecking)
            {
                File.WriteLine(Indent2 + "eventCheckingTimer = new System.Timers.Timer();");
                File.WriteLine(Indent2 + "eventCheckingTimer.Interval = 30;");
                File.WriteLine(Indent2 + "eventCheckingTimer.Elapsed += eventCheckingTimer_Tick;");
                File.WriteLine(Indent2 + "eventCheckingTimer.AutoReset = true;");
                File.WriteLine(Indent2 + "eventCheckingTimer.Enabled = true");
            }

            MemberEnd();
        }
    }
}