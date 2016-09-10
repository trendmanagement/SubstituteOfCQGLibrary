using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateTypes(IEnumerable<Type> types)
        {
            foreach (Type type in FilterSortDifferentTypes(types))
            {
                CreateType(type);
            }

            UpdateRegion(RegionType.None);
        }

        static void CreateType(Type type)
        {
            if (type.IsEnum)
            {
                CreateEnum(type);
            }
            else if (IsDelegate(type))
            {
                CreateDelegate(type);
            }
            else
            {
                CreateClassInterfaceStruct(type);
            }
        }

        static IEnumerable<Type> FilterSortDifferentTypes(IEnumerable<Type> types)
        {
            return
                SortIdenticalTypes(types.Where(type => type.IsEnum)).Concat(                        // enums
                SortIdenticalTypes(types.Where(type => IsDelegate(type)))).Concat(                  // delegates
                SortIdenticalTypes(types.Where(type => type.IsInterface))).Concat(                  // interfaces
                SortIdenticalTypes(types.Where(type => type.IsClass && !IsDelegate(type)))).Concat( // classes
                SortIdenticalTypes(types.Where(type => type.IsValueType && !type.IsEnum)));         // structs
        }

        static IEnumerable<Type> SortIdenticalTypes(IEnumerable<Type> types)
        {
            return types.OrderBy(type => type.Name);
        }

        static bool IsDelegate(Type type)
        {
            return type.BaseType == typeof(MulticastDelegate);
        }
    }
}