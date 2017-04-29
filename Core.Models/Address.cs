using System;

namespace Core.Models
{
    public class Address : IAddress
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string State { get; set; }

        public void Clone(IAddress address)
        {
            Street = address.Street;
            City = address.City;
            Zip = address.Zip;
            Country = address.Country;
            State = address.State;
        }
    }

    public class Contact : IContact
    {
        public Contact()
        {
            Address = new Address();
        }

        public IAddress Address { get; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public Guid Id { get; set; }

        public void Clone(IContact contact)
        {
            Email = contact.Email;
            Phone = contact.Phone;
            Id = contact.Id;
            Address.Clone(contact.Address);
        }
    }
}
