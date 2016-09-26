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
                if (SkippedAncestors.Contains(ancType.Name) ||
                    (ancType.IsInterface && CheckInterfaceInheritance(ancTypes, ancType)))
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

            if (type.IsClass)
            {
                File.WriteLine(Indent1 + "private string dcObjKey;" + Environment.NewLine);
            }
            if (eventsChecking)
            {
                File.WriteLine(Indent1 + "private System.Timers.Timer eventCheckingTimer;" + Environment.NewLine);
            }

            if (!type.IsInterface)
            {
                // Add fields
                CreateFields(type.GetFields());

                // Add constructors
                CreateCtors(type, eventsChecking);

                if (!type.IsValueType)
                {
                    // Add destructor
                    CreateDtor(type.Name);
                }
            }

            // Add properies
            CreateProperties(type.GetProperties(), type.IsInterface);

            // Add events
            CreateEvents(type.GetEvents(), type.IsInterface);

            // Add methods
            CreateMethods(type);

            if (eventsChecking)
            {
                UpdateRegion(RegionType.TimerTickHandler);

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

        /// <summary>
        /// Check if any of types "checkedTypes" has direct or indirect inheritance from interface "intType"
        /// </summary>
        static bool CheckInterfaceInheritance(IEnumerable<Type> allTypes, Type intType)
        {
            IEnumerable<Type> checkedTypes = allTypes.Where(type => type != intType);
            return CheckInterfaceEqualityOrInheritance(checkedTypes, intType);
        }

        /// <summary>
        /// Check if any of types "checkedTypes" is equal to or has direct or indirect inheritance from interface "intType"
        /// </summary>
        static bool CheckInterfaceEqualityOrInheritance(IEnumerable<Type> checkedTypes, Type intType)
        {
            foreach (Type checkedType in checkedTypes)
            {
                // Check this type
                if (checkedType == intType)
                {
                    return true;
                }

                // Get its ancestors
                IEnumerable<Type> ancTypes = checkedType.GetInterfaces();
                Type baseType = checkedType.BaseType;
                if (baseType != null)
                {
                    ancTypes = ancTypes.Concat(new[] { baseType });
                }

                // Recursive call
                if (CheckInterfaceEqualityOrInheritance(ancTypes, intType))
                {
                    return true;
                }
            }

            return false;
        }
    }
}