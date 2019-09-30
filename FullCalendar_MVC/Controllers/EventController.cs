using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace FullCalendar_MVC.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private DiaryContainer db = new DiaryContainer();

        //
        // GET: /Event/

        public ActionResult Index()
        {
            return View();
            //return View(db.AppointmentDiary.ToList());
        }
        public ActionResult ContactMessage()
        {
            return View();

        }
        public ActionResult AddGallery()
        {
            return View();

        }


        public ActionResult SetSalaat()
        {
            return View();

        }
        public JsonResult GetContacts()
        {
            var Contacts = db.ContactInfo.OrderByDescending(e=>e.DateSent).ToList();
            var messgaes = from e in Contacts select new { ID = e.ContactID, Date = e.DateSent.ToString(), Message = e.Message, Email = e.Email, FullName = e.FullName };
            return Json(messgaes.ToList(), JsonRequestBehavior.AllowGet);
            //return View(db.AppointmentDiary.ToList());
        }
        public JsonResult GetMessage(int Id)
        {
            var Message = db.ContactInfo.Find(Id);
            if (Message == null)
            {
                return new JsonResult();
            }

            return Json(new {ID=Message.Email, Title= Message.FullName +" - "+Message.DateSent.ToString(), Body= Message.Message }, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult SendReply(string Email, string Body, string FullName)
        //{
        //    var SentInfo = FullName.Split('-');
        //    MailMessage message = new MailMessage();
        //    message.From = new MailAddress("info@icommd.org");

        //    message.To.Add(new MailAddress(Email, SentInfo[0]));

        //    message.Subject = "Reply to Message Sent on" + SentInfo[1];
        //    message.Body = Body;

        //    SmtpClient client = new SmtpClient();
        //    client.Send(message);

        //    return Json(new {ID=Message.Email, Title= Message.FullName +" - "+Message.DateSent.ToString(), Body= Message.Message }, JsonRequestBehavior.AllowGet);
        //}
        public bool SendReply(string Email, string Body, string FullName)
        {
            bool blnStatus = false;

            try
            {
                string[] strSep = new string[] { " - " };
                string[] SentInfo = FullName.Split(strSep, StringSplitOptions.None);

             //   var PartSubject = SentInfo[1];
                MailMessage message = new MailMessage();
                message.From = new MailAddress("info@icommd.org", "Islamic Center of Owings Mills");

                message.To.Add(new MailAddress(Email, SentInfo[0]));

                message.Subject = "Reply to your message sent on" + SentInfo[1];
                message.Body = Body;

                SmtpClient client = new SmtpClient();
                client.Send(message);
                blnStatus = true;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                blnStatus = false;

            }

            return blnStatus;
        }

        public bool AutoReply(string Email, string Body, string FullName)
        {
            var SentInfo = FullName.Split('-');
            MailMessage message = new MailMessage();
            message.From = new MailAddress("info@icommd.org");

            message.To.Add(new MailAddress(Email, SentInfo[0]));

            message.Subject = "Reply to Message Sent on " + SentInfo[1];
            message.Body = Body;

            SmtpClient client = new SmtpClient();
            client.Send(message);

            return true;
        }

        [AllowAnonymous]
        public JsonResult GetSalaat()
        {
            var Sataals = db.Salaat.FirstOrDefault();
            return Json(Sataals, JsonRequestBehavior.AllowGet);
            //return View(db.AppointmentDiary.ToList());
        }


        public ActionResult SaveSalaat(string Fajr,string Dhuhr, string Asr, string Maghrib,string Isha, string Khutbah1,  string Khutbah2,string Jumma1, string Jumma2)
        {
            var Sataals = db.Salaat.FirstOrDefault();
            Sataals.Asr = Asr;
            Sataals.Fajr = Fajr;
            Sataals.DateAdded = DateTime.Now;
            Sataals.Dhuhr = Dhuhr;
            Sataals.Maghrib = Maghrib;
            Sataals.Isha = Isha;
            Sataals.Khutbah1 = Khutbah1;
            Sataals.Jumma1 = Jumma1;
            Sataals.Khutbah2 = Khutbah2;
            Sataals.Jumma2 = Jumma2;

            db.Entry(Sataals).State = EntityState.Modified;// System.Data.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("SetSalaat");
        }

        //
        public string Init()
        {
            bool rslt = Models.Utils.InitialiseDiary();
            return rslt.ToString();
        }


        public void UpdateEvent(int id, string NewEventStart, string NewEventEnd)
        {
            Models.DiaryEvent.UpdateDiaryEvent(id, NewEventStart, NewEventEnd);
        }


        public bool SaveEvent(string Title, string Description, string NewEventDate, string NewEventTime, string NewEventDuration, int NewEventType, int EventID=0)
        {
            return Models.DiaryEvent.CreateNewEvent(Title, Description,NewEventDate, NewEventTime, NewEventDuration, (Models.EventType)NewEventType,EventID);
        }

        public JsonResult GetDiarySummary(double start, double end)
        {
            var ApptListForDate = Models.DiaryEvent.LoadAppointmentSummaryInDateRange(start, end);
            var eventList = from e in ApptListForDate
                            select new
                            {
                                id = e.ID,
                                title = e.Title,
                                start = e.StartDateString,
                                end = e.EndDateString,
                                someKey = e.SomeImportantKeyID,
                                allDay = false
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEventDetail(int Id)
        {
            var appointmentdiary = db.AppointmentDiary.Find(Id);
            if (appointmentdiary == null)
            {
                return new JsonResult();
            }

            return Json(appointmentdiary, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDiaryEvents(double start, double end)
        {
            var ApptListForDate = Models.DiaryEvent.LoadAllAppointmentsInDateRange(start, end);
            var eventList = from e in ApptListForDate
                            select new
                            {
                                id = e.ID,
                                title = e.Title,
                                start = e.StartDateString,
                                end = e.EndDateString,
                                color = e.StatusColor,
                                className = e.ClassName,
                                someKey = e.SomeImportantKeyID,
                                allDay = false
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowEvents()
        {
            return Json((from e in db.AppointmentDiary.ToList() select new { ID = e.ID, Title = e.Title+" - "+e.DateTimeScheduled.ToString("MM/dd/yyyy hh:mm:ss tt") }).ToList(), JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Event/Details/5

        public ActionResult Details(int id = 0)
        {
            AppointmentDiary appointmentdiary = db.AppointmentDiary.Find(id);
            if (appointmentdiary == null)
            {
                return HttpNotFound();
            }
            return View(appointmentdiary);
        }

        //
        // GET: /Event/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Event/Create

        [HttpPost]
        public ActionResult Create(int EventID, HttpPostedFileBase[] Images)
        {

            if (Images != null)
            {
                foreach (var Image in Images)
                {
                    ArtWorks artwork = new ArtWorks { ID = EventID, DateAdded = DateTime.Now };
                    //attach the uploaded image to the object before saving to Database
                    db.ArtWorks.Add(artwork);
                    db.SaveChanges();
                    artwork.ImageMimeType = Image.ContentLength;
                    artwork.ImageData = new byte[Image.ContentLength];
                    Image.InputStream.Read(artwork.ImageData, 0, Image.ContentLength);

                    string filename = String.Empty;
                    var filePathOriginal = Utilities.Common.ProcessPublicComments(Image, artwork.ArtWorkID); 
                    //Read image back from file and create thumbnail from it
                    //   var filename = Path.Combine(Server.MapPath("~/Content/portfolio/full"), filename);
                    filename = System.IO.Path.GetFileName(filePathOriginal);
                    using (var srcImage = System.Drawing.Image.FromFile(filePathOriginal))
                    using (var newImage = new System.Drawing.Bitmap(338, 220))
                    using (var graphics = System.Drawing.Graphics.FromImage(newImage))
                    using (var stream = new System.IO.MemoryStream())
                    {
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        graphics.DrawImage(srcImage, new System.Drawing.Rectangle(0, 0, 338, 220));
                        newImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        var thumbNew = File(stream.ToArray(), "image/png");
                        artwork.ArtworkThumbnail = thumbNew.FileContents;
                    }
                    //Save model object to database
                    db.Entry(artwork).State = EntityState.Modified;// System.Data.EntityState.Modified;
                    //   db.ArtWorks.Add(artwork);
                    db.SaveChanges();
                    System.IO.File.Delete(filePathOriginal);

                }
            }

            return RedirectToAction("AddGallery");

        }

        //
        // GET: /Event/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AppointmentDiary appointmentdiary = db.AppointmentDiary.Find(id);
            if (appointmentdiary == null)
            {
                return HttpNotFound();
            }
            return View(appointmentdiary);
        }

        //
        // POST: /Event/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AppointmentDiary appointmentdiary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointmentdiary).State = EntityState.Modified;// System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appointmentdiary);
        }

        [Authorize]
        public ActionResult Donation()
        {
            var donation = db.Donation.FirstOrDefault();
            if (donation == null)
            {
                return HttpNotFound();
            }
            DonationViewModel currDon = new DonationViewModel
            {
                DonationID = donation.DonationID,
                Image = donation.Image,
                Background = donation.Background,
                TargetAmount = (decimal)donation.TargetAmount,
                CurrentAmount = (decimal)donation.CurrentAmount,
                Description = donation.Description,
                DonationType = donation.DonationType
            };
            return View(currDon);
        }

        //
        // GET: /Event/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CurrDonation(DonationViewModel currDon)
        {
            if (ModelState.IsValid)
            {
                var donation = db.Donation.FirstOrDefault();
                donation.Description = currDon.Description;
                donation.DonationType = currDon.DonationType;
                donation.DateSet = DateTime.Now;
                donation.TargetAmount = currDon.TargetAmount;
                donation.CurrentAmount = currDon.CurrentAmount;

                db.Entry(donation).State = EntityState.Modified;// System.Data.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Donation");
            }

            return View(currDon);
        }

        public ActionResult Delete(int id = 0)
        {
            AppointmentDiary appointmentdiary = db.AppointmentDiary.Find(id);
            if (appointmentdiary == null)
            {
                return HttpNotFound();
            }
            return View(appointmentdiary);
        }

        //
        // POST: /Event/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppointmentDiary appointmentdiary = db.AppointmentDiary.Find(id);
            db.AppointmentDiary.Remove(appointmentdiary);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}