using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Data
{
    internal class EventEntity : TableEntityBase<IEvent>
    {
        public EventEntity(Guid id) : base(id)
        {

        }

        public EventEntity(IEvent model) : base(model)
        {

        }

        internal override string TableName => "events";

        public List<IArrangement> Arrangements { get; private set; }

        private string _arrangementsJSON = string.Empty;
        public string ArrangementsJSON
        {
            get
            {
                if (Arrangements != null && Arrangements.Count > 0 && string.IsNullOrEmpty(_arrangementsJSON))
                {
                    var newList = Arrangements.Select(a => { return new { Id = a.Id }; });
                    _arrangementsJSON = JsonConvert.SerializeObject(newList, Formatting.None);
                }
                return _arrangementsJSON;
            }
            set
            {
                _arrangementsJSON = value;
            }
        }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Int32 Type { get; set; }

        public IContact Contact { get; set; }

        public IAddress Venue { get; set; }

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

        private string _venueJSON = string.Empty;
        public string VenueJSON
        {
            get
            {
                if (Venue != null && !string.IsNullOrEmpty(Venue.Street))
                    _venueJSON = JsonConvert.SerializeObject(Venue, Formatting.None);
                return _venueJSON;
            }
            set
            {
                _venueJSON = value;
            }
        }

        public string Name { get; set; }

        internal override IEvent ConvertToModel()
        {
            IEvent e = null;
            switch ((EventType)Type)
            {
                case EventType.Wedding:
                    e = new Wedding();
                    break;
                case EventType.BabyShower:
                    e = new BabyShower();
                    break;
                case EventType.Corporate:
                    e = new Corporate();
                    break;
                case EventType.Engagement:
                    e = new Engagement();
                    break;
            }
            e.Id = Id;
            e.StartTime = StartTime;
            e.EndTime = EndTime;
            e.Name = Name;
            e.Contact.Clone(Contact);
            e.Arrangements.AddRange(Arrangements);
            return e;
        }
        internal override void PopulateFromModel(IEvent model)
        {
            Id = model.Id;
            StartTime = model.StartTime;
            EndTime = model.EndTime;
            Name = model.Name;
            Type = (Int32)model.Type;
            Arrangements = new List<IArrangement>();
            Arrangements.AddRange(model.Arrangements);
            Contact = new Contact();
            Contact.Clone(model.Contact);
            Venue = new Address();
            Venue.Clone(model.Venue);
        }
        internal override List<IEvent> ExtractModels(List<DynamicTableEntity> entities)
        {
            var list = new List<IEvent>();
            foreach (var entity in entities)
            {
                IEvent e = null;
                switch ((EventType)entity.Properties["Type"].Int32Value.Value)
                {
                    case EventType.Wedding:
                        e = new Wedding();
                        break;
                    case EventType.BabyShower:
                        e = new BabyShower();
                        break;
                    case EventType.Corporate:
                        e = new Corporate();
                        break;
                    case EventType.Engagement:
                        e = new Engagement();
                        break;
                }
                if (e != null)
                {
                    e.Id = entity.Properties["Id"].GuidValue.Value;
                    e.StartTime = entity.Properties["StartTime"].DateTime.Value;
                    e.EndTime = entity.Properties["EndTime"].DateTime.Value;
                    e.Name = entity.Properties["Name"].StringValue;
                    e.Contact.Clone(JsonConvert.DeserializeObject<Contact>(entity.Properties["ContactJSON"].StringValue));
                    e.Venue.Clone(JsonConvert.DeserializeObject<Address>(entity.Properties["VenueJSON"].StringValue));

                    var arrangementsJSON = entity.Properties["ArrangementsJSON"].StringValue;
                    if (!string.IsNullOrEmpty(arrangementsJSON))
                    {
                        var arrangements = JsonConvert.DeserializeObject<IEnumerable<Arrangement>>(arrangementsJSON);
                        foreach(var a in arrangements)
                        {
                            var aEntity = new ArrangementEntity(a.Id);
                            a.Clone(aEntity.FetchById().Result);
                            e.Arrangements.Add(a);
                        }
                    }
                    list.Add(e);
                }
            }
            return list;
        }

        internal async override Task<IEvent> Save()
        {
            foreach (var a in Arrangements)
            {
                var aEntity = new ArrangementEntity(a);
                await aEntity.Save();
            }
            return await base.Save();
        }

        internal async override Task<int> Delete()
        {
            if(Arrangements == null)
            {
                //Try fetching from database.
                IEvent e = await FetchById();
                Arrangements = e.Arrangements;
            }
            if (Arrangements != null)
            {
                foreach (var a in Arrangements)
                {
                    var aEntity = new ArrangementEntity(a.Id);
                    await aEntity.Delete();
                }
            }
            return await base.Delete();
        }
    }
}
