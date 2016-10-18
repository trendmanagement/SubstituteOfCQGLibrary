using System;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateDtor(string typeName)
        {
            UpdateRegion(RegionType.Destructor);

            File.WriteLine(Indent1 + "~" + typeName + "()");
            File.WriteLine(Indent1 + "{");
            if (typeName == "CQGCELClass")
            {
                File.WriteLine(Indent2 + "if(!isDCClosed)" + Environment.NewLine + Indent2 + "{");
                Indent2 = Indent3;
            }

            File.WriteLine(Indent2 + "Internal.Core.CallDtor(dcObjKey);");

            if (typeName == "CQGCELClass")
            {
                InitIndents();
                File.WriteLine(Indent2 + "}");
            }

            MemberEnd();
        }
    }
}