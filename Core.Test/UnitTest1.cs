using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Data;
using Core.Models;
using System.Threading.Tasks;

namespace Core.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSave()
        {
            IUser user = new User();
            user.Id = Guid.NewGuid();
            user.Name = "AAA";
            user.Username = "user1";
            user.Password = "admin";
            user.Contact.Email = "ch_s@yahoo.com";
            user.Contact.Phone = "3254346322";
            user.Contact.Address.Street = "688";

            DataEntityHelper<IUser> helper = new DataEntityHelper<IUser>(user);
            var save = helper.Save().Result;

            helper = new DataEntityHelper<IUser>(save);
            var one = helper.FetchById().Result;

            var all = helper.FetchAll().Result;

            var delete = helper.Delete().Result;
        }
    }
}
