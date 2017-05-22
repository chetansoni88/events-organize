using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Models;
using Core.Business;
using Newtonsoft.Json;

namespace Core.Test
{
    [TestClass]
    public class CRUDTest
    {
        #region MockData

        private IUser CreateUser()
        {
            IUser user = new User()
            {
                Name = "AAA",
                Username = Guid.NewGuid().ToString(),
                Password = "admin"
            };
            user.Contact.Clone(CreateContact());
            return user;
        }

        private IVendor CreateVendor(VendorType type)
        {
            IVendor vendor = VendorBase.GetVendorFromType(type);
            vendor.Name = "AAA";
            vendor.Username = "user1";
            vendor.Password = "admin";
            vendor.Contact.Clone(CreateContact());
            return vendor;
        }

        private IEvent CreateEvent(EventType type)
        {
            IEvent e = EventBase.GetEventFromType(type);
            e.Name = "Wedding Sample";
            e.StartTime = DateTime.Today.AddMonths(1);
            e.EndTime = e.StartTime.AddHours(6);
            e.Contact.Clone(CreateContact());
            e.Venue.Clone(e.Contact.Address);
            var a = CreateArrangement();
            IVendor vendor = CreateVendor(VendorType.Photographer);

            var h = new VendorProcessor(vendor);
            var s = h.Create().Result;

            a.Vendor = s.Data;
            e.Arrangements.Add(a);

            return e;
        }

        private IContact CreateContact()
        {
            IContact c = new Contact()
            {
                Email = "ch_s@yahoo.com",
                Phone = "3254346322"
            };
            c.Address.Street = "688 110th Ave";
            c.Address.State = "WA";
            c.Address.Country = "USA";
            c.Address.Zip = "98004";
            c.Address.City = "Bellevue";
            return c;
        }

        private IArrangement CreateArrangement()
        {
            var a = new Arrangement()
            {
                StartTime = DateTime.Today.AddMonths(1)
            };
            a.EndTime = a.StartTime.AddHours(2);
            a.Status = ArrangementStatus.ToBeStarted;
            return a;
        }

        private IProject CreateProject()
        {
            IProject p = new Project()
            {
                Name = "Project Sample"
            };
            p.Events.Add(CreateEvent(EventType.Corporate));
            return p;
        }
        #endregion

        [TestMethod]
        public void TestUserCRUD()
        {
            IUser user = CreateUser();

            UserProcessor up = new UserProcessor(user);
            var save = up.Create().Result;

            Assert.IsTrue(save != null && save.Data != null && save.Data.Id != Guid.Empty, "User save failed.");

            var one = up.FetchById().Result;
            one.Data.Password = "admin";
            Assert.IsTrue(one != null && JsonConvert.SerializeObject(one, Formatting.None).Equals(JsonConvert.SerializeObject(save, Formatting.None)), "User fetch failed.");

            var loggedIn = up.Login().Result;
            Assert.IsTrue(loggedIn != null && loggedIn.Data != Guid.Empty, "User login failed.");

            var delete = up.Delete().Result;
            one = up.FetchById().Result;
            Assert.IsTrue(one.Data == null, "User delete failed.");

            var tp = new TokenProcessor(loggedIn.Data);
            int result = tp.Delete().Result;
            var t = tp.FetchById().Result;
            Assert.IsTrue(t.Data == null, "Token delete failed.");
        }

        [TestMethod]
        public void TestArrangementCRUD()
        {
            IArrangement a = CreateArrangement();

            var ap = new ArrangementProcessor(a);
            var save = ap.Create().Result;

            Assert.IsTrue(save.Data != null && save.Data.Id != Guid.Empty, "Arrangement save failed.");

            var one = ap.FetchById().Result;

            Assert.IsTrue(one.Data != null, "Arrangement fetch failed.");

            var delete = ap.Delete().Result;
            one = ap.FetchById().Result;
            Assert.IsTrue(one.Data == null, "Arrangement delete failed.");
        }

