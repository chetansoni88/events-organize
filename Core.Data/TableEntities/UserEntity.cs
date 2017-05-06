using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Core.Data
{
    internal class UserEntity : TableEntityBase<IUser>, IUser
    {
        internal UserEntity(Guid id) : base(id)
        {

        }
        internal UserEntity(IUser model) : base(model)
        {
            
        }
        public IContact Contact { get; private set; }

        private string _contactJSON = string.Empty;
        public string ContactJSON
        {
            get
            {
                if (Contact != null && !string.IsNullOrEmpty(Contact.Email))
                    _contactJSON = JsonConvert.SerializeObject(Contact, Formatting.None);
                return _contactJSON;
            }
            set
            {
                _contactJSON = value;
            }
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
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
                user.Contact.Clone(JsonConvert.DeserializeObject<Contact>(entity.Properties["ContactJSON"].StringValue));
                list.Add(user);
            }
            return list;
        }
    }
}
