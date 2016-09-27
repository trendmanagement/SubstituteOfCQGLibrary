using System;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void EventChecking(EventInfo einfo)
        {
            File.WriteLine(Indent2 + "CQG.SubscriberChecking(\"" + einfo.Name + "\", dcObjKey, " + Environment.NewLine +
                Indent3 + einfo.Name + " != null && !DataDictionaries.EventCheckingDictionary[dcObjKey][\"" + einfo.Name + "\"], "
                + Environment.NewLine + Indent3 + einfo.Name + " == null && DataDictionaries.EventCheckingDictionary[dcObjKey][\"" + einfo.Name + 
                "\"]);" + Environment.NewLine);

            File.WriteLine(Indent2 + "if (DataDictionaries.EventCheckingDictionary[dcObjKey][\"" + einfo.Name + "\"] == true)" +
                Environment.NewLine + Indent2 + "{");
            File.WriteLine(Indent3 + "try" + Environment.NewLine + Indent3 + "{");
            File.WriteLine(Indent4 + "object[] args = CQG.AnswerHelper.CheckWhetherEventHappened(\"" + einfo.Name + "\");");

            ParameterInfo[] pinfos = einfo.EventHandlerType.GetMethod("Invoke").GetParameters();

            foreach (var pinfo in pinfos)
            {
                string paramType = TypeToString(pinfo.ParameterType);
                if (paramType.Substring(paramType.Length - 1, 1) == "&")
                {
                    File.WriteLine(Indent4 + paramType.Substring(0, paramType.Length - 1) + " arg" + pinfo.Position +
                        " = (" + paramType.Substring(0, paramType.Length - 1) + ")args[" + pinfo.Position + "];");
                }
            }

            File.Write(Indent4 + einfo.Name + ".Invoke(");

            if (pinfos.Length == 0)
            {
                File.Write(");" + Environment.NewLine);
            }
            else
            {
                foreach (var pinfo in pinfos)
                {
                    string paramType = TypeToString(pinfo.ParameterType);
                    if (paramType.Substring(paramType.Length - 1, 1) == "&")
                    {
                        if (pinfo.Position == pinfos.Length - 1)
                        {
                            File.Write("ref arg" + pinfo.Position + ");" + Environment.NewLine);
                        }
                        else
                        {
                            File.Write("ref arg" + pinfo.Position + ", ");
                        }
                    }
                    else
                    {
                        if (pinfo.Position == pinfos.Length - 1)
                        {
                            File.Write("(" + paramType + ")args[" + pinfo.Position + "]);" + Environment.NewLine);
                        }
                        else
                        {
                            File.Write("(" + paramType + ")args[" + pinfo.Position + "], ");
                        }
                    }      
                }
            }
               
            File.WriteLine(Indent3 + "}");
            File.WriteLine(Indent3 + "catch (Exception)" + Environment.NewLine + Indent3 + "{ }");
            File.WriteLine(Indent2 + "}" + Environment.NewLine);
        }
    }
}
