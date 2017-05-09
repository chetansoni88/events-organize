using Core.Business;
using Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Core.Api.Controllers
{
    public class UserRequest
    {
        public Contact Contact { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }

        public VendorType VendorType { get; set; }

        public bool IsVendor { get; set; }

        public void CopyRequestToUser(IUser user)
        {
            if (user.Contact != null && Contact != null)
                user.Contact.Clone(Contact);
            user.Id = Id;
            user.Name = Name;
            user.Username = Username;
            user.Password = Password;
        }
    }

    public class UserController : ApiController
    {
        [TokenAuthentication]
        public async Task<HttpResponseMessage> Get(Guid id)
        {
            if (id != Guid.Empty)
            {
                var ep = new UserProcessor(id);
                var e = await ep.FetchById();
                if (e.Data != null)
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { Name = e.Data.Name, Contact = e.Data.Contact }))
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
        public async Task<HttpResponseMessage> Create(UserRequest req)
        {
            dynamic proc = GetSaveProcessor(req);

            var res = await proc.Create();
            if (res != null)
            {
                if (res.Success)
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { Id = res.Data.Id.ToString() }))
                    };
                }
                else if (res.FailureReason.ToLower().Contains("username"))
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("Username exists"),
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [TokenAuthentication]
        public async Task<HttpResponseMessage> Update(UserRequest req)
        {
            dynamic ep = GetSaveProcessor(req);
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
        [TokenAuthentication]
        public async Task<HttpResponseMessage> Delete(UserRequest req)
        {
            if (req.Id != Guid.Empty)
            {
                dynamic ep = GetProcessorById(req);
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
        public async Task<HttpResponseMessage> Login(UserRequest req)
        {
            dynamic ep = GetSaveProcessor(req);
            dynamic res = await ep.Login();
            if (res != null && res.Success)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new { TokenId = res.Data }))
                };
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }

        private dynamic GetSaveProcessor(UserRequest req)
        {
            dynamic proc = null;
            if (!req.IsVendor)
            {
                IUser u = new User();
                req.CopyRequestToUser(u);
                proc = new UserProcessor(u);
            }
            else
            {
                IVendor v = VendorBase.GetVendorFromType(req.VendorType);
                req.CopyRequestToUser(v);
                proc = new VendorProcessor(v);
            }
            return proc;
        }

        private dynamic GetProcessorById(UserRequest req)
        {
            dynamic proc = null;
            if (!req.IsVendor)
            {
                proc = new UserProcessor(req.Id);
            }
            else
            {
                proc = new VendorProcessor(req.Id);
            }
            return proc;
        }
    }
}
