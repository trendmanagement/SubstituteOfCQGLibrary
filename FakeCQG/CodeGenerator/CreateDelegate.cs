using System;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateDelegate(Type type)
        {
            UpdateRegion(RegionType.Delegates);

            MethodInfo minfo = type.GetMethod("Invoke");
            CreateMethodSignature(minfo, TypeToString(minfo.ReturnType), type.Name);
            CreateServerEventHandler(minfo, type);
            File.WriteLine("");
        }
    }
}