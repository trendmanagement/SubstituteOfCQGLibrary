using System;
using System.Collections.Generic;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateServerEventHandler(MethodInfo minfo, Type type)
        {
            CreateEventHandlerSignature(type.Name, minfo.GetParameters());

            DCEvHndlrFile.WriteLine(Indent2 + "{");
            DCEvHndlrFile.WriteLine(Indent3 + "string name = \"" + type.Name + "\";");

            // Create dictionaries for serializable and non-serializable input arguments
            // and put the arguments into the dictionaries
            ParameterInfo[] pinfos = minfo.GetParameters();

            string args, nonSerParPos;
            if (minfo.GetParameters().Length != 0)
            {
                // Write a line of code that collects all the arguments into object[] as following:
                // var args = new object[] { "!", arg1, arg2, ..., argn }
                CreateArgsObjectArray(DCEvHndlrFile, Indent3, pinfos);

                args = ", args";
            }
            else
            {
                args = string.Empty;
            }
            
            
            bool firstNSPP = true, isNSPP = false;
            foreach (var pinfo in pinfos)
            {
                if (!IsSerializableType(pinfo.ParameterType))
                {
                    if(firstNSPP)
                    {
                        DCEvHndlrFile.Write(Indent3 + "int[] nonSerParPos = new int[] {");
                        firstNSPP = false;
                        isNSPP = true;
                    }
                    else
                    {
                        DCEvHndlrFile.Write(" ,");
                    }
                    DCEvHndlrFile.Write(pinfo.Position);
                }
            }

            if(isNSPP)
            {
                DCEvHndlrFile.WriteLine("};");
                nonSerParPos = ", nonSerParPos";
            }
            else
            {
                nonSerParPos = string.Empty;
            }

            DCEvHndlrFile.WriteLine(Indent3 + "FakeCQG.Helpers.EventHelper fireEvent = new FakeCQG.Helpers.EventHelper(name" + 
                args + nonSerParPos + ");");

            DCEvHndlrFile.WriteLine(Indent2 + "}" + Environment.NewLine);
        }

        static void CreateEventHandlerSignature(string typeName, ParameterInfo[] pinfos)
        {
            DCEvHndlrFile.Write(Indent2 + "public static void " + typeName + "Impl(");
            CreateMethodArguments(pinfos, true);
            DCEvHndlrFile.WriteLine(")");
        }
    }
}
