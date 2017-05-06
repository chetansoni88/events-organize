using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Business;
using Core.Models;
using Newtonsoft.Json;

namespace Core.Api.Controllers
{
    public class ProjectRequest
    {
        public List<EventRequest> Events { get; set; }
        public Contact Contact { get; set; }
        public string Name { get; set; }
        public Address Venue { get; set; }
        public Guid Id { get; set; }
        public void CopyRequestToProject(IProject p)
        {
            p.Id = Id;
            p.Name = Name;
            if (Events != null)
            {
                foreach (var e in Events)
                {
                    IEvent ev = EventBase.GetEventFromType(e.Type);
                    e.CopyRequestToEvent(ev);
                    p.Events.Add(ev);
                }

            }
        }
    }

    public class ProjectController : ApiController
    {
        public async Task<HttpResponseMessage> Get(Guid id)
        {
            if (id != Guid.Empty)
            {
                var ep = new ProjectProcessor(id);
                var e = await ep.FetchById();
                if (e.Data != null)
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(e.Data))
                    };
                }
                else
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
                }
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Create(ProjectRequest req)
        {
            IProject p = new Project();
            req.CopyRequestToProject(p);
            var ep = new ProjectProcessor(p);
            var res = await ep.Create();
            if (res != null && res.Success)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new { Id = res.Data.Id.ToString() }))
                };
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Update(ProjectRequest req)
        {
            IProject p = new Project();
            req.CopyRequestToProject(p);
            var ep = new ProjectProcessor(p);
            var res = await ep.Update();
            if (res != null && res.Success)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new { Id = res.Data.Id.ToString() }))
                };
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Delete(ProjectRequest req)
        {
            if (req.Id != Guid.Empty)
            {
                var ep = new ProjectProcessor(req.Id);
                var e = await ep.Delete();
                if (e > 0)
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(Convert.ToBoolean(e).ToString()),
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                }
                else
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
                }
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddEvents(ProjectRequest req)
        {
            if (req.Id != Guid.Empty)
            {
                if (req.Events != null && req.Events.Any())
                {
                    var ep = new ProjectProcessor(req.Id);
                    List<IEvent> list = new List<IEvent>();
                    foreach (var ar in req.Events)
                    {
                        IEvent e = EventBase.GetEventFromType(ar.Type);
                        ar.CopyRequestToEvent(e);
                        list.Add(e);
                    }
                    await ep.AddEvents(list);

                    return new HttpResponseMessage()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                }
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> DeleteEvents(ProjectRequest req)
        {
            if (req.Id != Guid.Empty)
            {
                if (req.Events != null && req.Events.Any())
                {
                    var ep = new ProjectProcessor(req.Id);
                    List<IEvent> list = new List<IEvent>();
                    foreach (var ar in req.Events)
                    {
                        IEvent e = EventBase.GetEventFromType(ar.Type);
                        e.Id = ar.Id;
                        list.Add(e);
                    }
                    await ep.DeleteEvents(list);
                    return new HttpResponseMessage()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                }
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }

    }
}
