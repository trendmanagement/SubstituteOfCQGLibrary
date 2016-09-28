namespace DataCollectionForRealtime
{
    /// <summary>
    /// Helper class for reporting messages from asynchronous tasks
    /// </summary>
    static class AsyncTaskListener
    {
        public delegate void UpdateDelegate(string msg = null);
        public static event UpdateDelegate Updated;

        public static void LogMessage(string msg)
        {
            try
            {
                // Update text box
                Updated.Invoke(msg);
            }
            catch
            {
                //TODO: If control not available 
            }        
        }

        public static void LogMessageFormat(string msgPat, params object[] args)
        {
            // Update text box
            Updated.Invoke(string.Format(msgPat, args));
        }
    }
}
