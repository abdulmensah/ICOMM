using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
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
            return View(db.AppointmentDiary.ToList());
            //return View(db.AppointmentDiary.ToList());
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
        public ActionResult Create(int EventID, HttpPostedFileBase Image)
        {
           
                if (Image != null)
                {
                ArtWorks artwork = new ArtWorks { ID = EventID, DateAdded=DateTime.Now };
                    //attach the uploaded image to the object before saving to Database
                db.ArtWorks.Add(artwork);
                db.SaveChanges();
                    artwork.ImageMimeType = Image.ContentLength;
                    artwork.ImageData = new byte[Image.ContentLength];
                    Image.InputStream.Read(artwork.ImageData, 0, Image.ContentLength);

                string filename = String.Empty; 
              var filePathOriginal = Utilities.Common.ProcessPublicComments(Image, artwork.ArtWorkID);      //Read image back from file and create thumbnail from it
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
                db.Entry(artwork).State = System.Data.EntityState.Modified;
             //   db.ArtWorks.Add(artwork);
                db.SaveChanges();
                System.IO.File.Delete(filePathOriginal);
   
               }

                return RedirectToAction("Index");
        
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
                db.Entry(appointmentdiary).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appointmentdiary);
        }

        //
        // GET: /Event/Delete/5

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