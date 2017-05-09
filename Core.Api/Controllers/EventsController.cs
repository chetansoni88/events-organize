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

    public class EventRequest
    {
        public List<Arrangement> Arrangements { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public EventType Type { get; set; }
        public Contact Contact { get; set; }
        public string Name { get; set; }
        public Address Venue { get; set; }
        public Guid Id { get; set; }
        public void CopyRequestToEvent(IEvent e)
        {
            e.Id = Id;
            e.Contact.Clone(Contact);
            e.StartTime = StartTime;
            e.EndTime = EndTime;
            e.Name = Name;
            e.Venue.Clone(Venue);
            if (Arrangements != null)
                e.Arrangements.AddRange(Arrangements);
        }
    }

    [TokenAuthentication]
    public class EventController : ApiController
    {
        public async Task<HttpResponseMessage> Get(Guid id)
        {
            if (id != Guid.Empty)
            {
                var ep = new EventProcessor(id);
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
        public async Task<HttpResponseMessage> Create(EventRequest req)
        {
            IEvent e = EventBase.GetEventFromType(req.Type);
            if (e != null)
            {
                req.CopyRequestToEvent(e);
                var ep = new EventProcessor(e);
                var res = await ep.Create();
                if (res != null && res.Success)
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { Id = res.Data.Id.ToString() }))
                    };
                }
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Update(EventRequest req)
        {
            IEvent e = EventBase.GetEventFromType(req.Type);
            if (e != null)
            {
                req.CopyRequestToEvent(e);
                var ep = new EventProcessor(e);
                var res = await ep.Update();
                if (res != null && res.Success)
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { Id = res.Data.Id.ToString() }))
                    };
                }
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Delete(EventRequest req)
        {
            if (req.Id != Guid.Empty)
            {
                var ep = new EventProcessor(req.Id);
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
        public async Task<HttpResponseMessage> AddArrangements(EventRequest req)
        {
            if (req.Id != Guid.Empty)
            {
                if (req.Arrangements != null && req.Arrangements.Any())
                {
                    var ep = new EventProcessor(req.Id);
                    List<IArrangement> list = new List<IArrangement>();
                    foreach (var ar in req.Arrangements)
                    {
                        list.Add(ar);
                    }
                    await ep.AddArrangements(list);

                    return new HttpResponseMessage()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                }
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> DeleteArrangements(EventRequest req)
        {
            if (req.Id != Guid.Empty)
            {
                if (req.Arrangements != null && req.Arrangements.Any())
                {
                    var ep = new EventProcessor(req.Id);
                    List<IArrangement> list = new List<IArrangement>();
                    foreach (var ar in req.Arrangements)
                    {
                        list.Add(ar);
                    }
                    await ep.DeleteArrangements(list);
                    return new HttpResponseMessage()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                }
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateArrangements(EventRequest req)
        {
            if (req.Arrangements != null && req.Arrangements.Any())
            {
                foreach(var ar in req.Arrangements)
                {
                    var ep = new ArrangementProcessor(ar);
                    await ep.Update();
                }

                return new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }

    }
}
