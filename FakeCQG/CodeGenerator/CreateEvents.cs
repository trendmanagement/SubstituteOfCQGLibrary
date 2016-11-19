using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    partial class Program
    {
        // All generated implementations of methods, where the subscribing and unsubscribing are placed, for each event 
        public static StringBuilder subAndUnsubEvents = new StringBuilder();

        private static HashSet<string> eventsThatSkippedInQPGen = new HashSet<string>() {
            "_ICQGCELEvents_EventIsReady",
            "_ICQGCELGeneralEvents_EventIsReady",
            "CQGCELIsReady",
            "CQGDirectEventsAccessorCELStarted",
            "CQGDirectEventsAccessorCurrencyRatesChanged",
            "CQGDirectEventsAccessorDataConnectionStatusChanged",
            "CQGDirectEventsAccessorDataError",
            "CQGDirectEventsAccessorGWConnectionStatusChanged",
            "CQGDirectEventsAccessorIsReady",
            "CQGDirectEventsAccessorLineTimeChanged",
            "CQGDirectEventsAccessorOnIdle" };

        static void CreateEvents(EventInfo[] einfos, bool isInterface)
        {
            foreach (EventInfo einfo in SortEvents(einfos))
            {
                CreateEvent(einfo, isInterface);
            }
        }

        static void CreateEvent(EventInfo einfo, bool isInterface)
        {
            UpdateRegion(RegionType.Events);

            string public_ = !isInterface ? "public " : string.Empty;
            File.WriteLine(Indent1 + public_ + "event " + einfo.EventHandlerType.Name + " " + einfo.Name + ";" + Environment.NewLine);

            string iTypeName = einfo.DeclaringType.Name;
            if (iTypeName.EndsWith("Class"))
            {
                iTypeName = iTypeName.Substring(0, iTypeName.Length - 5);
            }

            if(!eventsThatSkippedInQPGen.Contains(string.Concat(iTypeName, einfo.Name)))
            {
                AddOrRemoveEventHandler(einfo, einfo.DeclaringType, iTypeName);
            }        
        }

        static IEnumerable<EventInfo> SortEvents(EventInfo[] einfos)
        {
            return einfos.OrderBy(einfo => einfo.EventHandlerType.Name);
        }

        // Events subscribe and unsubscribe queries processing
        static void AddOrRemoveEventHandler(EventInfo einfo, Type type, string iTypeName)
        {
            subAndUnsubEvents.Append(Indent2 + "private void Event" + type.Name + einfo.Name + "(QueryInfo query, object[] args)" +
                        Environment.NewLine + Indent2 + "{" + Environment.NewLine);

            hQPOfEventsDict.Append(Indent4 + "{ \"Event" + type.Name + einfo.Name + "\", this.Event" + type.Name + einfo.Name + "}," +
                        Environment.NewLine);

            subAndUnsubEvents.Append(Indent3 + iTypeName + " " + einfo.Name + "Obj = (" + iTypeName +
                            ")qObj;" + Environment.NewLine);
            
            subAndUnsubEvents.Append(Indent3 + "if (EventHandler.EventAppsSubscribersNum.ContainsKey(query.MemberName))" +
                        Environment.NewLine + Indent3 + "{" + Environment.NewLine +
                        Indent4 + "EventHandler.EventAppsSubscribersNum[query.MemberName] =" +
                        Environment.NewLine + Indent4 + "query.QueryType == QueryType.SubscribeToEvent ?" +
                        Environment.NewLine + Indent4 + "EventHandler.EventAppsSubscribersNum[query.MemberName] + 1 :" +
                        Environment.NewLine + Indent4 + "EventHandler.EventAppsSubscribersNum[query.MemberName] - 1;" +
                        Environment.NewLine + Indent3 + "}" + Environment.NewLine + Indent3 + "else" +
                        Environment.NewLine + Indent3 + "{" +
                        Environment.NewLine + Indent4 + "EventHandler.EventAppsSubscribersNum.Add(query.MemberName, 1);" +
                        Environment.NewLine + Indent3 + "}" + Environment.NewLine);


            subAndUnsubEvents.Append(Indent3 + "if (query.QueryType == QueryType.SubscribeToEvent)" +
                        Environment.NewLine + Indent3 + "{" + Environment.NewLine);
            if (einfo.Name == "DataConnectionStatusChanged" && (type.Name == "_ICQGCELEvents" || type.Name == "CQGCELClass"))
            {
                subAndUnsubEvents.Append(Indent4 + einfo.Name + "Obj." + einfo.Name + " += new " + einfo.EventHandlerType +
                        "(CQGEventHandlers." + einfo.EventHandlerType.Name + "Impl);" + Environment.NewLine +
                        Environment.NewLine);

                subAndUnsubEvents.Append(Indent4 + 
                    "PushAnswerAndDeleteQuery(new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, value: true));" + 
                    Environment.NewLine + Environment.NewLine);

                subAndUnsubEvents.Append(Indent4 + 
                    "// Fire this event explicitly, because data collector connects to real CQG beforehand and does not fire it anymore" +
                        Environment.NewLine + Indent4 +
                        "CQGEventHandlers._ICQGCELEvents_DataConnectionStatusChangedEventHandlerImpl(CqgDataManagement.currConnStat);" +
                        Environment.NewLine);
            }
            else
            {
                subAndUnsubEvents.Append(Indent4 + einfo.Name + "Obj." + einfo.Name + " += new " + einfo.EventHandlerType +
                        "(CQGEventHandlers." + einfo.EventHandlerType.Name + "Impl);" + Environment.NewLine +
                        Environment.NewLine);
                subAndUnsubEvents.Append(Indent4 +
                    "PushAnswerAndDeleteQuery(new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, value: true));" +
                    Environment.NewLine);
            }   
            subAndUnsubEvents.Append(Indent3 + "}" + Environment.NewLine + Indent3 + "else" +
                        Environment.NewLine + Indent3 + "{" +
                        Environment.NewLine + Indent4 + einfo.Name + "Obj." + einfo.Name + " += new " + einfo.EventHandlerType +
                        "(CQGEventHandlers." + einfo.EventHandlerType.Name + "Impl);" + Environment.NewLine +
                        Indent3 + "}" + Environment.NewLine);

            subAndUnsubEvents.Append(Indent2 + "}" + Environment.NewLine);
        }
    }
}