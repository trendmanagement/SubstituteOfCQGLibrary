using CQGLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQGLibrary.HandShaking
{
    public class HandShakingEventArgs
    {
        private List<HandShakerModel> _subscriberas;
        public bool NoSubscribers { get; set; }
        public List<HandShakerModel> Subscribers { get { return _subscriberas; } }

        public HandShakingEventArgs()
        {
            NoSubscribers = true;
        }

        public HandShakingEventArgs(List<HandShakerModel> subscribers)
        {
            NoSubscribers = false;
            _subscriberas = subscribers;
        }

    }
}
