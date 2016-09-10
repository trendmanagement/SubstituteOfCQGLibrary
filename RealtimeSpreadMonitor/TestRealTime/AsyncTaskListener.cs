using System;

namespace TestRealTime
{
    static class AsyncTaskListener
    {
        public delegate void UpdateDelegate(string msg = null);
        public static event UpdateDelegate Updated;

        public static void LogMessage(string msg)
        {
            // Update text box
            Updated.Invoke(msg);
        }

        public static void LogMessageFormat(string msgPat, params object[] args)
        {
            // Update text box
            Updated.Invoke(string.Format(msgPat, args));
        }

        internal static void LogMessageFormat(string v, object key)
        {
            throw new NotImplementedException();
        }
    }
}
