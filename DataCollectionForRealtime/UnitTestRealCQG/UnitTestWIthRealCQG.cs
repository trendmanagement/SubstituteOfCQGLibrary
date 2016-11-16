using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using FakeCQG;
using FakeCQG.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestRealCQG
{
    [TestClass]
    [DeploymentItem("Interop.CQG.dll")]
    public class UnitTestWIthRealCQG
    {
        public string status = string.Empty;
        [TestMethod]
        //This test should run if CQG client is connected
        public void FakeCQG_EventHandlersWork()
        {
            // arrange
            UnitTestHelper.StartUp();
            string statusConnectionUp = "csConnectionUp";
            Timer timer = new Timer();
            timer.Interval = 30;
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            CQGCEL fakeCQGCel = new CQGCELClass();
            fakeCQGCel.DataConnectionStatusChanged += new _ICQGCELEvents_DataConnectionStatusChangedEventHandler(Cell_DataConnectionStatusChanged);
            Core.LogChange += CQG_LogChange;

            // act
            fakeCQGCel.Startup();
            Task.Delay(300).GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(statusConnectionUp, status);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UnitTestHelper.QueryHandler.ReadQueries();
            UnitTestHelper.QueryHandler.ProcessEntireQueryList();
        }

        private void Cell_DataConnectionStatusChanged(eConnectionStatus new_status)
        {
            status = new_status.ToString();
        }

        private void CQG_LogChange(string message)
        {
        }



    }
}
