using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Wedding : EventBase
    {
        public override EventType Type => EventType.Wedding;
    }
    public class BabyShower : EventBase
    {
        public override EventType Type => EventType.BabyShower;
    }
    public class Corporate : EventBase
    {
        public override EventType Type => EventType.Corporate;
    }
    public class Engagement : EventBase
    {
        public override EventType Type => EventType.Engagement;
    }
}
