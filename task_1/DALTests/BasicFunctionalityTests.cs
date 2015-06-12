using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using task_1.Models.ProfileModel;
using System.Collections.Generic;

namespace DALTests
{
    [TestClass]
    public class BasicFunctionalityTests
    {
        [TestMethod]
        public void CanCreateDAL()
        {
            var dal = new DAL();
            Assert.IsNotNull(dal);
        }

        [TestMethod]
        public void CanGetAllProfiles()
        {
            var dal = new DAL();
            Assert.IsInstanceOfType(dal.Profiles, typeof(List<Profile>));
            Assert.IsNotNull(dal.Profiles);
        }
    }
}
