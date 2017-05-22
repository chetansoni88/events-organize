using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class User : IUser
    {
        public IContact Contact { get; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }
        public User()
        {
            Contact = new Contact();
        }
    }
}
