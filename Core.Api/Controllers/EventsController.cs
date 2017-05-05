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
    public class EventsController : ApiController
    {
        public async Task<HttpResponseMessage> Get(Guid id)
        {
            var ep = new EventProcessor(id);
            var e = await ep.FetchById();
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(e.Data))
            };
        }

        public async Task<HttpResponseMessage> Post(IEvent e)
        {
            var ep = new EventProcessor(e);
            var res = await ep.Create();
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { Id = res.Data.Id.ToString() }))
            };
        }
    }
}
