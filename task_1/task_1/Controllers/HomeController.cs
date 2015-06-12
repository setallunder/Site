using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using task_1.Models;
using Microsoft.AspNet.Identity;
using task_1.Models.ProfileModel;
using System.IO;
using task_1.Filters;
using System.Net;
using System.Text;

namespace task_1.Controllers
{
    [Culture]
    [RequireHttps]
    public class HomeController : Controller
    {
        pdfcrowd.Client clientPDF = new pdfcrowd.Client("sanyadovskiy", "d7422b94589309fbfab29d6a69fdf10c");

        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;

            List<string> cultures = new List<string>() { "ru", "en" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }

            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;
            else
            {

                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles="Admin")]
        [HttpGet]
        public ActionResult AdminOnly()
        {
            using (var DAL = new DAL())
            {
                return View(DAL.GetAllProfiles());
            }
        }

        [Authorize(Roles="Admin")]
        [HttpPost]
        public ActionResult DeleteUser(int userId)
        {
            using (var DAL = new DAL())
            {
                var context = new ApplicationDbContext();
                var profile = context.Users.SingleOrDefault(x => x.UserName == DAL.GetProfile(userId).UserName);
                context.Users.Remove(profile);
                context.SaveChanges();
                DAL.DeleteProfile(userId);
                return RedirectToAction("AdminOnly");
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult UserProfile()
        {
            using (var DAL = new DAL())
            {
                var profile = DAL.GetProfile(User.Identity.Name);
                if (profile == null)
                {
                    DAL.AddProfile(User.Identity.Name);
                    profile = DAL.GetProfile(User.Identity.Name);
                }
                return View(Tuple.Create(profile.BusinessCards, profile.Fields));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult UserProfile(Profile profile)
        {
            return View(Tuple.Create(profile.BusinessCards, profile.Fields));
        }

        [HttpGet]
        [Authorize]
        public ActionResult BusinessCardRedactor()
        {
            using (var DAL = new DAL())
            {
                return View(Tuple.Create(
                    User.Identity.Name, 
                    new BusinessCard() { Id = -1, Template = "Empty" }, 
                    DAL.GetProfile(User.Identity.Name).Fields));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult BusinessCardRedactor(string template)
        {
            using (var DAL = new DAL())
            {
                return View(Tuple.Create(
                    User.Identity.Name,
                    new BusinessCard() { Id = -1, Template = template },
                    DAL.GetProfile(User.Identity.Name).Fields));
            }
        }


        [HttpGet]
        [Authorize]
        public ActionResult AddField()
        {
            using (var DAL = new DAL())
            {
                var profile = DAL.GetProfile(User.Identity.Name);
                profile.Fields.Add(new Field());
                return View("UserProfile", Tuple.Create(profile.BusinessCards, profile.Fields));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditFields(ICollection<Field> fields)
        {
            using (var DAL = new DAL())
            {
                DAL.UpdateProfileFields(User.Identity.Name, fields);
                var profile = DAL.GetProfile(User.Identity.Name);
                return View("UserProfile", Tuple.Create(profile.BusinessCards, profile.Fields));
            }
        }

        [HttpPost]
        [Authorize]
        public void SaveBusinessCard(string template, string fields, int cardId, string userName, string coordinates)
        {
            using (var DAL = new DAL())
            {
                var profile = DAL.GetProfile(userName);
                if (cardId < 0)
                {
                    var businessCard = new BusinessCard() { Url = "Raw", Template = template };
                    DAL.AddBusinessCardToUser(userName, businessCard);
                    businessCard = DAL.GetBusinessCardByUrl("Raw");
                    DAL.SetBusinessCardUrl(businessCard.Id, "https://localhost:44300/Home/BusinessCardStorage?userName=" +
                        User.Identity.Name + "&businessCardId=" + businessCard.Id.ToString());
                    var separateFields = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var fieldsCoordinates = coordinates.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    DAL.AddFieldsToBusinessCard(userName, businessCard.Id, separateFields, fieldsCoordinates);
                }
                else
                {
                    DAL.RemoveAllFieldsFromBusinessCard(cardId);
                    var separateFields = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var fieldsCoordinates = coordinates.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    DAL.AddFieldsToBusinessCard(userName, cardId, separateFields, fieldsCoordinates);
                }
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteBusinessCard(int cardId)
        {
            using (var DAL = new DAL())
            {
                DAL.DeleteBusinessCard(cardId);
                return RedirectToAction("UserProfile");
            }
        }

        [HttpGet]
        public ActionResult BusinessCardStorage(string userName, int businessCardId)
        {
            using(var DAL = new DAL())
            {
                var businessCard = DAL.GetBusinessCard(businessCardId);
                if (businessCard != null)
                {
                    var links = DAL.GetAllBusinessCardToFieldsLinks(businessCardId);
                    List<Field> collectionOfFields = new List<Field>();
                    foreach (var link in links)
                    {
                        var field = DAL.GetField(link.FieldId);
                        field.OffsetTop = link.OffsetTop;
                        field.OffsetLeft = link.OffsetLeft;
                        collectionOfFields.Add(field);
                    }
                    businessCard.Fields = collectionOfFields;
                    if (userName == User.Identity.Name || User.IsInRole("Admin"))
                    {
                        var fields = DAL.GetAllProfileFields(userName);
                        foreach (var f in businessCard.Fields)
                        {
                            fields.Remove(fields.SingleOrDefault(x => x.Id == f.Id));
                        }
                        return View("BusinessCardRedactor", Tuple.Create(userName, businessCard, fields));
                    }
                    else
                    {
                        return View(Tuple.Create(userName, businessCard));
                    }
                }
                else
                {
                    return Redirect("ErrorNotFound");
                }
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
            using (var DAL = new DAL())
            {
                DAL.IncreaseCardRating(id, howMuch);
            }
        }

        [HttpGet]
        public ActionResult Search(string searchRequest)
        {
            var profileSearch = new ProfileSearch();
            profileSearch.Index();
            var profiles = profileSearch.Find(searchRequest);
            if (profiles.FirstOrDefault(x => x != null) != default(Profile))
            {
                int id = profiles.FirstOrDefault(x => x != null).Id;
                using (var DAL = new DAL())
                {
                    var profile = DAL.GetProfile(id);
                    return View(profile.BusinessCards);
                }
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public void PrintCard(int cardID, string clientName)
        {
            using (var DAL = new DAL())
            {
                var card = DAL.GetBusinessCard(cardID);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(card.Url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string data = null;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    data = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();
                }
                data = data.Replace("relative", "absolute");
                using (FileStream filestream = new FileStream(@"G:\itransition\kursSite\task_1\task_1\Prints\" +
                                                                clientName + 
                                                                ".pdf", FileMode.Create))
                {
                    clientPDF.convertHtml(data, filestream);
                    filestream.Close();
                }
            }
        }
    }
}