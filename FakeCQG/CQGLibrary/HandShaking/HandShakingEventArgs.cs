using System.Collections.Generic;
using FakeCQG.Internal.Models;

namespace FakeCQG.Internal.Handshaking
{
    public class HandshakingEventArgs
    {
        private List<HandshakingModel> _subscribers;
        public bool NoSubscribers { get; set; }
        public List<HandshakingModel> Subscribers { get { return _subscribers; } }

        public HandshakingEventArgs()
        {
            NoSubscribers = true;
        }

        public HandshakingEventArgs(List<HandshakingModel> subscribers)
        {
            NoSubscribers = false;
            _subscribers = subscribers;
        }
    }
}
