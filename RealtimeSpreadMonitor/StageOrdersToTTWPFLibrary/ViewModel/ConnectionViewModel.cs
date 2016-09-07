using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix;

namespace StageOrdersToTTWPFLibrary.ViewModel
{
    class ConnectionViewModel : ViewModelBase
    {
        private FixConnectionSystem _fixConnectionSystem;
        private FixConnectionEvent _fxce;

        //public ICommand ConnectCommand { get; set; }
        //public ICommand DisconnectCommand { get; set; }


        public ConnectionViewModel(FixConnectionSystem fixConnectionSystem,
            FixConnectionEvent fxce)
        {
            _fixConnectionSystem = fixConnectionSystem;

            _fxce = fxce;

            // initialize SessionString
            HashSet<QuickFix.SessionID> sidset = _fixConnectionSystem.MySessionSettings.GetSessions();
            Trace.WriteLine("Sessions count in config: " + sidset.Count);
            //foreach (QuickFix.SessionID sid in sidset)
            //    Trace.WriteLine("-> " + sid.ToString());
            this.SessionString = sidset.First().SenderCompID.ToString() + " "
                + sidset.First().TargetCompID.ToString();

            //this.PriceSessionString = sidset.ElementAt(1).TargetCompID.ToString();

            // command definitions
            //ConnectCommand = new RelayCommand(Connect);
            //DisconnectCommand = new RelayCommand(Disconnect);

            fixConnectionSystem.OrderSessionLogonEvent += new Action(delegate() 
                { IsOrderConnected = true;
                _fxce.isConnectedToFIXTTServer();   
            });
            fixConnectionSystem.OrderSessionLogoutEvent += new Action(delegate() 
                { IsOrderConnected = false;
                _fxce.isDisconnectedFromFIXTTServer();
                });

            fixConnectionSystem.PriceSessionLogonEvent += new Action(delegate() { IsPriceConnected = true; });
            fixConnectionSystem.PriceSessionLogoutEvent += new Action(delegate() { IsPriceConnected = false; });

        }


        private string _session = "";
        public string SessionString
        {
            get { return _session; }
            set { _session = value; base.OnPropertyChanged("SessionString"); }
        }

        private string _priceSessionString = "";
        public string PriceSessionString
        {
            get { return _priceSessionString; }
            set { _priceSessionString = value; base.OnPropertyChanged("PriceSessionString"); }
        }

        private bool _isOrderConnected = false;
        public bool IsOrderConnected
        {
            get { return _isOrderConnected; }
            set { _isOrderConnected = value; base.OnPropertyChanged("IsOrderConnected"); }
        }

        private bool _isPriceConnected = false;
        public bool IsPriceConnected
        {
            get { return _isPriceConnected; }
            set { _isPriceConnected = value; base.OnPropertyChanged("IsPriceConnected"); }
        }

        // commands
        //private void Connect(object ignored)
        //{
        //    Trace.WriteLine("ConnectionViewModel::Connect called");
        //    _qfapp.Start();
        //}

        //private void Disconnect(object ignored)
        //{
        //    Trace.WriteLine("ConnectionViewModel::Disconnect called");
        //    _qfapp.Stop();
        //}
    }
}
