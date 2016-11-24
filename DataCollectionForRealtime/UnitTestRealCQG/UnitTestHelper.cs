using System.Threading.Tasks;
using DataCollectionForRealtime;
using FakeCQG.Internal;

namespace UnitTestRealCQG
{
    internal static class UnitTestHelper
    {
        public static QueryHandler QueryHandler { get; set; }
        public static CQGDataManagement CQGDataManagment { get; set; }
        public static DCMainForm DCMainForm { get; set; }
        public static void StartUp()
        {
            DCMainForm = new DCMainForm();
            CQGDataManagment = new CQGDataManagement(DCMainForm, null);
            QueryHandler = new QueryHandler(CQGDataManagment);
            QueryHandler.HelpersInit();

            Task.Run(async () =>
            {
                await QueryHandler.ClearQueriesListAsync();
                await Core.AnswerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();
        }
    }
}
