using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Core.Data
{
    internal class UserEntity : TableEntityBase<IUser>, IUser
    {
        public UserEntity(IUser model) : base(model)
        {
            PartitionKey = "User";
            RowKey = Id.ToString();
        }
        public IContact Contact { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }
        internal override string TableName => "users";
        internal override IUser ConvertToModel()
        {
            IUser user = new User();
            user.Id = Id;
            user.Name = Name;
            user.Username = Username;
            user.Password = Password;
            user.Contact.Clone(Contact);
            return user;
        }
        internal override void PopulateFromModel(IUser model)
        {
            Id = model.Id;
            Name = model.Name;
            Username = model.Username;
            Password = model.Password;
            Contact = new Contact();
            Contact.Clone(model.Contact);
        }
    }
}
