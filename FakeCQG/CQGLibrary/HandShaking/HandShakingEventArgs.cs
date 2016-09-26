using FakeCQG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeCQG.Handshaking
{
    public class HandshakingEventArgs
    {
        private List<HandshakerModel> _subscriberas;
        public bool NoSubscribers { get; set; }
        public List<HandshakerModel> Subscribers { get { return _subscriberas; } }

        public HandshakingEventArgs()
        {
            NoSubscribers = true;
        }

        public HandshakingEventArgs(List<HandshakerModel> subscribers)
        {
            NoSubscribers = false;
            _subscriberas = subscribers;
        }
    }
}
