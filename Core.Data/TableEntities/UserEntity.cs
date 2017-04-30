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
            RowKey = Username;
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
        internal override List<IUser> ExtractModels(List<DynamicTableEntity> entities)
        {
            var list = new List<IUser>();
            foreach (var entity in entities)
            {
                IUser user = new User();
                user.Id = entity.Properties["Id"].GuidValue.Value;
                user.Name = entity.Properties["Name"].StringValue;
                user.Password = entity.Properties["Password"].StringValue;
                user.Username = entity.Properties["Username"].StringValue;
                list.Add(user);
            }
            return list;
        }
    }
}
