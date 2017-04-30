using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Wedding : EventBase
    {
        public override EventType Type => EventType.Wedding;
    }
}
