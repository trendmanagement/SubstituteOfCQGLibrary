using System;
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
            bool serArgsFound, nonSerArgsFound;
            CreateAndPopulateDictionaries(pinfos, out serArgsFound, out nonSerArgsFound);

            // Call the utility method
            DCEvHndlrFile.Write(Indent3 + "FakeCQG.CQG.AnswerHelper.CommonEventHandler(name");
            if (serArgsFound)
            {
                DCEvHndlrFile.Write(", serArgs: serArgs");
            }
            if (nonSerArgsFound)
            {
                DCEvHndlrFile.Write(", nonSerArgs: nonSerArgs");
            }
            DCEvHndlrFile.WriteLine(");");

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
