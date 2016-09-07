using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateCtor(ConstructorInfo cinfo)
        {
            UpdateRegion(RegionType.Constructors);

            CreateMethodSignature(cinfo);

            File.WriteLine(Indent2 + "thisObjUnqKey = Guid.NewGuid().ToString(\"D\");");
            File.WriteLine(Indent2 + "string name = \"" + cinfo.DeclaringType + "\";");
            File.WriteLine(Indent2 + "string v = (string)CQG.ExecuteTheQuery(QueryInfo.QueryType.Constructor, thisObjUnqKey, name);");

            MemberEnd();
        }
    }
}