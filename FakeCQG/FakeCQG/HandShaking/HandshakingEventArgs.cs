using System;
using System.Collections.Generic;
using FakeCQG.Internal.Models;

namespace FakeCQG.Internal.Handshaking
{
    public class HandshakingEventArgs
    {
        private List<HandshakingInfo> _subscribers;
        public bool NoSubscribers { get; set; }
        public List<HandshakingInfo> Subscribers { get { return _subscribers; } }

        public List<Guid> Ids { get; set; }

        public HandshakingEventArgs()
        {
            NoSubscribers = true;
        }

        public HandshakingEventArgs(List<HandshakingInfo> subscribers)
        {
            Ids = new List<Guid>();
            foreach (var item in subscribers)
            {
                Ids.Add(item.ID);
            }
            NoSubscribers = false;
            _subscribers = subscribers;
        }
    }
}
