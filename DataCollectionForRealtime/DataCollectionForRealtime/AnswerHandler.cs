using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeCQG.Internal;
using MongoDB.Driver;

namespace DataCollectionForRealtime
{
    static class AnswerHandler
    {
        public static Task PushAnswerAsync(FakeCQG.Internal.Models.AnswerInfo answer)
        {
            return Task.Run(() => PushAnswer(answer));
        }

        public static void PushAnswer(FakeCQG.Internal.Models.AnswerInfo answer)
        {
            try
            {
                Core.AnswerHelper.GetCollection.InsertOne(answer);
                lock (Core.LogLock)
                {
                    AsyncTaskListener.LogMessage("************************************************************");
                    AsyncTaskListener.LogMessage(answer.ToString());
                }
            }
            catch (Exception ex)
            {
                AsyncTaskListener.LogMessage(ex.Message);
                if (Core.AnswerHelper.Connect())
                {
                    PushAnswer(answer);
                }
            }
        }
    }
}
