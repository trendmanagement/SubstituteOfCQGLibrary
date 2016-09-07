using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageOrdersToTTWPFLibrary
{
    public class FixConnectionEvent
    {
        public delegate void AlertEventHandler(Object sender, AlertEventArgs e);

        //delegate void updateConnection();

        public FixConnectionEvent()
        {

        }

        //public void updateCQGReConnectBtn()
        //{
        //    if (this.InvokeRequired)
        //    {
        //        ThreadSafeTTFIXConnDelegate d = new ThreadSafeTTFIXConnDelegate(threadSafeTTFIXConn);

        //        this.Invoke(d, e);
        //    }
        //    else
        //    {
        //        threadSafeTTFIXConn(e);
        //    }
        //}

        public void isConnectedToFIXTTServer()
        {
            AlertEventArgs alertEventArgs = new AlertEventArgs();
            //alertEventArgs.uuiData = "Connected To TT FIX";
            alertEventArgs.fixConn = true;
            CallAlert(new object(), alertEventArgs);
        }

        public void isDisconnectedFromFIXTTServer()
        {
            AlertEventArgs alertEventArgs = new AlertEventArgs();
            //alertEventArgs.uuiData = "Disconnected From TT FIX";
            alertEventArgs.fixConn = false;
            CallAlert(new object(), alertEventArgs);
        }

        public event AlertEventHandler CallAlert; 
    }

    public class AlertEventArgs : EventArgs
    {
        #region AlertEventArgs Properties
        //private string _uui = null;
        private bool conn = false;
        #endregion

        #region Get/Set Properties
        //public string uuiData
        //{
        //    get { return _uui; }
        //    set { _uui = value; }
        //}

        public bool fixConn
        {
            get { return conn; }
            set { conn = value; }
        }
        #endregion
    }
}
