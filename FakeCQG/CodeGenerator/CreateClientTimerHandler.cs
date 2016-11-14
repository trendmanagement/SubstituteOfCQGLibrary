using System;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateClientTimerHandler(EventInfo einfo)
        {
            File.WriteLine(Indent2 + "Internal.Core.SubscriberChecking(\"" + einfo.Name + "\", dcObjKey, " + Environment.NewLine +
                Indent3 + einfo.Name + " != null && !Internal.ClientDictionaries.EventCheckingDictionary[dcObjKey][\"" + einfo.Name + "\"], "
                + Environment.NewLine + Indent3 + einfo.Name + " == null && Internal.ClientDictionaries.EventCheckingDictionary[dcObjKey][\"" + einfo.Name + 
                "\"]);" + Environment.NewLine);

            File.WriteLine(Indent2 + "if (Internal.ClientDictionaries.EventCheckingDictionary[dcObjKey][\"" + einfo.Name + "\"])" +
                Environment.NewLine + Indent2 + "{");
            File.WriteLine(Indent3 + "object[] args;");
            File.WriteLine(Indent3 + "bool happened = Internal.Core.EventHelper.CheckWhetherEventHappened(\"" + einfo.Name + "\", out args);");

            File.WriteLine(Indent3 + "if (happened)");
            File.WriteLine(Indent3 + "{");

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
            File.WriteLine(Indent2 + "}" + Environment.NewLine);
        }
    }
}
