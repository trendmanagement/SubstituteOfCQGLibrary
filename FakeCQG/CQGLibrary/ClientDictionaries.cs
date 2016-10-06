using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FakeCQG
{
    public class ClientDictionaries
    {
        static Dictionary<string, bool> isAnswer = new Dictionary<string, bool>();

        static Dictionary<string, Dictionary<string, bool>> eventCheckingDictionary = new Dictionary<string, Dictionary<string, bool>>();

        public static HashSet<string> ObjectNames = new HashSet<string>(); 
        public static Dictionary<string, bool> IsAnswer
        {
            get
            {
                return isAnswer;
            }
        }


        public static Dictionary<string, Dictionary<string, bool>> EventCheckingDictionary
        {
            get
            {
                return eventCheckingDictionary;
            }
        }

        public static void FillEventCheckingDictionary(string objKey, string objTName)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string assmPath = Path.Combine(path, "Interop.CQG.dll");
            var assm = Assembly.LoadFrom(assmPath);

            Type objT = assm.GetType(objTName, true);

            //foreach (Type type in assm.ExportedTypes.Where(type => type.IsClass))
            //{
            IEnumerable<EventInfo> einfos = objT.GetEvents();

            if (einfos != null)
            {
#if DEBUG
                einfos = einfos.OrderBy(einfo => einfo.EventHandlerType.Name);
#endif
                Dictionary<string, bool> objEventCheckingDictionary = new Dictionary<string, bool>();
                foreach (EventInfo einfo in einfos)
                {
                    objEventCheckingDictionary.Add(einfo.Name, false);
                }
                eventCheckingDictionary.Add(objKey, objEventCheckingDictionary);
            }
        }
    }
}
