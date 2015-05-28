using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using task_1.Models.ProfileModel;
using System.Linq;
using task_1.Models;

namespace SiteTest
{
    /// <summary>
    /// Сводное описание для ProfileDatabaseTests
    /// </summary>
    [TestClass]
    public class ProfileDatabaseTests
    {
        public ProfileDatabaseTests()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Дополнительные атрибуты тестирования
        //
        // При написании тестов можно использовать следующие дополнительные атрибуты:
        //
        // ClassInitialize используется для выполнения кода до запуска первого теста в классе
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // TestInitialize используется для выполнения кода перед запуском каждого теста 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // TestCleanup используется для выполнения кода после завершения каждого теста
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //[TestMethod]
        //public void ProfileShouldReturnAllEmailsByName()
        //{
        //    var profiles = new ProfileModelContext();
        //    var emails = new List<Emails>();
        //    emails.Add(new Emails() { Email = "1@mail.com" });
        //    emails.Add(new Emails() { Email = "2@mail.com" });
        //    var profile = new Profile() { Emails = emails, UserName = "Test" };
        //    profiles.Profile.Add(profile);
        //    profiles.SaveChanges();
        //    profile = profiles.Profile.SingleOrDefault(n => n.UserName == "Test");
        //    var query = from e in profile.Emails select e.Email;
        //    Assert.IsTrue(query.Contains("1@mail.com"));
        //    Assert.IsTrue(query.Contains("2@mail.com"));
        //    profiles.Profile.Remove(profile);
        //    profiles.SaveChanges();
        //}

        //[TestMethod]
        //public void ProfileShouldReturnAllusinessCardsById()
        //{
        //    var profiles = new ProfileModelContext();
        //    List<BusinessCard> cards = new List<BusinessCard>();
        //    cards.Add(new BusinessCard() { Template = "Empty" });
        //    cards.Add(new BusinessCard() { Template = "Not empty" });
        //    var profile = new Profile() { BusinessCards = cards, UserName = "Test" };
        //    profiles.Profile.Add(profile);
        //    profiles.SaveChanges();
        //    profile = profiles.Profile.SingleOrDefault(n => n.UserName == "Test");
        //    var query = from e in profile.BusinessCards select e.Template;
        //    Assert.IsTrue(query.Contains("Empty"));
        //    Assert.IsTrue(query.Contains("Not empty"));
        //    profiles.Profile.Remove(profile);
        //    profiles.SaveChanges();
        //}

        //[TestMethod]
        //public void BusinessCardShouldBeAbleToLinkToExistingEmails()
        //{
        //    var profiles = new ProfileModelContext();
        //    List<Emails> emails = new List<Emails>();
        //    emails.Add(new Emails() { Email = "1@test.com" });
        //    emails.Add(new Emails() { Email = "2@test.com" });
        //    List<BusinessCard> cards = new List<BusinessCard>();
        //    cards.Add(new BusinessCard() { Template = "Empty", Url = "Test/card1"});
        //    var profile = new Profile() { Emails = emails, BusinessCards = cards, UserName = "Test" };
        //    profiles.Profile.Add(profile);
        //    profiles.SaveChanges();
        //    profile = profiles.Profile.SingleOrDefault(n => n.UserName == "Test");
        //    var card = profile.BusinessCards.SingleOrDefault(c => c.Url == "Test/card1");
        //    card.Emails.Add(profile.Emails.SingleOrDefault(e => e.Email == "1@test.com"));
        //    card.Emails.Add(profiles.Emails.SingleOrDefault(e => e.Email == "2@test.com"));
        //    profiles.SaveChanges();
        //    profile = profiles.Profile.SingleOrDefault(n => n.UserName == "Test");
        //    card = profile.BusinessCards.SingleOrDefault(c => c.Url == "Test/card1");
        //    var query = from e in card.Emails select e.Email;
        //    Assert.IsTrue(query.Contains("1@test.com"));
        //    Assert.IsTrue(query.Contains("2@test.com"));
        //}
    }
}
