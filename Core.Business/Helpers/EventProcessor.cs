using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business
{
    public class EventProcessor : ProcessorBase<IEvent>
    {
        public EventProcessor(Guid id) : base(id)
        {

        }

        public EventProcessor(IEvent eveent) : base(eveent)
        {

        }

    }
}
