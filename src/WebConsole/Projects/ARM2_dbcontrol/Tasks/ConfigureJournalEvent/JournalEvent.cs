using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ARM2_dbcontrol.Tasks.ConfigureJournalEvent
{
    [Serializable]
    public class JournalEvent
    {
        private SingleJournalEvent[] _events;

        public SingleJournalEvent[] Events
        {
            get { return _events; }
            set
            {
                if (value == null)
                    _events = new SingleJournalEvent[0];
                else
                    _events = value;
            }
        }

        public JournalEvent()
        {
            _events = new SingleJournalEvent[0];            
        }

        public JournalEvent(String[] EventNames)
        {
            _events = new SingleJournalEvent[EventNames.Length];
            for (Int32 i = 0; i < EventNames.Length; i++)
            {
                Events[i] = new SingleJournalEvent();
                Events[i].EventName = EventNames[i];
            }
        }

        public String GetTask()
        {
            StringBuilder task = new StringBuilder(256);
            
            task.Append("<param>");
            task.Append("<id>Events</id>");
            task.Append("<type>stringmap</type>");
            task.Append("<value>");
            for (Int32 i = 1; i < _events.Length; i++)
            {
                task.AppendFormat("<string><id>{0}</id>{1}</string>", i.ToString(), ConvertEventForTask(_events[i]));
            }
            task.Append("</value>");
            task.Append("</param>");

            return task.ToString();
        }

        private String ConvertEventForTask(SingleJournalEvent journalEvent)
        {
            UInt32 val = 0;
            if ((journalEvent.EventFlag & EventJournalFlags.WindowsJournal) != 0)
                val += (UInt32)EventJournalFlags.WindowsJournal;

            if ((journalEvent.EventFlag & EventJournalFlags.LocalJournal) != 0)
                val += (UInt32)EventJournalFlags.LocalJournal;

            if ((journalEvent.EventFlag & EventJournalFlags.CCJournal) != 0)
                val += (UInt32)EventJournalFlags.CCJournal;

            return String.Format("<key>{0}</key><val>{1}</val>", journalEvent.EventName, val.ToString("X"));
        }

        public void ClearEvents()
        {
            for (Int32 i = 0; i < _events.Length; i++)
            {
                _events[i].EventFlag = EventJournalFlags.NoOneJournal;
            }
        }
    }

    public struct SingleJournalEvent
    {
        public String EventName;
        public EventJournalFlags EventFlag;
    }

    [Flags]
    public enum EventJournalFlags
    {
        NoOneJournal = 0x00,
        WindowsJournal = 0x01,
        LocalJournal = 0x02,
        CCJournal = 0x04
    }
}
