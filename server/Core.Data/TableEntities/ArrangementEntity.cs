using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System.Linq;

namespace Core.Data
{
    internal class ArrangementEntity : TableEntityBase<IArrangement>

    {
        public ArrangementEntity(Guid id) : base(id)
        {

        }

        public ArrangementEntity(IArrangement model) : base(model)
        {

        }

        internal override string TableName => "arrangements";

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid VendorId { get; set; }
        public Int32 Status { get; set; }

        internal override IArrangement ConvertToModel()
        {
            IArrangement a = new Arrangement();
            a.StartTime = StartTime;
            a.EndTime = EndTime;
            a.Id = Id;
            a.Status = (ArrangementStatus)Status;
            a.Vendor = (new VendorEntity(VendorId)).FetchById().Result;
            return a;
        }
        internal override void PopulateFromModel(IArrangement model)
        {
            Id = model.Id;
            StartTime = model.StartTime;
            EndTime = model.EndTime;
            Status = (Int32)model.Status;
            if (model.Vendor != null)
                VendorId = model.Vendor.Id;
        }
        internal override List<IArrangement> ExtractModels(List<DynamicTableEntity> entities)
        {
            var list = new List<IArrangement>();
            foreach (var entity in entities)
            {
                IArrangement a = new Arrangement();
                a.Id = entity.Properties["Id"].GuidValue.Value;
                a.StartTime = entity.Properties["StartTime"].DateTime.Value;
                a.EndTime = entity.Properties["EndTime"].DateTime.Value;
                a.Status = (ArrangementStatus)entity.Properties["Status"].Int32Value.Value;
                a.Vendor = (new VendorEntity(entity.Properties["VendorId"].GuidValue.Value)).FetchById().Result;
                list.Add(a);
            }
            return list;
        }
    }
}
