using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        /// <summary>
        /// Write a line of code that collects all the arguments into object[] as following:
        /// var args = new object[] { arg1, arg2, ..., argn }
        /// </summary>
        static void CreateArgsObjectArray(string indent, ParameterInfo[] pinfos)
        {
            File.Write(indent + "var args = new object[] { ");

            for (int i = 0; i < pinfos.Length; i++)
            {
                ParameterInfo pinfo = pinfos[i];
                string paramName = GetParamName(pinfo);
                File.Write(paramName);
                if (i != pinfos.Length - 1)
                {
                    File.Write(", ");
                }
            }
            File.WriteLine(" };");
        }

        /// <summary>
        /// Create dictionaries for serializable and non-serializable parameters
        /// and put the parameters into the dictionaries
        /// </summary>
        static void CreateAndPopulateDictionaries(ParameterInfo[] pinfos, out bool serArgsFound, out bool nonSerArgsFound)
        {
            // Determine whether any serializable and non-serializable arguments are present
            serArgsFound = false;
            nonSerArgsFound = false;
            foreach (ParameterInfo pinfo in pinfos)
            {
                if (IsSerializableType(pinfo.ParameterType))
                {
                    serArgsFound = true;
                }
                else
                {
                    nonSerArgsFound = true;
                }
                if (serArgsFound && nonSerArgsFound)
                {
                    break;
                }
            }

            // Generate code for creation of dictionaries for argKeys and argValues (both optional)
            string pattern = Indent3 + "var {0} = new Dictionary<int, object>();";
            if (serArgsFound)
            {
                DCEvHndlrFile.WriteLine(string.Format(pattern, "serArgs"));
            }
            if (nonSerArgsFound)
            {
                DCEvHndlrFile.WriteLine(string.Format(pattern, "nonSerArgs"));
            }

            // Populate serArgs and nonSerArgs dictionaries
            pattern = Indent3 + "{0}[{1}] = {2};";
            for (int i = 0; i < pinfos.Length; i++)
            {
                ParameterInfo pinfo = pinfos[i];
                string paramName = GetParamName(pinfo);
                if (IsSerializableType(pinfo.ParameterType))
                {
                    DCEvHndlrFile.WriteLine(string.Format(pattern, "serArgs", i, paramName));
                }
                else
                {
                    DCEvHndlrFile.WriteLine(string.Format(pattern, "nonSerArgs", i, paramName));
                }
            }
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