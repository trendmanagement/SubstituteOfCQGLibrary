using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StageOrdersToTTWPFLibrary.View
{
    /// <summary>
    /// Interaction logic for StageOrdersMainView.xaml
    /// </summary>
    public partial class StageOrdersMainView : Window
    {
        private FixConnectionSetupAndMethods _fixConnSetupAndMethods;

        public StageOrdersMainView(FixConnectionSetupAndMethods fixConnSetupAndMethods)
        {
            this._fixConnSetupAndMethods = fixConnSetupAndMethods;

            //InitializeComponent();
        }

        

        //public StageOrderWindow(FixConnectionSetupAndMethods fixConnSetupAndMethods)
        //{
            

        //    InitializeComponent();
        //}



        private void btnLogon_Click_1(object sender, RoutedEventArgs e)
        {
            _fixConnSetupAndMethods.logonFixConnection();
        }

        private void btnLogoff_Click_1(object sender, RoutedEventArgs e)
        {
            _fixConnSetupAndMethods.shutdownFixConnection();
        }

        private void btnSecDefReq_Click(object sender, RoutedEventArgs e)
        {
            //_fixConnSetupAndMethods.sendDefRequest();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

    }
}
