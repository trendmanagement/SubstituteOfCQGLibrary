using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StageOrdersToTTWPFLibrary.Model;
using System.Diagnostics;

namespace StageOrdersToTTWPFLibrary.ViewModel
{
    class FIXMessageViewModel
    {
        private FixConnectionSystem _fixConnectionSystem;
        public ObservableCollection<MessageRecord> Messages { get; set; }

        public FIXMessageViewModel(FixConnectionSystem fixConnectionSystem)
        {
            this._fixConnectionSystem = fixConnectionSystem;

            Messages = new ObservableCollection<MessageRecord>();

            _fixConnectionSystem.MessageEvent += new Action<QuickFix.Message, bool>(HandleMessage);

        }

        public void HandleMessage(QuickFix.Message msg, bool isIncoming)
        {
            try
            {
                MessageRecord mr = new MessageRecord(msg, isIncoming);

                SmartDispatcher.Invoke(new Action<MessageRecord>(AddMessage), mr);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        public void AddMessage(MessageRecord r)
        {
            try
            {
                //Messages.Add(r);
                Messages.Insert(0, r);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

    }
}
