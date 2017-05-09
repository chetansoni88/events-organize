using System;
using System.Collections.Generic;

namespace Core.Models
{

    public interface IModel
    {
        Guid Id { get; set; }
    }

    public interface IVendor : IUser, IModel
    {
        VendorType Type { get; }
    }

    public interface IArrangement : IModel
    {
        IVendor Vendor { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        ArrangementStatus Status { get; set; }
    }

    public interface IEvent : IModel
    {
        List<IArrangement> Arrangements { get; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        EventType Type { get; }
        IContact Contact { get; }
        string Name { get; set; }
        IAddress Venue { get; }
    }

    public interface IUser : IModel
    {
        IContact Contact { get; }
        string Username { get; set; }
        string Password { get; set; }
        string Name { get; set; }
    }

    public interface IContact : IModel
    {
        IAddress Address { get; }
        string Email { get; set; }
        string Phone { get; set; }

        void Clone(IContact contact);
    }

    public interface IAddress
    {
        string Street { get; set; }
        string City { get; set; }
        string Zip { get; set; }
        string Country { get; set; }
        string State { get; set; }

        void Clone(IAddress address);
    }

    public interface IProject : IModel
    {
        List<IEvent> Events { get; }
        string Name { get; set; }

        Guid UserId { get; set; }
    }

    public interface IToken:IModel
    {
        Guid UserId { get; set; }
    }
}
