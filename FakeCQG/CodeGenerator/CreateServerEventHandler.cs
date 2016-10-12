﻿using System;
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

            ParameterInfo[] pinfos = minfo.GetParameters();

            string args;
            if (pinfos.Length != 0)
            {
                // Write a line of code that collects all the arguments into object[] as following:
                // var args = new object[] { arg1, arg2, ..., argn }
                CreateArgsObjectArray(DCEvHndlrFile, Indent3, pinfos);

                args = ", args";
            }
            else
            {
                args = string.Empty;
            }
            
            DCEvHndlrFile.WriteLine(Indent3 + "EventHandler.CommonEventHandler(name" + args + ");");

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
