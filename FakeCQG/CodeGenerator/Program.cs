﻿using System;
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

        public static StringBuilder hQPOfCtrosDict = new StringBuilder();
        public static StringBuilder hQPOfGettersDict = new StringBuilder();
        public static StringBuilder hQPOfSettersDict = new StringBuilder();
        public static StringBuilder hQPOfMethodsDict = new StringBuilder();
        public static StringBuilder hQPOfEventsDict = new StringBuilder();

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
                // AutoGen client API part
                ClientAPICodeBeforeTypesCreation();
                // AutoGen client API part


                // Server event handlers part
                ServerEventHandlersAutoGenIntro();
                // Server event handlers part


                // AutoGen query processing part
                QueryProcessingAutoGenIntro();
                // AutoGen query processing part


                // Main generator of code corresponding to each type and its members
                CreateTypes(assm.ExportedTypes);


                // AutoGen query processing part
                QueryProcessingAutoGenOutro();
                // AutoGen query processing part


                // AutoGen client API part
                ClientAPICodeAfterTypesCreation();
                // AutoGen client API part


                // Server event handlers part
                ServerEventHandlersAutoGenOutro();
                // Server event handlers part
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

        static void ClientAPICodeBeforeTypesCreation()
        {
            CreateWarningHeader(File);

            File.WriteLine("using System;");
            File.WriteLine("using System.Collections;");
            File.WriteLine("");
            File.WriteLine("namespace FakeCQG");
            File.WriteLine("{");
        }

        static void ClientAPICodeAfterTypesCreation()
        {
            File.WriteLine("}");
        }

        static void ServerEventHandlersAutoGenIntro()
        {
            CreateWarningHeader(DCEvHndlrFile);

            DCEvHndlrFile.WriteLine("using System;");
            DCEvHndlrFile.WriteLine("using FakeCQG.Internal;");
            DCEvHndlrFile.WriteLine("");
            DCEvHndlrFile.WriteLine("namespace DataCollectionForRealtime");
            DCEvHndlrFile.WriteLine("{");
            DCEvHndlrFile.WriteLine(Indent1 + "class CQGEventHandlers");
            DCEvHndlrFile.WriteLine(Indent1 + "{");
        }

        static void ServerEventHandlersAutoGenOutro()
        {
            DCEvHndlrFile.WriteLine(Indent1 + "}");
            DCEvHndlrFile.WriteLine("}");
        }

        static void QueryProcessingAutoGenIntro()
        {
            CreateWarningHeader(DCQProcFile);

            DCQProcFile.WriteLine("using CQG;");
            DCQProcFile.WriteLine("using FakeCQG.Internal;");
            DCQProcFile.WriteLine("using FakeCQG.Internal.Models;");
            DCQProcFile.WriteLine("using MongoDB.Driver;");
            DCQProcFile.WriteLine("using System;");
            DCQProcFile.WriteLine("using System.Collections.Generic;");
            DCQProcFile.WriteLine("using System.Reflection;");
            DCQProcFile.WriteLine("");
            DCQProcFile.WriteLine("namespace DataCollectionForRealtime");
            DCQProcFile.WriteLine("{");
            DCQProcFile.WriteLine(Indent1 + "partial class QueryHandler");
            DCQProcFile.WriteLine(Indent1 + "{");
            DCQProcFile.WriteLine(Indent2 + "private object qObj;");
            DCQProcFile.WriteLine(Indent2 + "private delegate void MPDel(QueryInfo query, object[] args);" + Environment.NewLine);

            DCQProcFile.WriteLine(Indent2 + "private Dictionary<string, MPDel> hQPOfCtros;" + Environment.NewLine +
                Indent2 + "private Dictionary<string, MPDel> hQPOfGetters;" + Environment.NewLine +
                Indent2 + "private Dictionary<string, MPDel> hQPOfSetters;" + Environment.NewLine +
                Indent2 + "private Dictionary<string, MPDel> hQPOfMethods;" + Environment.NewLine +
                Indent2 + "private Dictionary<string, MPDel> hQPOfEvents;" +
                Environment.NewLine + Environment.NewLine);
        }
        static void QueryProcessingAutoGenOutro()
        {
            // Initialization of methods-processors dictionaries
            DCQProcFile.WriteLine(Indent2 + "public void InitHMethodDict()");
            DCQProcFile.WriteLine(Indent2 + "{");

            // Filling a dictionary by methods that are processing a ctors call queries
            DCQProcFile.WriteLine(Indent3 + "hQPOfCtros = new Dictionary<string, MPDel> (StringComparer.Ordinal)" +
                Environment.NewLine + Indent3 + "{" + Environment.NewLine);
            hQPOfCtrosDict.Append(Indent3 + "};" + Environment.NewLine + Environment.NewLine);
            DCQProcFile.WriteLine(hQPOfCtrosDict.ToString());

            // Filling a dictionary by methods that are processing a getters call queries
            DCQProcFile.WriteLine(Indent3 + "hQPOfGetters = new Dictionary<string, MPDel> (StringComparer.Ordinal)" +
                Environment.NewLine + Indent3 + "{" + Environment.NewLine);
            hQPOfGettersDict.Append(Indent3 + "};" + Environment.NewLine + Environment.NewLine);
            DCQProcFile.WriteLine(hQPOfGettersDict.ToString());

            // Filling a dictionary by methods that are processing a setters call queries
            DCQProcFile.WriteLine(Indent3 + "hQPOfSetters = new Dictionary<string, MPDel> (StringComparer.Ordinal)" +
                Environment.NewLine + Indent3 + "{" + Environment.NewLine);
            hQPOfSettersDict.Append(Indent3 + "};" + Environment.NewLine + Environment.NewLine);
            DCQProcFile.WriteLine(hQPOfSettersDict.ToString());

            // Filling a dictionary by methods that are processing a methods call queries
            DCQProcFile.WriteLine(Indent3 + "hQPOfMethods = new Dictionary<string, MPDel> (StringComparer.Ordinal)" +
                Environment.NewLine + Indent3 + "{" + Environment.NewLine);
            hQPOfMethodsDict.Append(Indent3 + "};" + Environment.NewLine + Environment.NewLine);
            DCQProcFile.WriteLine(hQPOfMethodsDict.ToString());

            // Filling a dictionary by methods that are processing an event subscribing and unsubscribing queries
            DCQProcFile.WriteLine(Indent3 + "hQPOfEvents = new Dictionary<string, MPDel> (StringComparer.Ordinal)" +
                Environment.NewLine + Indent3 + "{" + Environment.NewLine);
            hQPOfEventsDict.Append(Indent3 + "};" + Environment.NewLine + Environment.NewLine);
            DCQProcFile.Write(hQPOfEventsDict.ToString());

            DCQProcFile.WriteLine(Indent2 + "}");

            // Main query processing switch
            DCQProcFile.WriteLine(Indent2 + "public void AutoGenQueryProcessing(QueryInfo query)");
            DCQProcFile.WriteLine(Indent2 + "{");
            DCQProcFile.WriteLine(Indent3 + "object[] args = Core.GetArgsIntoArrayFromTwoDicts(query.ArgKeys, query.ArgValues);");
            DCQProcFile.WriteLine(Indent3 + "switch (query.QueryType)");
            DCQProcFile.WriteLine(Indent3 + "{");

            // Ctors call query processing
            DCQProcFile.WriteLine(Indent4 + "case QueryType.CallCtor:");
            DCQProcFile.WriteLine(Indent5 + "string ctorHndlrName = string.Concat(\"Ctor\", query.ObjectType);");
            DCQProcFile.WriteLine(Indent5 + "if (!hQPOfCtros.ContainsKey(ctorHndlrName)) " + Environment.NewLine +
                Indent5 + "{" + Environment.NewLine +
                Indent6 + "throw new System.ArgumentException(string.Concat(\"Operation \", ctorHndlrName, \" is invalid\"), \"ctor name\");" +
                Environment.NewLine + Indent5 + "}" + Environment.NewLine +
                Indent5 + "hQPOfCtros[ctorHndlrName](query, args); ");
            DCQProcFile.WriteLine(Indent5 + "break;" + Environment.NewLine);

            // Dtors call query processing
            DCQProcFile.WriteLine(Indent4 + "case QueryType.CallDtor:");
            DCQProcFile.WriteLine(Indent5 + "if (query.ObjectKey != CqgDataManagement.CEL_key) " + Environment.NewLine + Indent5 +
                "{" + Environment.NewLine + Indent6 + "ServerDictionaries.RemoveObjectFromTheDictionary(query.ObjectKey);" +
                    Environment.NewLine + Indent5 + "}");
            DCQProcFile.WriteLine(Indent5 + "break;" + Environment.NewLine);

            // Getters call query processing
            DCQProcFile.WriteLine(Indent4 + "case QueryType.GetProperty:");
            DCQProcFile.WriteLine(Indent5 + "qObj = ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);");
            DCQProcFile.WriteLine(Indent5 + "string getHndlrName = string.Concat(\"Get\", query.ObjectType, query.MemberName);");
            DCQProcFile.WriteLine(Indent5 + "if (!hQPOfGetters.ContainsKey(getHndlrName)) " + Environment.NewLine +
                Indent5 + "{" + Environment.NewLine +
                Indent6 + "throw new System.ArgumentException(string.Concat(\"Operation \", getHndlrName, \" is invalid\"), \"getter name\");" +
                Environment.NewLine + Indent5 + "}" + Environment.NewLine +
                Indent5 + "hQPOfGetters[getHndlrName](query, args); ");
            DCQProcFile.WriteLine(Indent5 + "break;" + Environment.NewLine);

            // Setters call query processing
            DCQProcFile.WriteLine(Indent4 + "case QueryType.SetProperty:");
            DCQProcFile.WriteLine(Indent5 + "qObj = ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);");
            DCQProcFile.WriteLine(Indent5 + "string setHndlrName = string.Concat(\"Set\", query.ObjectType, query.MemberName);");
            DCQProcFile.WriteLine(Indent5 + "if (!hQPOfSetters.ContainsKey(setHndlrName)) " + Environment.NewLine +
                Indent5 + "{" + Environment.NewLine +
                Indent6 + "throw new System.ArgumentException(string.Concat(\"Operation \", setHndlrName, \" is invalid\"), \"setter name\");" +
                Environment.NewLine + Indent5 + "}" + Environment.NewLine +
                Indent5 + "hQPOfSetters[setHndlrName](query, args); ");
            DCQProcFile.WriteLine(Indent5 + "break;" + Environment.NewLine);

            // Methods call query processing
            DCQProcFile.WriteLine(Indent4 + "case QueryType.CallMethod:");
            DCQProcFile.WriteLine(Indent5 + "qObj = ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);");
            DCQProcFile.WriteLine(Indent5 + "string mthdHndlrName = string.Concat(\"Method\", query.ObjectType, query.MemberName);");
            DCQProcFile.WriteLine(Indent5 + "if (!hQPOfMethods.ContainsKey(mthdHndlrName)) " + Environment.NewLine +
                Indent5 + "{" + Environment.NewLine +
                Indent6 + "throw new System.ArgumentException(string.Concat(\"Operation \", mthdHndlrName, \" is invalid\"), \"method name\");" +
                Environment.NewLine + Indent5 + "}" + Environment.NewLine +
                Indent5 + "hQPOfMethods[mthdHndlrName](query, args); ");
            DCQProcFile.WriteLine(Indent5 + "break;" + Environment.NewLine);

            // Subscribing on and unsubscribing from events query processing
            DCQProcFile.WriteLine(Indent4 + "case QueryType.SubscribeToEvent:");
            DCQProcFile.WriteLine(Indent4 + "case QueryType.UnsubscribeFromEvent:");
            DCQProcFile.WriteLine(Indent5 + "qObj = ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);");
            DCQProcFile.WriteLine(Indent5 + "string eventHndlrName = string.Concat(\"Event\", query.ObjectType, query.MemberName);");
            DCQProcFile.WriteLine(Indent5 + "if (!hQPOfEvents.ContainsKey(eventHndlrName)) " + Environment.NewLine + Indent6 +
                Indent5 + "{" + Environment.NewLine +
                Indent6 + "throw new System.ArgumentException(string.Concat(\"Operation \", eventHndlrName, \" is invalid\"), \"event name\");" +
                Environment.NewLine + Indent5 + "}" + Environment.NewLine +
                Indent5 + "if (EventHandler.EventAppsSubscribersNum.ContainsKey(query.MemberName))" +
                    Environment.NewLine + Indent5 + "{" + Environment.NewLine +
                    Indent6 + "EventHandler.EventAppsSubscribersNum[query.MemberName] =" +
                    Environment.NewLine + Indent7 + "query.QueryType == QueryType.SubscribeToEvent ?" +
                    Environment.NewLine + Indent7 + "EventHandler.EventAppsSubscribersNum[query.MemberName] + 1 :" +
                    Environment.NewLine + Indent7 + "EventHandler.EventAppsSubscribersNum[query.MemberName] - 1;" +
                    Environment.NewLine + Indent5 + "}" + Environment.NewLine + Indent5 + "else" +
                    Environment.NewLine + Indent5 + "{" +
                    Environment.NewLine + Indent6 + "EventHandler.EventAppsSubscribersNum.Add(query.MemberName, 1);" +
                    Environment.NewLine + Indent5 + "}" + Environment.NewLine +
                Indent5 + "hQPOfEvents[eventHndlrName](query, args); ");
            DCQProcFile.WriteLine(Indent5 + "break;" + Environment.NewLine);

            DCQProcFile.WriteLine(Indent3 + "}");
            DCQProcFile.WriteLine(Indent2 + "}" + Environment.NewLine);

            // Methods-processors
            DCQProcFile.Write(ctorCall.ToString());
            DCQProcFile.Write(getProp.ToString());
            DCQProcFile.Write(setProp.ToString());
            DCQProcFile.Write(methodCall.ToString());
            DCQProcFile.Write(subAndUnsubEvents.ToString());

            // Ending
            DCQProcFile.WriteLine(Indent1 + "}");
            DCQProcFile.WriteLine("}");
        }
    }
}
