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
    }
}
