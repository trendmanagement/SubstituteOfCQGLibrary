﻿namespace CodeGenerator
{
    partial class Program
    {
        static void CreateDtor(string typeName)
        {
            UpdateRegion(RegionType.Destructor);

            File.WriteLine(Indent1 + "~" + typeName + "()");
            File.WriteLine(Indent1 + "{");
            File.WriteLine(Indent2 + "Internal.Core.CallDtor(dcObjKey);");

            MemberEnd();
        }
    }
}