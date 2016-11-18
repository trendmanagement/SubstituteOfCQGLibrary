using System;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    partial class Program
    {
        const int EventCheckingTimerInterval = 30;  // ms

        // All generated implementations of methods calling for each type
        public static StringBuilder ctorCall = new StringBuilder();

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
            File.WriteLine(Indent2 + "this.dcObjType = \"" + cinfo.DeclaringType.Name + "\";");
            File.WriteLine(Indent2 + "dcObjKey = Internal.Core.CallCtor(name, this.dcObjType);");

            CtorEnd(eventsChecking);



            var argins = cinfo.GetParameters();

            ctorCall.Append(Indent2 + "private void Ctor" + cinfo.DeclaringType.Name + "(QueryInfo query, object[] args)" +
                Environment.NewLine + Indent2 + "{" + Environment.NewLine);

            hQPOfCtrosDict.Append(Indent4 + "{ \"Ctor" + cinfo.DeclaringType.Name + "\", this.Ctor" + cinfo.DeclaringType.Name + "}," +
                Environment.NewLine);

            if (cinfo.DeclaringType.Name == "CQGCELClass")
            {
                ctorCall.Append(Indent3 + "string key = CqgDataManagement.CEL_key;" + Environment.NewLine);
            }
            else
            {
                if (argins.Length > 0)
                {
                    for (int i = 0; i < argins.Length; i++)
                    {
                        if (argins[i].ParameterType.FullName.EndsWith("&"))
                        {
                            ctorCall.Append(Indent3 + "var arg" + i + " = (" +
                                argins[i].ParameterType.FullName.Substring(0, argins[i].ParameterType.FullName.Length - 1) +
                                ")args[" + i + "];" + Environment.NewLine);
                        }
                    }
                }

                ctorCall.Append(Indent3 + cinfo.DeclaringType.Name + " " + cinfo.DeclaringType.Name + "Obj = new " + cinfo.DeclaringType.Name + "(");

                if (argins.Length > 0)
                {
                    for (int i = 0; i < argins.Length; i++)
                    {
                        if (argins[i].ParameterType.FullName.EndsWith("&"))
                        {
                            ctorCall.Append("out arg" + i);
                        }
                        else
                        {
                            ctorCall.Append("(" + argins[i].ParameterType + ")args[" + i + "]");
                        }

                        ctorCall.Append(i == argins.Length - 1 ? ");" : ",");
                    }
                }
                else
                {
                    ctorCall.Append(");" + Environment.NewLine);
                }

                ctorCall.Append(Indent3 + "string key = Core.CreateUniqueKey();");

                // Get name of an instrument if it's CQG.CQGInstrument object creation
                // and show it in MiniMonitor form
                if (cinfo.DeclaringType.Name == "CQGInstrument")
                {
                    ctorCall.Append(Indent3 + "string instrName = " + cinfo.DeclaringType.Name + "Obj.FullName;" + Environment.NewLine +
                    Indent3 + "DCMiniMonitor.instrumentsList.Add(instrName);" + Environment.NewLine +
                    Indent3 + "Program.MiniMonitor.SymbolsAndInstrumentsListsUpdate();");
                }

                ctorCall.Append(Indent3 + "ServerDictionaries.PutObjectToTheDictionary(key, qObj);" + Environment.NewLine);   
            }

            ctorCall.Append(Indent3 +
                        "PushAnswerAndDeleteQuery(new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: key));" +
                        Environment.NewLine);

            ctorCall.Append(Indent2 + "}" + Environment.NewLine + Environment.NewLine);
        }

        static void CreateInternalCtor(string typeName, bool eventsChecking)
        {
            File.WriteLine(Indent1 + "internal " + typeName + "(string dcObjKey)");
            File.WriteLine(Indent1 + "{");

            File.WriteLine(Indent2 + "this.dcObjKey = dcObjKey;");
            File.WriteLine(Indent2 + "this.dcObjType = \"" + typeName + "\";");

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