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
    internal class ProjectEntity : TableEntityBase<IProject>, IProject
    {
        public ProjectEntity(Guid id) : base(id)
        {

        }

        public ProjectEntity(IProject model) : base(model)
        {

        }

        internal override string TableName => "projects";

        public string Name { get; set; }

        public List<IEvent> Events
        {
            get;
            private set;
        }

        private string _eventsJSON = string.Empty;
        public string EventsJSON
        {
            get
            {
                if (Events != null && Events.Count > 0 && string.IsNullOrEmpty(_eventsJSON))
                {
                    var newList = Events.Select(a => { return new { Id = a.Id }; });
                    _eventsJSON = JsonConvert.SerializeObject(newList, Formatting.None);
                }
                return _eventsJSON;
            }
            set
            {
                _eventsJSON = value;
            }
        }


        internal override IProject ConvertToModel()
        {
            IProject e = null;
            e.Id = Id;
            e.Name = Name;
            e.Events.AddRange(Events);
            return e;
        }
        internal override void PopulateFromModel(IProject model)
        {
            Id = model.Id;
            Name = model.Name;
            Events = new List<IEvent>();
            Events.AddRange(model.Events);
        }
        internal override List<IProject> ExtractModels(List<DynamicTableEntity> entities)
        {
            var list = new List<IProject>();
            foreach (var entity in entities)
            {
                IProject e = new Project();
                e.Id = entity.Properties["Id"].GuidValue.Value;
                e.Name = entity.Properties["Name"].StringValue;
                var eventsJSON = entity.Properties["EventsJSON"].StringValue;
                if (!string.IsNullOrEmpty(eventsJSON))
                {
                    var events = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(eventsJSON);
                    foreach (var a in events)
                    {
                        var aEntity = new EventEntity(Guid.Parse(a.Id.Value));
                        var ev = aEntity.FetchById().Result;
                        e.Events.Add(ev);
                    }
                }
                list.Add(e);
            }

            return list;
        }

        internal async override Task<IProject> Save()
        {
            PopulateFromModel(Model);
            foreach (var a in Events)
            {
                var aEntity = new EventEntity(a);
                await aEntity.Save();
            }
            return await base.Save();
        }

        internal async override Task<int> Delete()
        {
            if (Events == null)
            {
                //Try fetching from database.
                IProject e = await FetchById();
                Events = e.Events;
            }
            if (Events != null)
            {
                foreach (var a in Events)
                {
                    var aEntity = new EventEntity(a.Id);
                    await aEntity.Delete();
                }
            }
            return await base.Delete();
        }
    }
}
