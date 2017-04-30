using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Core.Data
{
    internal class EventEntity : TableEntityBase<IEvent>, IEvent
    {
        public EventEntity(IEvent model) : base(model)
        {

        }

        internal override string TableName => "events";

        public List<IVendor> Vendors { get; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public EventType Type { get; private set; }

        public IContact Contact { get; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        internal override IEvent ConvertToModel()
        {
            IEvent e = null;
            switch (Type)
            {
                case EventType.Wedding:
                    e = new Wedding();
                    break;
            }
            e.Id = Id;
            e.StartTime = StartTime;
            e.EndTime = EndTime;
            e.Name = Name;
            e.Contact.Clone(Contact);
            e.Vendors.AddRange(Vendors);
            return e;
        }
        internal override void PopulateFromModel(IEvent model)
        {
            Id = model.Id;
            StartTime = model.StartTime;
            EndTime = model.EndTime;
            Name = model.Name;
            Type = model.Type;
            Vendors.AddRange(model.Vendors);
            Contact.Clone(model.Contact);
        }
        internal override List<IEvent> ExtractModels(List<DynamicTableEntity> entities)
        {
            var list = new List<IEvent>();
            foreach (var entity in entities)
            {
                IEvent e = null;
                switch((EventType)entity.Properties["Type"].Int32Value.Value)
                {
                    case EventType.Wedding:
                        e = new Wedding();
                        break;
                }
                e.Id = entity.Properties["Id"].GuidValue.Value;
                e.StartTime = entity.Properties["StartTime"].DateTime.Value;
                e.EndTime = entity.Properties["EndTime"].DateTime.Value;
                e.Name = entity.Properties["Name"].StringValue;
                list.Add(e);
            }
            return list;
        }
    }
}
