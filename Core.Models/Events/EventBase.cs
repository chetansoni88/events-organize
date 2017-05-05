using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public abstract class EventBase : IEvent
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public virtual EventType Type => throw new NotImplementedException();
        public IContact Contact { get; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<IArrangement> Arrangements { get; private set; }
        public IAddress Venue { get; }

        public EventBase()
        {
            Contact = new Contact();
            Arrangements = new List<IArrangement>();
            Venue = new Address();
        }

        public static IEvent GetEventFromType(EventType type)
        {
            IEvent e = null;
            switch (type)
            {
                case EventType.Wedding:
                    e = new Wedding();
                    break;
                case EventType.BabyShower:
                    e = new BabyShower();
                    break;
                case EventType.Corporate:
                    e = new Corporate();
                    break;
                case EventType.Engagement:
                    e = new Engagement();
                    break;
            }
            return e;
        }
    }
}
