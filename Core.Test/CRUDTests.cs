using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Data;
using Core.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Test
{
    [TestClass]
    public class CRUDTest
    {
        #region MockData

        private IUser CreateUser()
        {
            IUser user = new User();
            user.Name = "AAA";
            user.Username = "user1";
            user.Password = "admin";
            user.Contact.Clone(CreateContact());
            return user;
        }

        private IVendor CreateVendor(VendorType type)
        {
            IVendor vendor = null;
            switch (type)
            {
                case VendorType.Photographer:
                    vendor = new Photographer();
                    break;
                case VendorType.Florist:
                    vendor = new Florist();
                    break;
                case VendorType.Caterer:
                    vendor = new Caterer();
                    break;
                case VendorType.Videographer:
                    vendor = new Videographer();
                    break;
            }
            vendor.Name = "AAA";
            vendor.Username = "user1";
            vendor.Password = "admin";
            vendor.Contact.Clone(CreateContact());
            return vendor;
        }

        private IEvent CreateEvent(EventType type)
        {
            IEvent e = null;
            switch (type)
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

            e.Name = "Wedding Sample";
            e.StartTime = DateTime.Today.AddMonths(1);
            e.EndTime = e.StartTime.AddHours(6);
            e.Contact.Clone(CreateContact());
            e.Venue.Clone(e.Contact.Address);
            var a = CreateArrangement();
            IVendor vendor = CreateVendor(VendorType.Photographer);

            DataEntityHelper<IVendor> h = new DataEntityHelper<IVendor>(vendor);
            var s = h.Save().Result;

            a.Vendor = s;
            e.Arrangements.Add(a);

            return e;
        }

        private IContact CreateContact()
        {
            IContact c = new Contact();
            c.Email = "ch_s@yahoo.com";
            c.Phone = "3254346322";
            c.Address.Street = "688 110th Ave";
            c.Address.State = "WA";
            c.Address.Country = "USA";
            c.Address.Zip = "98004";
            c.Address.City = "Bellevue";
            return c;
        }

        private IArrangement CreateArrangement()
        {
            var a = new Arrangement();
            a.StartTime = DateTime.Today.AddMonths(1);
            a.EndTime = a.StartTime.AddHours(2);
            a.Status = ArrangementStatus.ToBeStarted;
            return a;
        }

        private IProject CreateProject()
        {
            IProject p = new Project();
            p.Name = "Project Sample";
            p.Events.Add(CreateEvent(EventType.Corporate));
            return p;
        }
        #endregion

        [TestMethod]
        public void TestUserCRUD()
        {
            IUser user = CreateUser();

            DataEntityHelper<IUser> helper = new DataEntityHelper<IUser>(user);
            var save = helper.Save().Result;

            Assert.IsTrue(save != null && save.Id != Guid.Empty, "User save failed.");

            helper = new DataEntityHelper<IUser>(save.Id);
            var one = helper.FetchById().Result;

            Assert.IsTrue(one != null && JsonConvert.SerializeObject(one, Formatting.None).Equals(JsonConvert.SerializeObject(save, Formatting.None)), "User fetch failed.");

            var all = helper.FetchAll().Result;
            Assert.IsTrue(all.Count > 0, "User Fetch all failed.");

            var delete = helper.Delete().Result;
            one = helper.FetchById().Result;
            Assert.IsTrue(one == null, "User delete failed.");
        }

        [TestMethod]
        public void TestArrangementCRUD()
        {
            IArrangement a = CreateArrangement();

            DataEntityHelper<IArrangement> helper = new DataEntityHelper<IArrangement>(a);
            var save = helper.Save().Result;

            Assert.IsTrue(save != null && save.Id != Guid.Empty, "Arrangement save failed.");

            helper = new DataEntityHelper<IArrangement>(save.Id);
            var one = helper.FetchById().Result;

            Assert.IsTrue(one != null, "Arrangement fetch failed.");

            var all = helper.FetchAll().Result;
            Assert.IsTrue(all.Count > 0, "Arrangement Fetch all failed.");

            var delete = helper.Delete().Result;
            one = helper.FetchById().Result;
            Assert.IsTrue(one == null, "Arrangement delete failed.");
        }

        [TestMethod]
        public void TestVendorCRUD()
        {
            IVendor vendor = CreateVendor(VendorType.Photographer);

            DataEntityHelper<IVendor> helper = new DataEntityHelper<IVendor>(vendor);
            var save = helper.Save().Result;

            Assert.IsTrue(save != null && save.Id != Guid.Empty, "Vendor save failed.");

            helper = new DataEntityHelper<IVendor>(save.Id);
            var one = helper.FetchById().Result;

            Assert.IsTrue(one != null && JsonConvert.SerializeObject(one, Formatting.None).Equals(JsonConvert.SerializeObject(save, Formatting.None)), "Vendor fetch failed.");

            var uHelper = new DataEntityHelper<IUser>(save.Id);
            var uOne = uHelper.FetchById().Result;
            Assert.IsTrue(uOne != null, "User not populated from vendor save.");

            var all = helper.FetchAll().Result;
            Assert.IsTrue(all.Count > 0, "Vendor Fetch all failed.");

            var delete = helper.Delete().Result;
            one = helper.FetchById().Result;
            Assert.IsTrue(one == null, "Vendor delete failed.");

            uOne = uHelper.FetchById().Result;
            Assert.IsTrue(uOne == null, "User not deleted from vendor delete.");
        }

        [TestMethod]
        public void TestEventCRUD()
        {
            IEvent e = CreateEvent(EventType.Wedding);
            DataEntityHelper<IEvent> helper = new DataEntityHelper<IEvent>(e);
            var save = helper.Save().Result;

            Assert.IsTrue(save != null && save.Id != Guid.Empty, "Event save failed.");

            helper = new DataEntityHelper<IEvent>(save.Id);
            var one = helper.FetchById().Result;

            Assert.IsTrue(one != null, "Event fetch failed.");

            var all = helper.FetchAll().Result;
            Assert.IsTrue(all.Count > 0, "Event fetch all failed.");

            var delete = helper.Delete().Result;
            one = helper.FetchById().Result;
            Assert.IsTrue(one == null, "Event delete failed.");

            DataEntityHelper<IVendor> h = new DataEntityHelper<IVendor>(e.Arrangements[0].Vendor.Id);
            var d = h.Delete().Result;
            var o = h.FetchById().Result;
            Assert.IsTrue(o == null, "Vendor delete failed.");

            var uHelper = new DataEntityHelper<IUser>(e.Arrangements[0].Vendor.Id);
            var uOne = uHelper.FetchById().Result;
            Assert.IsTrue(uOne == null, "User not deleted from vendor delete.");
        }

        [TestMethod]
        public void TestProjectCRUD()
        {
            IProject p = CreateProject();

            DataEntityHelper<IProject> helper = new DataEntityHelper<IProject>(p);
            var save = helper.Save().Result;

            Assert.IsTrue(save != null && save.Id != Guid.Empty, "Project save failed.");

            helper = new DataEntityHelper<IProject>(save.Id);
            var one = helper.FetchById().Result;

            Assert.IsTrue(one != null, "Project fetch failed.");

            DataEntityHelper<IEvent> eHelper = new DataEntityHelper<IEvent>(p.Events[0].Id);
            var eOne = eHelper.FetchById().Result;

            Assert.IsTrue(eOne != null, "Event fetch failed.");

            var all = helper.FetchAll().Result;
            Assert.IsTrue(all.Count > 0, "Project Fetch all failed.");

            var delete = helper.Delete().Result;
            one = helper.FetchById().Result;
            Assert.IsTrue(one == null, "Project delete failed.");

            eOne = eHelper.FetchById().Result;
            Assert.IsTrue(eOne == null, "Event delete failed from Project.");

            var vhelper = new DataEntityHelper<IVendor>(p.Events[0].Arrangements[0].Vendor.Id);
            delete = vhelper.Delete().Result;

            var vendor = vhelper.FetchById().Result;
            Assert.IsTrue(one == null, "Vendor delete failed.");

            var uHelper = new DataEntityHelper<IUser>(p.Events[0].Arrangements[0].Vendor.Id);
            var user = uHelper.FetchById().Result;
            Assert.IsTrue(user == null, "User not deleted from vendor delete.");
        }
    }
}
