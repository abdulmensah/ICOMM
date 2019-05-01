using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace FullCalendar_MVC.Controllers
{
    public class MemberController : Controller
    {
        private DiaryContainer db = new DiaryContainer();

        //
        // GET: /Member/

        [Authorize]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Address" ? "address_desc" : "Address";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var members = from s in db.IcomMembers
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                members = members.Where(s => s.FullName.Contains(searchString)
                                       || s.Email.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    members = members.OrderByDescending(s => s.FullName);
                    break;
                case "Address":
                    members = members.OrderBy(s => s.Address);
                    break;
                case "address_desc":
                    members = members.OrderByDescending(s => s.Address);
                    break;
                default:
                    members = members.OrderBy(s => s.FullName);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(members.ToList().Select(m => CreateMember(m)).ToPagedList(pageNumber, pageSize));
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

        public ActionResult AddToFamily(int id, bool isPrimary = false)
        {
            var u = new FamilyViewModel();
            u.IncIndex = id;
            u.IsPrimary = isPrimary;
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
                db.IcomMembers.Add(member);
                db.SaveChanges();

                //var dOB = member.DOB;
                //var fullName = member.FullName;
                //db.FamilyMembers.Add(new FamilyMembers { DOB = dOB, FullName = fullName, IcomMembersId = member.IcomMembersId, DateAdded = DateTime.Now });
                //db.SaveChanges();

                icmb.Family = icmb.Family.Concat(new List<FamilyViewModel>() { new FamilyViewModel { FamilyDOB = member.DOB, FamilyFullName = member.FullName, IncIndex = 1, IsPrimary=true } });
                //mList.Add
                icmb.IcomMembersId = member.IcomMembersId;

                SaveFamilies(icmb.IcomMembersId, icmb.Family);
                Response.Redirect("https://services.madinaapps.com/donation/clients/icom/paymentOptions/308");
            }

            return View(icmb);
        }

        //
        // GET: /Member/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            IcomMembers member = db.IcomMembers.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(CreateMember(member));
        }



        private RegisterViewModel CreateMember(IcomMembers icmb)
        {
            return new RegisterViewModel
            {
                IcomMembersId = icmb.IcomMembersId,
                AmountCharged = icmb.AmountCharged,
                AmountPaid = icmb.AmountPaid,
                DateRegistered = icmb.DateRegistered,
                Country = icmb.Country,
                FullName = icmb.FullName,
                State = icmb.State,
                Address = icmb.Address,
                City = icmb.City,
                DOB = icmb.DOB,
                Email = icmb.Email,
                Phone = icmb.Phone,
                PostalCode = icmb.PostalCode,
                IsDeleted = icmb.IsDeleted
        };

        }

        //
        // POST: /Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(RegisterViewModel icmb)
        {
            if (ModelState.IsValid)
            {
                //Save Member Details
                var member = db.IcomMembers.Find(icmb.IcomMembersId);
                member.Country = icmb.Country;
                member.FullName = icmb.FullName;
                member.State = icmb.State;
                member.Address = icmb.Address;
                member.City = icmb.City;
                member.DOB = icmb.DOB;
                member.Email = icmb.Email;
                member.Phone = icmb.Phone;
                member.PostalCode = icmb.PostalCode;
                member.DateRegistered = icmb.DateRegistered;
                member.AmountPaid = icmb.AmountPaid;
                member.IsDeleted = icmb.IsDeleted;

                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();


                //Remove all existing family details for this member
                db.FamilyMembers.RemoveRange(db.FamilyMembers.Where(m => m.IcomMembersId == member.IcomMembersId));
                db.SaveChanges();
                icmb.Family = icmb.Family.Concat(new List<FamilyViewModel>() { new FamilyViewModel { FamilyDOB = member.DOB, FamilyFullName = member.FullName, IncIndex = 1, IsPrimary = true } });

                //Save new Family member details for this member
                SaveFamilies(icmb.IcomMembersId, icmb.Family.ToList());

                return RedirectToAction("Index");
            }
            return View(icmb);
        }


        private void SaveFamilies(int Id, IEnumerable<FamilyViewModel> members, int currMembers = 0)
        {
            //Save Family member details
          //  var Id = member.IcomMembersId;
            int intFamMbrs = currMembers;
            var allFamilyMembers = members;
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
                            DateAdded = DateTime.Now,
                            PrimaryMember=fm.IsPrimary
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

        [Authorize]
        public ActionResult DeleteMember(int id, int status = 1)
        {
            IcomMembers icommembers = db.IcomMembers.Find(id);
            icommembers.IsDeleted = status != 0;
            db.Entry(icommembers).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id, bool blDel=true)
        {
            IcomMembers icommembers = db.IcomMembers.Find(id);
            //db.IcomMembers.Remove(icommembers);
            icommembers.IsDeleted = blDel;
            db.Entry(icommembers).State = EntityState.Modified;
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