using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FullCalendar_MVC.Controllers
{
    public class MemberController : Controller
    {
        private DiaryContainer db = new DiaryContainer();

        //
        // GET: /Member/

        [Authorize]
        public ActionResult Index()
        {
            return View(db.IcomMembers.ToList());
        }

        //
        // GET: /Member/Details/5

        [Authorize]
        public ActionResult Details(int id = 0)
        {
            IcomMembers icommembers = db.IcomMembers.Find(id);
            if (icommembers == null)
            {
                return HttpNotFound();
            }
            return View(icommembers);
        }

        //
        // GET: /Member/Create

        public ActionResult Create()
        {
            var model = new RegisterViewModel();
            model.Country = "";
            model.State = "";

            return View(model);
        }

        //
        // POST: /Member/Create

        public static int CalculateAge(DateTime birthDay)
        {
            int years = DateTime.Now.Year - birthDay.Year;

            if ((birthDay.Month > DateTime.Now.Month) || (birthDay.Month == DateTime.Now.Month && birthDay.Day > DateTime.Now.Day))
                years--;

            return years;
        }

        public JsonResult GetStates(string countryId)
        {
            var companyQuery = from d in db.states
                               join c in db.countries.Where(i => i.abbreviation == countryId)
                               on d.country_id equals c.id
                               orderby d.name

                               select d;

            var company = companyQuery.Select(m => new SelectListItem()
            {
                Text = m.name,
                Value = m.abbreviation
            });
            return Json(company, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddToFamily(int id)
        {
            var u = new FamilyViewModel();
            u.IncIndex = id;
            return PartialView("AddToFamily", u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel icmb)
        {
            if (ModelState.IsValid)
            {
                var member = new IcomMembers
                {
                    Country = icmb.Country,
                    FullName = icmb.FullName,
                    State = icmb.State,
                    Address = icmb.Address,
                    City = icmb.City,
                    DOB = icmb.DOB,
                    Email = icmb.Email,
                    Phone = icmb.Phone,
                    PostalCode = icmb.PostalCode

                };

                member.DateRegistered = DateTime.Now;
                var dOB = icmb.DOB;
                var fullName = icmb.FullName;
                db.IcomMembers.Add(member);
                 db.SaveChanges();
                db.IcomMembers.Add(member);
                var Id = member.IcomMembersId;
                int intFamMbrs = 1;
                db.FamilyMembers.Add(new FamilyMembers { DOB = dOB, FullName = fullName, IcomMembersId = Id, DateAdded = DateTime.Now });

                var allFamilyMembers = icmb.Family;
                if (allFamilyMembers != null)
                {
                    foreach (var fm in allFamilyMembers)
                    {
                        if (!string.IsNullOrWhiteSpace(fm.FamilyFullName))
                        {
                            var fM = new FamilyMembers
                            {
                                DOB = fm.FamilyDOB,
                                FullName = fm.FamilyFullName,
                                IcomMembersId = Id,
                                DateAdded = DateTime.Now
                            };
                            db.FamilyMembers.Add(fM);
                            if (CalculateAge((DateTime)fm.FamilyDOB) >= 18)
                            { intFamMbrs++; }

                        }
                    }
                }
                db.SaveChanges();
                var currMember = db.IcomMembers.Find(Id);
                currMember.AmountCharged = Convert.ToDecimal(intFamMbrs * 120.0M);
                db.Entry(currMember).State = EntityState.Modified;
                db.SaveChanges();
                Response.Redirect("https://services.madinaapps.com/donation/clients/icom/paymentOptions/308");
            }

            return View(icmb);
        }

        //
        // GET: /Member/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            IcomMembers icommembers = db.IcomMembers.Find(id);
            if (icommembers == null)
            {
                return HttpNotFound();
            }
            return View(icommembers);
        }

        //
        // POST: /Member/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(IcomMembers icommembers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(icommembers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(icommembers);
        }

        //
        // GET: /Member/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            IcomMembers icommembers = db.IcomMembers.Find(id);
            if (icommembers == null)
            {
                return HttpNotFound();
            }
            return View(icommembers);
        }

        //
        // POST: /Member/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            IcomMembers icommembers = db.IcomMembers.Find(id);
            db.IcomMembers.Remove(icommembers);
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