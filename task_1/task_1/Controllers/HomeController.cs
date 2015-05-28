using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using task_1.Models;
using Microsoft.AspNet.Identity;
using task_1.Models.ProfileModel;
using System.IO;

namespace task_1.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(SearchRequest request)
        {
            //TODO: search request processing
            return View();
        }

        [Authorize(Roles="Admin")]
        public ActionResult AdminOnly()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult UserProfile()
        {
            var context = new ProfileModelContext();
            var profile = context.Profile.SingleOrDefault(x => x.UserName == User.Identity.Name);
            if (profile == null)
            {
                profile = new Profile() { UserName = User.Identity.Name };
                context.Profile.Add(profile);
                context.SaveChanges();
            }
            return View(profile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UserProfile(Profile profile)
        {
            return View(profile);
        }

        [HttpGet]
        [Authorize]
        public ActionResult BusinessCardRedactor()
        {
            var context = new ProfileModelContext();
            var profile = context.Profile.SingleOrDefault(x => x.UserName == User.Identity.Name);
            return View(profile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult BusinessCardRedactor(string template)
        {
            var context = new ProfileModelContext();
            var profile = context.Profile.SingleOrDefault(x => x.UserName == User.Identity.Name);
            profile.BusinessCards.Add(new BusinessCard { Url = "Raw", Template = template });
            return View(profile);
        }


        [HttpGet]
        [Authorize]
        public ActionResult AddField()
        {
            var context = new ProfileModelContext();
            var profile = context.Profile.SingleOrDefault(x => x.UserName == User.Identity.Name);
            profile.Fields.Add(new Field());
            return View("UserProfile", profile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditFields(ICollection<Field> fields)
        {
            var context = new ProfileModelContext();
            var profile = context.Profile.SingleOrDefault(x => x.UserName == User.Identity.Name);
            if (fields != null)
            {
                while (profile.Fields.Count != fields.Count)
                {
                    profile.Fields.Add(new Field());
                }
                for (int i = 0; i < fields.Count; i++)
                {
                    if (fields.ElementAt(i).Value == null)
                    {
                        context.Fields.Remove(profile.Fields.ElementAt(i));
                    }
                    else
                    {
                        profile.Fields.ElementAt(i).Type = fields.ElementAt(i).Type;
                        profile.Fields.ElementAt(i).Value = fields.ElementAt(i).Value;
                    }
                }
                context.SaveChanges();
            }
            return View("UserProfile", profile);
        }

        [HttpPost]
        [Authorize]
        public void SaveBusinessCard(string template, string fields, int cardId)
        {
            var context = new ProfileModelContext();
            var profile = context.Profile.SingleOrDefault(x => x.UserName == User.Identity.Name);
            BusinessCard businessCard = null;
            if (cardId < 0)
            {
                businessCard = new BusinessCard() { Url = "Raw", Template = template };
                profile.BusinessCards.Add(businessCard);
                context.SaveChanges();
                profile = context.Profile.SingleOrDefault(x => x.UserName == User.Identity.Name);
                businessCard = profile.BusinessCards.SingleOrDefault(x => x.Url == "Raw");
                businessCard.Url = "https://localhost:44300/Home/BusinessCardStorage?userName=" +
                    User.Identity.Name + "&businessCardId=" + businessCard.Id.ToString();
            }
            else
            {
                businessCard = profile.BusinessCards.SingleOrDefault(x => x.Id == cardId);
                foreach (var line in context.BusinessCardsToFields)
                {
                    if (line.BusinessCardId == businessCard.Id)
                    {
                        context.BusinessCardsToFields.Remove(line);
                    }
                }
            }
            var separateFields = fields.Split(new char[] { ',' });
            for (int i = 0; i < separateFields.Length && i < 8; i++)
            {
                if (separateFields[i] != " ")
                {
                    context.BusinessCardsToFields.Add(
                        new BusinessCardToField()
                        {
                            FieldId = profile.Fields.SingleOrDefault(x => x.Value == separateFields[i]).Id,
                            BusinessCardId = businessCard.Id
                        });
                }
                else
                {
                    context.BusinessCardsToFields.Add(
                        new BusinessCardToField()
                        {
                            FieldId = -1,
                            BusinessCardId = businessCard.Id
                        });
                }
            }
            context.SaveChanges();
        }

        [HttpPost]
        [Authorize]
        public void DeleteBusinessCard(int cardId)
        {
            var context = new ProfileModelContext();
            context.BusinessCards.Remove(context.BusinessCards.SingleOrDefault(x => x.Id == cardId));
            var cardFields = from line in context.BusinessCardsToFields
                             where line.BusinessCardId == cardId
                             select line;
            foreach (var cf in cardFields)
            {
                context.BusinessCardsToFields.Remove(cf);
            }
            context.SaveChanges();
        }

        [HttpGet]
        public ActionResult BusinessCardStorage(string userName, int businessCardId)
        {
            var context = new ProfileModelContext();
            var profile = context.Profile.SingleOrDefault(x => x.UserName == userName);
            var businessCard = profile.BusinessCards.Count(x => x.Id == businessCardId);
            if (businessCard != 0)
            {
                profile.BusinessCards.SingleOrDefault(x => x.Id == businessCardId).Url = "Current";
                var fields = profile.Fields;
                var currentBusinessCardFieldsIds = from a in context.BusinessCardsToFields
                                                   where a.BusinessCardId == businessCardId
                                                   select a.FieldId;
                List<Field> collectionOfFields = new List<Field>();
                foreach (var id in currentBusinessCardFieldsIds)
                {
                    if (id != -1)
                    {
                        collectionOfFields.Add(fields.SingleOrDefault(x => x.Id == id));
                    }
                    else
                    {
                        collectionOfFields.Add(new Field() { Type = " ", Value = " "});
                    }
                }
                profile.BusinessCards.SingleOrDefault(x => x.Id == businessCardId).Fields = collectionOfFields;
                if (userName == User.Identity.Name)
                {
                    profile.BusinessCards.SingleOrDefault(x => x.Id == businessCardId).Url = "Edit";
                    foreach (var f in profile.BusinessCards.SingleOrDefault(x => x.Id == businessCardId).Fields)
                    {
                        if (profile.Fields.Count(x => x.Id == f.Id) != 0)
                        {
                            profile.Fields.Remove(profile.Fields.SingleOrDefault(x => x.Id == f.Id));
                        }
                    }
                    return View("BusinessCardRedactor", profile);
                }
                else
                {
                    return View(profile);
                }
            }
            else
            {
                return Redirect("ErrorNotFound");
            }
        }

        [HttpGet]
        public ActionResult ErrorNotFound()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public void IncreaseRating(int id, int howMuch)
        {
            var context = new ProfileModelContext();
            if (context.BusinessCards.SingleOrDefault(x => x.Id == id).Rating + howMuch < 2000000000)
            {
                context.BusinessCards.SingleOrDefault(x => x.Id == id).Rating += howMuch;
                context.SaveChanges();
            }
        }
    }
}