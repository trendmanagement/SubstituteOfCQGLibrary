using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator
{
    partial class Program
    {
        const string Indent = "    ";

        static string Indent1;
        static string Indent2;
        static string Indent3;
        static string Indent4;
        static int CurrentIndent;

        enum RegionType
        {
            None,
            Enums,
            Interfaces,
            Classes,
            Structs,
            Constructors,
            Destructor,
            Fields,
            Properties,
            Delegates,
            Events,
            Methods,
            TimerTickHandlers
        }

        static List<RegionType> CurrentRegionTypes = new List<RegionType> { RegionType.None };

        static void InitIndents()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < CurrentIndent; i++)
            {
                sb.Append(Indent);
            }
            Indent1 = sb.ToString();
            sb.Append(Indent);
            Indent2 = sb.ToString();
            sb.Append(Indent);
            Indent3 = sb.ToString();
            sb.Append(Indent);
            Indent4 = sb.ToString();
        }

        static void IncreaseIndent()
        {
            CurrentIndent++;
            InitIndents();
        }

        static void DecreaseIndent()
        {
            CurrentIndent--;
            InitIndents();
        }

        static void MemberEnd()
        {
            File.WriteLine(Indent1 + "}" + Environment.NewLine);
        }

        static void UpdateRegion(RegionType regionType)
        {
            int n = CurrentRegionTypes.Count;
            RegionType lastRegionType = CurrentRegionTypes[n - 1];

            if (regionType == lastRegionType)
            {
                return;
            }
            if (lastRegionType != RegionType.None && CurrentIndent != n)
            {
                File.WriteLine(Indent1 + "#endregion" + Environment.NewLine);
                CurrentRegionTypes.RemoveAt(n - 1);
            }
            if (regionType != RegionType.None)
            {
                File.WriteLine(Indent1 + "#region " + regionType.ToString() + Environment.NewLine);
                CurrentRegionTypes.Add(regionType);
            }
        }
    }
}