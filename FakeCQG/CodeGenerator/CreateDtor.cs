namespace CodeGenerator
{
    partial class Program
    {
        static void CreateDtor(string typeName)
        {
            UpdateRegion(RegionType.Destructor);

            File.WriteLine(Indent1 + "~" + typeName + "()");
            File.WriteLine(Indent1 + "{");
            File.WriteLine(Indent2 + "CQG.PushQueryAsync(new QueryInfo(QueryInfo.QueryType.CallDtor, string.Empty, dcObjKey, null, null));");
            File.WriteLine(Indent2 + "CQG.CallDtor(dcObjKey);");

            MemberEnd();
        }
    }
}