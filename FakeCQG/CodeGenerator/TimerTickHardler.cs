using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void EventChecking(EventInfo einfo)
        {
            File.WriteLine(Indent2 + "if(" + einfo.Name + "== null && DataDictionaries.EventCheckingDictionary[\"" + einfo.Name + "\"])" +
                Environment.NewLine + Indent2 + "{");
            File.WriteLine(Indent2 + Indent1 + "bool res = (bool)CQG.ExecuteTheQuery(QueryInfo.QueryType.Event, thisObjUnqKey, \"" +
                einfo.Name + "\", new object[] {\"-\"});");
            File.WriteLine(Indent2 + Indent1 + "DataDictionaries.EventCheckingDictionary[\"" + einfo.Name + "\"] = false;");
            File.WriteLine(Indent2 + "}" + Environment.NewLine + Indent2 + "else if(" + einfo.Name + 
                " != null && !DataDictionaries.EventCheckingDictionary[\"" + einfo.Name + "\"])" +
                Environment.NewLine + Indent2 + "{");
            File.WriteLine(Indent2 + Indent1 + "bool res = (bool)CQG.ExecuteTheQuery(QueryInfo.QueryType.Event, thisObjUnqKey, \"" +
                einfo.Name + "\", new object[] {\"+\"});");
            File.WriteLine(Indent2 + Indent1 + "DataDictionaries.EventCheckingDictionary[\"" + einfo.Name + "\"] = true;");
            File.WriteLine(Indent2 + "}"+ Environment.NewLine);

        }
    }
}
