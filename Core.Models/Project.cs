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
            Users = new List<IUser>();
            Vendors = new List<IVendor>();
        }

        public string Name { get; set; }

        List<IUser> Users { get; }
        List<IVendor> Vendors { get; }
    }
}
