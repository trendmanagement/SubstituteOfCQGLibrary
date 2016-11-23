using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FakeCQG.Internal
{
    // Here placed dictionaries and methods for its managing that are used on the client's side
    public class ClientDictionaries
    {
        static Dictionary<string, bool> isAnswer = new Dictionary<string, bool>(StringComparer.Ordinal);

        static Dictionary<string, Dictionary<string, bool>> eventCheckingDictionary = new Dictionary<string, Dictionary<string, bool>>(StringComparer.Ordinal);

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

        public static void FillEventCheckingDictionary(string objKey, string objTypeName)
        {
            // Load fake assembly
            string fakeAssmDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fakeAssmPath = Path.Combine(fakeAssmDir, "FakeCQG.dll");
            var fakeAssm = Assembly.LoadFrom(fakeAssmPath);

            // Get fake object type
            string fakeObjTypeName = "Fake" + objTypeName;
            Type fakeObjType = fakeAssm.GetType(fakeObjTypeName, true);

            IEnumerable<EventInfo> einfos = fakeObjType.GetEvents();

            if (einfos != null)
            {
#if DEBUG
                einfos = einfos.OrderBy(einfo => einfo.EventHandlerType.Name);
#endif
                Dictionary<string, bool> objEventCheckingDictionary = new Dictionary<string, bool>(StringComparer.Ordinal);
                foreach (EventInfo einfo in einfos)
                {
                    objEventCheckingDictionary.Add(einfo.Name, false);
                }
                eventCheckingDictionary.Add(objKey, objEventCheckingDictionary);
            }
        }
    }
}
