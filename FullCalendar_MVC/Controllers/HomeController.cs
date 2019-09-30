using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace FullCalendar_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            DiaryContainer db = new DiaryContainer();

            var donation = db.Donation.FirstOrDefault();

            ViewBag.CurrDate = easternTime.ToLongDateString();
            ViewBag.TargetAmount = String.Format("{0:n0}", donation.TargetAmount);
            ViewBag.CurrAmount = String.Format("{0:n0}", donation.CurrentAmount);  
            return View();
        }

        public ActionResult Gallery()
        {
            return View();
        }

        public ActionResult Donate()
        {
            return View();
        }
        public ActionResult Expand()
        {
            DiaryContainer db = new DiaryContainer();

            var donation = db.Donation.FirstOrDefault();

            ViewBag.TargetAmount = String.Format("{0:n0}", donation.TargetAmount);
            ViewBag.CurrAmount = String.Format("{0:n0}", donation.CurrentAmount);

            return View();
        }

        public ActionResult Services()
        {
            return View();
        }

        public JsonResult GetHeaders()
        {
          //  return View();
      return Json(Request.Headers.AllKeys.ToList(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetEvents()
        {
            DiaryContainer db = new DiaryContainer();
            IEnumerable<AppointmentDiary> list = (from t in db.AppointmentDiary

                                                  orderby t.DateTimeScheduled descending
                                                  select t).Take(3);
            var eventList = from e in list
                            select new Models.GalleryEvent
                            {
                                GalleryEventID = e.ID,
                                Title = e.Title,
                                EventDate = e.DateTimeScheduled.ToString("MM/dd/yyyy hh:mm:ss tt"),
                                Description = e.Description,
                                EventType = e.EventType,
                            };
            return Json(eventList.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGallery()
        {
            DiaryContainer db = new DiaryContainer();

            var list = (from t in db.ArtWorks
                        orderby t.DateAdded descending
                        select t).Take(8);

            List<Models.ImageItem> eventList = new List<Models.ImageItem>();

            foreach (var item in list)
            {
                eventList.Add(
                    new Models.ImageItem
                    {
                        Description = Utilities.StringExtensions.CutString(ImageEvent(db, item.ID).Description, 80),
                        ImageID = item.ArtWorkID,
                        Title = Utilities.StringExtensions.CutString(ImageEvent(db, item.ID).Title, 15)
                    });
            }

            return Json(eventList, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult GetThumbnailImage(int imageID)
        {
            DiaryContainer db = new DiaryContainer();
            ArtWorks art = db.ArtWorks.FirstOrDefault(p => p.ArtWorkID == imageID);
            if (art != null)
            {
                return File(art.ArtworkThumbnail, art.ImageMimeType.ToString());
            }
            else
            {
                return null;
            }
        }

        public FileContentResult GetGalleryImage(int imageID)
        {
            DiaryContainer db = new DiaryContainer();
            ArtWorks art = db.ArtWorks.FirstOrDefault(p => p.ArtWorkID == imageID);
            if (art != null)
            {
                return File(art.ImageData, art.ImageMimeType.ToString());
            }
            else
            {
                return null;
            }
        }

        public ActionResult FAQ()
        {
            return View();
        }
        public ActionResult ViewPDF()
        {
            return View();
        }

        public ActionResult Contact()
        {
            string TitlMsg = Session["CustomError"] != null ? Session["CustomError"].ToString() : "Get in touch via any of the media below and we’ll get back to you as soon as we can.  We look forward to hearing from you!";

            ViewBag.Message = TitlMsg;
            Session.Remove("CustomError");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult SendContact(ContactInfo model)
        {
            //   bool isOK = false;

            var contact = new ContactInfo { FullName = model.FullName, Email = model.Email, Phone = model.Phone, Message = model.Message, DateSent = DateTime.Now };
            DiaryContainer db = new DiaryContainer();

            db.ContactInfo.Add(contact);
            db.SaveChanges();
            return Json("Your message has been sent successfully. We will contact you with feedback shortly.", JsonRequestBehavior.DenyGet);
        }

        public ActionResult About()
        {
            // ViewBag.Message = "Get in touch via any of the media below and we’ll get back to you as soon as we can.  We look forward to hearing from you!";
            return View();
        }

        //public ActionResult RenderPDF(int id)
        //{
        //    ViewData["fileID"] = id;

        //    return PartialView("_PDFPartial");
        //}

        public FileResult DownloadFile(int fileID)
        {
            DiaryContainer db = new DiaryContainer();
            var file = db.Folder.Find(fileID);
            if (file != null)
            {
                string fileName = file.FileName;
                string filePath = "/Allfiles/";

                if (System.Configuration.ConfigurationManager.AppSettings["TempFilePath"] != null)
                {
                    filePath = System.Configuration.ConfigurationManager.AppSettings["TempFilePath"] + filePath;
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath + fileName);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            else
                return null;


        }

        public FileResult ViewPDFFile(int fileID)
        {
            DiaryContainer db = new DiaryContainer();
            var file = db.Folder.Find(fileID);
            if (file != null)
            {
                string fileName = file.FileName;
                string filePath = "/Allfiles/";

                     filePath = System.Configuration.ConfigurationManager.AppSettings["TempFilePath"] + filePath;
               //if (System.Configuration.ConfigurationManager.AppSettings["TempFilePath"] != null)
               // {
               // }

                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath + fileName);
                return File(fileBytes, "application/pdf");

            }
            else
                return null;


        }

        public ActionResult Search(string SearchString)
        {
            ViewBag.SearchString = SearchString;
            ViewBag.Title = "Search";
            return View("SearchResult");
        }

        public AppointmentDiary ImageEvent(DiaryContainer db, int? ID)
        {
                return db.AppointmentDiary.Find(ID);
       
        }


    }
}
