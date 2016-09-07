using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StageOrdersToTTWPFLibrary.ViewModel;
using StageOrdersToTTWPFLibrary.View;

namespace StageOrdersToTTWPFLibrary
{
    public class FixOrderStagingController
    {
        private FixConnectionSetupAndMethods _fixConnSetupAndMethods;

        public FixConnectionEvent initializeFixSetup(string portfolioName)
        {
            _fixConnSetupAndMethods =
                new FixConnectionSetupAndMethods();            

            _fixConnSetupAndMethods.setupFixConnection();

            StageOrdersMainView stageOrdersMainView =
                new StageOrdersMainView(_fixConnSetupAndMethods);

            stageOrdersMainView.DataContext = new StageOrdersMainViewModel();

            stageOrdersMainView.Title = portfolioName + " Staging Controller";

            OrderViewModel orderViewModel 
                = new OrderViewModel(_fixConnSetupAndMethods,
                    _fixConnSetupAndMethods.getFixConnectionSystem());

            _fixConnSetupAndMethods.setOrderViewModel(orderViewModel);

            //stageOrdersMainView.OrderView.DataContext
            //    = orderViewModel;

            //stageOrdersMainView.FIXMessageView.DataContext
            //    = new FIXMessageViewModel(
            //            _fixConnSetupAndMethods.getFixConnectionSystem());

            //stageOrdersMainView.ExecutionView.DataContext
            //    = new ExecutionViewModel(
            //            _fixConnSetupAndMethods.getFixConnectionSystem());

            FixConnectionEvent fxce = new FixConnectionEvent();

            ConnectionViewModel connectionViewModel
                = new ConnectionViewModel(_fixConnSetupAndMethods.getFixConnectionSystem(),
                    fxce);

            _fixConnSetupAndMethods.setConnectionViewModel(connectionViewModel);

            //stageOrdersMainView.ConnectionView.DataContext = connectionViewModel;

            SmartDispatcher.SetDispatcher(stageOrdersMainView.Dispatcher);

            stageOrdersMainView.Show();

            _fixConnSetupAndMethods.logonFixConnection();

            

            return fxce;
        }

        public void shutDownAndLogOff()
        {
            _fixConnSetupAndMethods.shutdownFixConnection();
        }

        public void stageOrders(List<StageOrdersToTTWPFLibrary.Model.OrderModel> contractListToStage)
        {
            _fixConnSetupAndMethods.sendOrder(contractListToStage);

            //_fixConnSetupAndMethods.sendDefRequest(contractListToStage);

        }

        public void securityDefRequest(List<StageOrdersToTTWPFLibrary.Model.OrderModel> contractListToStage)
        {
            //_fixConnSetupAndMethods.sendOrder(contractListToStage);

            _fixConnSetupAndMethods.sendDefRequest(contractListToStage);

        }
    }
}
