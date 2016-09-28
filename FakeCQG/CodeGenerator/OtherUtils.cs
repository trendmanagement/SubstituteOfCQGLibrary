using System.IO;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        /// <summary>
        /// Write a line of code that collects all the arguments into object[] as following:
        /// var args = new object[] { arg1, arg2, ..., argn }
        /// </summary>
        static void CreateArgsObjectArray(StreamWriter file, string indent, ParameterInfo[] pinfos, bool addExclamation = false)
        {
            file.Write(indent + "var args = new object[] { ");
            if (addExclamation)
            {
                file.Write("\"!\", ");
            }
            for (int i = 0; i < pinfos.Length; i++)
            {
                ParameterInfo pinfo = pinfos[i];
                string paramName = GetParamName(pinfo);
                file.Write(paramName);
                if (i != pinfos.Length - 1)
                {
                    file.Write(", ");
                }
            }
            file.WriteLine(" };");
        }

        /// <summary>
        /// Get parameter name of choose a default name for nameless parameters
        /// </summary>
        static string GetParamName(ParameterInfo pinfo)
        {
            if (pinfo.Name == null)
            {
                return "arg" + (pinfo.Position + 1).ToString();
            }
            else
            {
                return pinfo.Name;
            }
        }
    }
}