using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Core.Data
{
    internal class VendorEntity : TableEntityBase<IVendor>
    {
        internal VendorEntity(Guid id) : base(id)
        {
            UserId = id;
        }
        internal VendorEntity(IVendor model) : base(model)
        {

        }

        internal override string TableName => "vendors";

        public Int32 Type
        {
            get;
            set;
        }

        public Guid UserId
        {
            get; set;
        }

        internal override IVendor ConvertToModel()
        {
            IVendor vendor = VendorBase.GetVendorFromType((VendorType)Type);
            if (vendor != null)
            {
                var uEntity = new UserEntity(Id);
                var user = uEntity.FetchById().Result;
                vendor.Id = Id;
                vendor.Name = user.Name;
                vendor.Username = user.Username;
                vendor.Password = user.Password;
                vendor.Contact.Clone(user.Contact);
            }
            return vendor;
        }
        internal override void PopulateFromModel(IVendor model)
        {
            UserId = model.Id;
            Type = (Int32)model.Type;
        }
        internal override List<IVendor> ExtractModels(List<DynamicTableEntity> entities)
        {
            var list = new List<IVendor>();
            foreach (var entity in entities)
            {
                IVendor vendor = VendorBase.GetVendorFromType((VendorType)entity.Properties["Type"].Int32Value.Value);
                if (vendor != null)
                {
                    vendor.Id = entity.Properties["Id"].GuidValue.Value;
                    var uEntity = new UserEntity(Id);
                    var user = uEntity.FetchById().Result;
                    vendor.Id = Id;
                    vendor.Name = user.Name;
                    vendor.Username = user.Username;
                    vendor.Password = user.Password;
                    vendor.Contact.Clone(user.Contact);
                    list.Add(vendor);
                }

            }
            return list;
        }
        internal async override Task<IVendor> Save()
        {
            PopulateFromModel(Model);
            var save = await base.Save();
            var uEntity = new UserEntity((IUser)Model);
            await uEntity.Save();
            return save;
        }
        internal async override Task<int> Delete()
        {
            var result = await base.Delete();
            var uEntity = new UserEntity(Id);
            await uEntity.Delete();
            return result;
        }
    }
}
