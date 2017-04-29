using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public abstract class EventBase : IEvent
    {
        public List<IVendor> Vendors { get; private set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public virtual EventType Type => throw new NotImplementedException();
        public IContact Contact { get; }
        public Guid Id { get; set; }
        public EventBase()
        {
            Contact = new Contact();
            Vendors = new List<IVendor>();
        }
    }
}
