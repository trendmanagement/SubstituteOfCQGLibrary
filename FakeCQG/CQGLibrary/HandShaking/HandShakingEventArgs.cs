using System.Collections.Generic;
using FakeCQG.Models;

namespace FakeCQG.Handshaking
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
