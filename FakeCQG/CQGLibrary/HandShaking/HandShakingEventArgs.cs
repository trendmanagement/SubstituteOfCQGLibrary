using System.Collections.Generic;
using FakeCQG.Internal.Models;
using System;

namespace FakeCQG.Internal.Handshaking
{
    public class HandshakingEventArgs
    {
        private List<HandshakingModel> _subscribers;
        public bool NoSubscribers { get; set; }
        public List<HandshakingModel> Subscribers { get { return _subscribers; } }

        public List<Guid> Ids { get; set; }

        public HandshakingEventArgs()
        {
            NoSubscribers = true;
        }

        public HandshakingEventArgs(List<HandshakingModel> subscribers)
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
