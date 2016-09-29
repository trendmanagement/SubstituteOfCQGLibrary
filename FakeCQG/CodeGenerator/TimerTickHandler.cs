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

            bool isRef = false;

            foreach (var pinfo in pinfos)
            {
                string paramType = TypeToString(pinfo.ParameterType);
                string arg = "args[" + pinfo.Position + "]";
                

                if (paramType.Substring(paramType.Length - 1, 1) == "&")
                {
                    paramType = paramType.Substring(0, paramType.Length - 1);
                    isRef = true;
                }

                if (!IsSerializableType(pinfo.ParameterType) && paramType != "eReadyStatus")
                { 
                    File.WriteLine(Indent4 + "var arg" + pinfo.Position + " = new " +
                        paramType + "Class((string)args[" + pinfo.Position + "]);");
                    arg = "arg" + pinfo.Position;
                }

                if (isRef)
                {
                    File.WriteLine(Indent4 + paramType + " rArg" + pinfo.Position +
                        " = (" + paramType + ")" + arg + ";");
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
                    string arg = "args[" + pinfo.Position + "]";
                    if (!IsSerializableType(pinfo.ParameterType))
                    {
                        arg = "arg" + pinfo.Position;
                    }

                    if (isRef)
                    {
                        if (pinfo.Position == pinfos.Length - 1)
                        {
                            File.Write("ref rArg" + pinfo.Position + ");" + Environment.NewLine);
                        }
                        else
                        {
                            File.Write("ref rArg" + pinfo.Position + ", ");
                        }
                    }
                    else
                    {
                        if (pinfo.Position == pinfos.Length - 1)
                        {
                            File.Write("(" + paramType + ")" + arg + ");" + Environment.NewLine);
                        }
                        else
                        {
                            File.Write("(" + paramType + ")" + arg + ", ");
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
