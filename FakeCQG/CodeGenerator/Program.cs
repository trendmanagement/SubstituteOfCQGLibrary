using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    partial class Program
    {
        static StreamWriter File;
        static StreamWriter DCEvHndlrFile;
        static StreamWriter DCQProcFile;

        public static StringBuilder hMethodsDict = new StringBuilder();

        // Set this to False to get type names in short form, e.g. "int" instead of "Int32"
        static bool QuickTestMode = true;

        static void Main(string[] args)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string assmPath = Path.Combine(path, "Interop.CQG.dll");
            var assm = Assembly.LoadFrom(assmPath);

            string outPath = Path.GetFullPath(path + @"..\..\..\..\FakeCQG\AutoGenClientAPI.cs");
            string dcEHOutPath = Path.GetFullPath(path + @"..\..\..\..\..\DataCollectionForRealtime\DataCollectionForRealtime\AutoGenEventHandlers.cs");
            string dcQPOutPath = Path.GetFullPath(path + @"..\..\..\..\..\DataCollectionForRealtime\DataCollectionForRealtime\AutoGenQueryProcessing.cs");

            CurrentIndent = 1;
            InitIndents();

            using (File = new StreamWriter(outPath))
            using (DCEvHndlrFile = new StreamWriter(dcEHOutPath))
            using (DCQProcFile = new StreamWriter(dcQPOutPath))
            {
                CreateWarningHeader(File);

                File.WriteLine("using System;");
                File.WriteLine("using System.Collections;");
                File.WriteLine("");
                File.WriteLine("namespace FakeCQG");
                File.WriteLine("{");

                CreateWarningHeader(DCEvHndlrFile);

                DCEvHndlrFile.WriteLine("using System;");
                DCEvHndlrFile.WriteLine("using FakeCQG.Internal;");
                DCEvHndlrFile.WriteLine("");
                DCEvHndlrFile.WriteLine("namespace DataCollectionForRealtime");
                DCEvHndlrFile.WriteLine("{");
                DCEvHndlrFile.WriteLine(Indent1 + "class CQGEventHandlers");
                DCEvHndlrFile.WriteLine(Indent1 + "{");

                CreateWarningHeader(DCQProcFile);

                DCQProcFile.WriteLine("using CQG;");
                DCQProcFile.WriteLine("using FakeCQG.Internal;");
                DCQProcFile.WriteLine("using FakeCQG.Internal.Models;");
                DCQProcFile.WriteLine("using MongoDB.Driver;"); 
                DCQProcFile.WriteLine("using System.Collections.Generic;");
                DCQProcFile.WriteLine("using System.Reflection;");
                DCQProcFile.WriteLine("");
                DCQProcFile.WriteLine("namespace DataCollectionForRealtime");
                DCQProcFile.WriteLine("{");
                DCQProcFile.WriteLine(Indent1 + "partial class QueryHandler");
                DCQProcFile.WriteLine(Indent1 + "{");
                DCQProcFile.WriteLine(Indent2 + "private delegate void PropDel(QueryInfo query, object[] args);" + Environment.NewLine);
                DCQProcFile.WriteLine(Indent2 + "private Dictionary<string, PropDel> hMethods; " + 
                    Environment.NewLine  + Environment.NewLine);

                CreateTypes(assm.ExportedTypes);

                DCQProcFile.WriteLine(Indent2 + "public void InitHMethodDict()");
                DCQProcFile.WriteLine(Indent2 + "{");
                DCQProcFile.WriteLine(Indent3 + "hMethods = new Dictionary<string, PropDel> " + 
                    Environment.NewLine + Indent3 + "{" + Environment.NewLine);
                hMethodsDict.Append(Indent3 + "};" + Environment.NewLine + Environment.NewLine);
                DCQProcFile.Write(hMethodsDict.ToString());
                DCQProcFile.WriteLine(Indent2 + "}");
                      
                DCQProcFile.WriteLine(Indent2 + "public void AutoGenQueryProcessing(QueryInfo query)");
                DCQProcFile.WriteLine(Indent2 + "{");
                DCQProcFile.WriteLine(Indent3 + "object qObj = ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);");
                DCQProcFile.WriteLine(Indent3 + "object[] args = Core.ParseInputArgsFromQueryInfo(query);");
                DCQProcFile.WriteLine(Indent3 + "switch (query.QueryType)");
                DCQProcFile.WriteLine(Indent3 + "{"); 

                

                File.WriteLine("}");

                DCEvHndlrFile.WriteLine(Indent1 + "}");
                DCEvHndlrFile.WriteLine("}");

                DCQProcFile.WriteLine(Indent4 + "case QueryType.GetProperty:");
                DCQProcFile.WriteLine(Indent5 + "string getHndlrName = string.Concat(\"Get\", query.ObjectType, query.MemberName);");
                DCQProcFile.WriteLine(Indent5 + "if (!hMethods.ContainsKey(getHndlrName)) " + Environment.NewLine + Indent6 +
                    "throw new System.ArgumentException(string.Concat(\"Operation \", getHndlrName, \" is invalid\"), \"getter name\");" + 
                    Environment.NewLine + Indent5 + "hMethods[getHndlrName](query, args); ");
                DCQProcFile.WriteLine(Indent5 + "break;");
                
                DCQProcFile.WriteLine(Indent4 + "case QueryType.SetProperty:");
                DCQProcFile.WriteLine(Indent5 + "string setHndlrName = string.Concat(\"Set\", query.ObjectType, query.MemberName);");
                DCQProcFile.WriteLine(Indent5 + "if (!hMethods.ContainsKey(setHndlrName)) " + Environment.NewLine + Indent6 +
                    "throw new System.ArgumentException(string.Concat(\"Operation \", setHndlrName, \" is invalid\"), \"setter name\");" + 
                    Environment.NewLine + Indent5 + "hMethods[setHndlrName](query, args); ");
                DCQProcFile.WriteLine(Indent5 + "break;");

                DCQProcFile.WriteLine(Indent3 + "}");
                DCQProcFile.WriteLine(Indent2 + "}" + Environment.NewLine);

                DCQProcFile.Write(getProp.ToString());
                DCQProcFile.Write(setProp.ToString());

                DCQProcFile.WriteLine(Indent1 + "}");
                DCQProcFile.WriteLine("}");
            }
        }

        static void CreateWarningHeader(StreamWriter file)
        {
            file.WriteLine("// WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING");
            file.WriteLine("// WARNING                                                                         WARNING");
            file.WriteLine("// WARNING    THIS .CS FILE WAS AUTOGENERATED!                                     WARNING");
            file.WriteLine("// WARNING    DO NOT EDIT IT BY HAND, BECAUSE ALL YOUR CHANGES WILL BE LOST!       WARNING");
            file.WriteLine("// WARNING    MAKE ALL CHANGES DIRECTLY TO THE FILE GENERATOR CODE!                WARNING");
            file.WriteLine("// WARNING                                                                         WARNING");
            file.WriteLine("// WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING");
            file.WriteLine("");
            file.WriteLine("// Disable two warnings caused by CQG API specific:");
            file.WriteLine("// CS3003: Type of 'variable' is not CLS-compliant");
            file.WriteLine("// CS3008: Identifier 'identifier' differing only in case is not CLS-compliant");
            file.WriteLine("#pragma warning disable 3003, 3008");
            file.WriteLine("");
        }
    }
}