        [TestMethod]
        public void TestVendorCRUD()
        {
            IVendor vendor = CreateVendor(VendorType.Photographer);

            var vp = new VendorProcessor(vendor);
            var save = vp.Create().Result;

            Assert.IsTrue(save.Data != null && save.Data.Id != Guid.Empty, "Vendor save failed.");

            var one = vp.FetchById().Result;
            one.Data.Password = "admin";
            Assert.IsTrue(one.Data != null && JsonConvert.SerializeObject(one.Data, Formatting.None).Equals(JsonConvert.SerializeObject(save.Data, Formatting.None)), "Vendor fetch failed.");

            var uHelper = new UserProcessor(save.Data.Id);
            var uOne = uHelper.FetchById().Result;
            Assert.IsTrue(uOne.Data != null, "User not populated from vendor save.");

            var loggedIn = vp.Login().Result;
            Assert.IsTrue(loggedIn != null && loggedIn.Data != Guid.Empty, "Vendor login failed.");

            var delete = vp.Delete().Result;
            one = vp.FetchById().Result;
            Assert.IsTrue(one.Data == null, "Vendor delete failed.");

            uOne = uHelper.FetchById().Result;
            Assert.IsTrue(uOne.Data == null, "User not deleted from vendor delete.");

            var tp = new TokenProcessor(loggedIn.Data);
            int result = tp.Delete().Result;
            var t = tp.FetchById().Result;
            Assert.IsTrue(t.Data == null, "Token delete failed.");
        }

        [TestMethod]
        public void TestEventCRUD()
        {
            IEvent e = CreateEvent(EventType.Wedding);
            var ep = new EventProcessor(e);
            var save = ep.Create().Result;

            Assert.IsTrue(save.Data != null && save.Data.Id != Guid.Empty, "Event save failed.");

            var one = ep.FetchById().Result;

            Assert.IsTrue(one.Data != null, "Event fetch failed.");

            var delete = ep.Delete().Result;
            one = ep.FetchById().Result;
            Assert.IsTrue(one.Data == null, "Event delete failed.");

            var vp = new VendorProcessor(e.Arrangements[0].Vendor.Id);
            var d = vp.Delete().Result;
            var o = vp.FetchById().Result;
            Assert.IsTrue(o.Data == null, "Vendor delete failed.");

            var uHelper = new UserProcessor(e.Arrangements[0].Vendor.Id);
            var uOne = uHelper.FetchById().Result;
            Assert.IsTrue(uOne.Data == null, "User not deleted from vendor delete.");
        }

        [TestMethod]
        public void TestProjectCRUD()
        {
            IProject p = CreateProject();

            var pp = new ProjectProcessor(p);
            var save = pp.Create().Result;

            Assert.IsTrue(save.Data != null && save.Data.Id != Guid.Empty, "Project save failed.");

            var one = pp.FetchById().Result;

            Assert.IsTrue(one.Data != null, "Project fetch failed.");

           var eHelper = new EventProcessor(p.Events[0].Id);
            var eOne = eHelper.FetchById().Result;

            Assert.IsTrue(eOne.Data != null, "Event fetch failed.");

            var delete = pp.Delete().Result;
            one = pp.FetchById().Result;
            Assert.IsTrue(one.Data == null, "Project delete failed.");

            eOne = eHelper.FetchById().Result;
            Assert.IsTrue(eOne.Data == null, "Event delete failed from Project.");

            var vhelper = new VendorProcessor(p.Events[0].Arrangements[0].Vendor.Id);
            delete = vhelper.Delete().Result;

            var vendor = vhelper.FetchById().Result;
            Assert.IsTrue(one.Data == null, "Vendor delete failed.");

            var uHelper = new UserProcessor(p.Events[0].Arrangements[0].Vendor.Id);
            var user = uHelper.FetchById().Result;
            Assert.IsTrue(user.Data == null, "User not deleted from vendor delete.");
        }
    }
}
