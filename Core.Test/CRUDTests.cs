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
        [TestMethod]
        public void TestUserCRUD()
        {
            IUser user = new User();
            user.Name = "AAA";
            user.Username = "user1";
            user.Password = "admin";
            user.Contact.Email = "ch_s@yahoo.com";
            user.Contact.Phone = "3254346322";
            user.Contact.Address.Street = "688 110th Ave";
            user.Contact.Address.State = "WA";
            user.Contact.Address.Country = "USA";
            user.Contact.Address.Zip = "98004";
            user.Contact.Address.City = "Bellevue";

            DataEntityHelper<IUser> helper = new DataEntityHelper<IUser>(user);
            var save = helper.Save().Result;

            Assert.IsTrue(save != null && save.Id != Guid.Empty, "User save failed.");

            helper = new DataEntityHelper<IUser>(save.Id);
            var one = helper.FetchById().Result;

            Assert.IsTrue(one != null && JsonConvert.SerializeObject(one, Formatting.None).Equals(JsonConvert.SerializeObject(one, Formatting.None)), "User fetch failed.");

            var all = helper.FetchAll().Result;
            Assert.IsTrue(all.Count > 0, "User Fetch all failed.");

            var delete = helper.Delete().Result;
            one = helper.FetchById().Result;
            Assert.IsTrue(one == null, "User delete failed.");
        }

        [TestMethod]
        public void TestVendorCRUD()
        {
            IVendor vendor = new Photographer();
            vendor.Name = "AAA";
            vendor.Username = "user1";
            vendor.Password = "admin";
            vendor.Contact.Email = "ch_s@yahoo.com";
            vendor.Contact.Phone = "3254346322";
            vendor.Contact.Address.Street = "688 110th Ave";
            vendor.Contact.Address.State = "WA";
            vendor.Contact.Address.Country = "USA";
            vendor.Contact.Address.Zip = "98004";
            vendor.Contact.Address.City = "Bellevue";

            DataEntityHelper<IVendor> helper = new DataEntityHelper<IVendor>(vendor);
            var save = helper.Save().Result;

            Assert.IsTrue(save != null && save.Id != Guid.Empty, "Vendor save failed.");

            helper = new DataEntityHelper<IVendor>(save.Id);
            var one = helper.FetchById().Result;

            Assert.IsTrue(one != null && JsonConvert.SerializeObject(one, Formatting.None).Equals(JsonConvert.SerializeObject(one, Formatting.None)), "Vendor fetch failed.");

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
    }
}
