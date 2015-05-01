using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using task_1.Models;
using photo_album_mvc4.Models;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Data.Linq;
using System.Configuration;

namespace task_1.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        Cloudinary cloudinaryAccess;
        List<string> picturesID = new List<string>();
        static List<string> previews = new List<string>();

        public class Cort
        {
            public Cort(Cloudinary cloudinary, List<string> pictures)
            {
                Cloudinary = cloudinary;
                Pictures = pictures;
            }

            public Cloudinary Cloudinary { get; set; }
            public List<string> Pictures { get; set; }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Мы все Товарищи!";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Контакты товарищей!";
            return View();
        }

        [Authorize]
        public ActionResult Gallery()
        {
            var context = new lessDataContext(ConfigurationManager.ConnectionStrings["ImagesConnectionString"].ConnectionString);
            var images = context.img.ToList();
            foreach (var image in images)
            {
                picturesID.Add(image.ImageID);
            }
            Account account = new Account(
              "drez5mnni",
              "695916952775454",
              "yl6QMs9Ew7x5emOieEOvLm0VoV0");
            cloudinaryAccess = new Cloudinary(account);
            Cort c = new Cort(cloudinaryAccess,picturesID);

            return View(c);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public void UploadDirect()
        {
            var headers = HttpContext.Request.Headers;

            string content = null;
            using (StreamReader reader = new StreamReader(HttpContext.Request.InputStream))
            {
                content = reader.ReadToEnd();
            }

            if (String.IsNullOrEmpty(content)) return;

            Dictionary<string, string> results = new Dictionary<string, string>();

            string[] pairs = content.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pair in pairs)
            {
                string[] splittedPair = pair.Split('=');

                if (splittedPair[0].StartsWith("faces"))
                    continue;

                results.Add(splittedPair[0], splittedPair[1]);
            }

            Photo p = new Photo()
            {
                Bytes = Int32.Parse(results["bytes"]),
                CreatedAt = DateTime.ParseExact(HttpUtility.UrlDecode(results["created_at"]), "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture),
                Format = results["format"],
                Height = Int32.Parse(results["height"]),
                Path = results["path"],
                PublicId = results["public_id"],
                ResourceType = results["resource_type"],
                SecureUrl = results["secure_url"],
                Signature = results["signature"],
                Type = results["type"],
                Url = results["url"],
                Version = Int32.Parse(results["version"]),
                Width = Int32.Parse(results["width"]),
            };
            var context = new lessDataContext(ConfigurationManager.ConnectionStrings["ImagesConnectionString"].ConnectionString);
            var images = context.img.ToList();
            int counter = 0;
            foreach (var image in images)
            {
                counter++;
            }
            var newImg = new img
            {
                Id = counter,
                ImageID = p.PublicId+'.'+p.Format,
            };
            context.img.InsertOnSubmit(newImg);
            context.img.Context.SubmitChanges();
        }

        [Authorize]
        public ActionResult Upload()
        {
            Account account = new Account(
              "drez5mnni",
              "695916952775454",
              "yl6QMs9Ew7x5emOieEOvLm0VoV0");
            cloudinaryAccess = new Cloudinary(account);
            return View(cloudinaryAccess);
        }

        [HttpGet]
        [Authorize]
        public ActionResult OboobsUpload()
        {
            return View(previews);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public void OboobsParse()
        {
            string content = null;
            using (StreamReader reader = new StreamReader(HttpContext.Request.InputStream))
            {
                content = reader.ReadToEnd();
            }
            if (String.IsNullOrEmpty(content)) return;
            int startIndexOfPreview = 0;
            int endIndexOfPreview = 0;
            previews.Clear();
            for (int countPreviews = 0; countPreviews < 100; countPreviews++)
            {
                startIndexOfPreview = content.IndexOf("noise_preview", startIndexOfPreview + 1) + 14;
                endIndexOfPreview = content.IndexOf("\"", startIndexOfPreview);
                previews.Add(content.Substring(startIndexOfPreview, endIndexOfPreview - startIndexOfPreview));
                countPreviews++;
            }
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public void OboobsSave()
        {
            Account account = new Account(
              "drez5mnni",
              "695916952775454",
              "yl6QMs9Ew7x5emOieEOvLm0VoV0");
            cloudinaryAccess = new Cloudinary(account);
            string content = null;
            using (StreamReader reader = new StreamReader(HttpContext.Request.InputStream))
            {
                content = reader.ReadToEnd();
            }
            if (String.IsNullOrEmpty(content)) return;
            string filePath = "http://media.oboobs.ru/noise/" + content;
            var uploadedImage = cloudinaryAccess.Upload(new ImageUploadParams()
            {
                File = new FileDescription(filePath)
            });
            var context = new lessDataContext(ConfigurationManager.ConnectionStrings["ImagesConnectionString"].ConnectionString);
            var images = context.img.ToList();
            int counter = 0;
            foreach (var image in images)
            {
                counter++;
            }
            var newImg = new img
            {
                Id = counter,
                ImageID = uploadedImage.PublicId + ".jpg",
            };
            context.img.InsertOnSubmit(newImg);
            context.img.Context.SubmitChanges();
        }

    }
}