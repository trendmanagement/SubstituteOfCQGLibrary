using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateClassInterfaceStruct(Type type)
        {
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

            // Add constructors
            foreach (ConstructorInfo cinfo in type.GetConstructors())
            {
                CreateCtor(cinfo);
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
            foreach (MethodInfo minfo in FilterSortMethods(type.GetMethods()))
            {
                CreateMethod(minfo, type.IsInterface, type.IsValueType);
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

        static IEnumerable<MethodInfo> FilterSortMethods(MethodInfo[] minfos)
        {
            return FilterMethods(minfos).OrderBy(minfo => minfo.Name);
        }

        static IEnumerable<MethodInfo> FilterMethods(MethodInfo[] minfos)
        {
            foreach (MethodInfo minfo in minfos)
            {
                foreach (string name in SkippedMethodsNames)
                {
                    if (minfo.Name == name)
                    {
                        goto Label;
                    }
                }
                foreach (string prefix in SkippedMethodsPrefixes)
                {
                    if (minfo.Name.StartsWith(prefix))
                    {
                        goto Label;
                    }
                }
                foreach (var prefixNumArgs in SkippedMethodsPrefixesNumArgs)
                {
                    if (minfo.Name.StartsWith(prefixNumArgs.Item1) && (minfo.GetParameters().Length == prefixNumArgs.Item2))
                    {
                        goto Label;
                    }
                }
                yield return minfo;
            Label:
                ;
            }
        }
    }
}