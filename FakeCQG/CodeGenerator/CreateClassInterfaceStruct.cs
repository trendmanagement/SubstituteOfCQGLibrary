using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static HashSet<string> SkippedAncestors = new HashSet<string>() { "Object", "ValueType", "__ComObject" };

        static void CreateClassInterfaceStruct(Type type)
        {
            bool eventsChecking = type.GetEvents().Length != 0 && type.IsClass;

            // Add signature
            string keyword = null;
            if (type.IsInterface)
            {
                UpdateRegion(RegionType.Interfaces);
                keyword = "interface";
            }
            else if (type.IsClass)
            {
                UpdateRegion(RegionType.Classes);
                keyword = "class";
            }
            else if (type.IsValueType)
            {
                UpdateRegion(RegionType.Structs);
                keyword = "struct";
            }
            else
            {
                throw new Exception();
            }

            string typeName = type.Name;

            File.Write(Indent1 + "public " + keyword + " " + typeName);

            // Add inheritance
            bool first = true;
            IEnumerable<Type> ancTypes = type.GetInterfaces();
            Type baseType = type.BaseType;
            if (baseType != null)
            {
                ancTypes = ancTypes.Concat(new[] { baseType });
            }
            foreach (Type ancType in ancTypes)
            {
                if (SkippedAncestors.Contains(ancType.Name))
                {
                    continue;
                }
                if (first)
                {
                    File.Write(" : ");
                    first = false;
                }
                else
                {
                    File.Write(", ");
                }
                File.Write(ancType.Name);
            }

            File.WriteLine(Environment.NewLine + Indent1 + "{");

            IncreaseIndent();

            if (type.IsClass || type.IsValueType)
            {
                File.WriteLine(Indent1 + "private string thisObjUnqKey;" + Environment.NewLine);                
            }
            if (eventsChecking)
            {
                File.WriteLine(Indent1 + "private System.Timers.Timer eventCheckingTimer;" + Environment.NewLine);
            }

            if (!type.IsInterface)
            {
                // Add constructors
                CreateCtors(type, eventsChecking);
            }

            if (!type.IsInterface && !type.IsValueType)
            {
                // Add destructor
                CreateDtor(type.Name);
            }

            // Add properies
            CreateProperties(type.GetProperties(), type.IsInterface);

            // Add events
            foreach (EventInfo einfo in SortEvents(type.GetEvents()))
            {
                CreateEvent(einfo, type.IsInterface);
            }

            // Add methods
            CreateMethods(type);

            if (eventsChecking)
            {
                UpdateRegion(RegionType.TimerTickHardler);
                File.WriteLine(Indent1 + "private void eventCheckingTimer_Tick(Object source, System.Timers.ElapsedEventArgs e)" +
                    Environment.NewLine + Indent1 + "{");

                foreach (EventInfo einfo in SortEvents(type.GetEvents()))
                {
                    EventChecking(einfo);
                }

                MemberEnd();
            }

            // Add nested types
            CreateTypes(type.GetNestedTypes());

            DecreaseIndent();

            File.WriteLine(Indent1 + "}" + Environment.NewLine);
        }

        static IEnumerable<EventInfo> SortEvents(EventInfo[] einfos)
        {
            return einfos.OrderBy(einfo => einfo.EventHandlerType.Name);
        }
    }
}