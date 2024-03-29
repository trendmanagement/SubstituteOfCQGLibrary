﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateFields(FieldInfo[] finfos)
        {
            if (finfos.Length == 0)
            {
                return;
            }

            UpdateRegion(RegionType.Fields);

            foreach (FieldInfo finfo in SortFields(finfos))
            {
                CreateField(finfo);
            }
        }

        static void CreateField(FieldInfo finfo)
        {
            Type fieldType = finfo.FieldType;
            if (!fieldType.IsValueType && !IsDelegate(fieldType))
            {
                throw new NotImplementedException();
            }
            File.Write(Indent1 + "public ");
            if (finfo.IsStatic)
            {
                File.Write("static ");
            }
            File.WriteLine(TypeToString(fieldType) + " " + finfo.Name + ";" + Environment.NewLine);
        }

        static IEnumerable<FieldInfo> SortFields(FieldInfo[] finfos)
        {
            return finfos.OrderBy(finfo => finfo.Name);
        }
    }
}