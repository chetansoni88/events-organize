using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Project : IProject
    {
        public List<IEvent> Events { get; }
        public Guid Id { get; set; }
        public Project()
        {
            Events = new List<IEvent>();
        }
    }
}
