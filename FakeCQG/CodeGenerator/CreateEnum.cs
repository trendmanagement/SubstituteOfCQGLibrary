using System;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateEnum(Type type)
        {
            UpdateRegion(RegionType.Enums);

            File.WriteLine(Indent1 + "public enum " + type.Name);
            File.WriteLine(Indent1 + "{");

            string[] names = Enum.GetNames(type);
            Array values = Enum.GetValues(type);
            for (int i = 0; i < names.Length; i++)
            {
                string comma = (i != names.Length - 1) ? "," : string.Empty;
                File.WriteLine(Indent2 + names[i] + " = " + (int)values.GetValue(i) + comma);
            }

            MemberEnd();
        }
    }
}