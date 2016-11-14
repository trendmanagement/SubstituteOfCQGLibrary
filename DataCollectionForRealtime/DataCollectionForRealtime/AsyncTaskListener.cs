using System;
using System.Collections.Generic;
using FakeCQG.Internal;

namespace DataCollectionForRealtime
{
    /// <summary>
    /// Helper class for reporting messages from asynchronous tasks
    /// </summary>
    static class AsyncTaskListener
    {
        public delegate void UpdateDelegate(string msg = null);
        public static event UpdateDelegate Updated;

        #region Log settings fields

        static List<string[]> templatesOfLogMessage = new List<string[]>();

        static Dictionary<string[], string> lastLogMessages = new Dictionary<string[], string>();
        
        public static HashSet<string> AllLog = new HashSet<string>();

        #endregion

        static AsyncTaskListener()
        {
            // Initialization of templates
            templatesOfLogMessage.Add(new string[] { "No subscribers for handshaking", "DC has" });
            templatesOfLogMessage.Add(new string[] { "new quer(y/ies) in database" });
            templatesOfLogMessage.Add(new string[] { "Events list was cleared successfully" });
            templatesOfLogMessage.Add(new string[] { "Queries list was cleared successfully" });
            templatesOfLogMessage.Add(new string[] { "Timer elapsed. No answer." });
            templatesOfLogMessage.Add(new string[] { "QUERY" });
            templatesOfLogMessage.Add(new string[] { "ANSWER" });
            templatesOfLogMessage.Add(new string[] { "EVENT" });
        }

        // Checking whether last message matches with incoming template
        static bool HasData(string [] template, string msg)
        {
            if (lastLogMessages.ContainsKey(template))
            {
                if (lastLogMessages[template] == msg)
                {
                    return false;
                }
                else
                {
                    lastLogMessages[template] = msg;
                    return true;
                }
            }
            else
            {
                lastLogMessages.Add(template, msg);
                return true;
            }
        }

        // Check whether log update is required 
        static bool LogUpdateRequired(string msg)
        {
            bool doesLogContainMessage = AllLog.Add(msg);
            foreach (var template in templatesOfLogMessage)
            {
                foreach (var message in template)
                {
                    if (msg.Contains(message))
                    {
                        return HasData(template, msg);
                    }
                }
            }
            return doesLogContainMessage;
        }

        static void Update(string msg)
        {
            try
            {
                // Update text box
                Updated.Invoke(msg);
            }
            catch
            {
            }
        }

        public static void LogMessage(string msg)
        {
            switch (Core.LogMode)
            {
                case LogModeEnum.Muted:
                    break;
                case LogModeEnum.Filtered:
                    if (LogUpdateRequired(msg))
                    {
                        Update(msg);
                    }
                    break;
                case LogModeEnum.Unfiltered:
                    Update(msg);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public static void LogMessageFormat(string msgPat, params object[] args)
        {
            // Update text box
            Updated.Invoke(string.Format(msgPat, args));
        }
    }
}
