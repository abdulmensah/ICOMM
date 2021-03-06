﻿
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace FullCalendar_MVC
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class State
    {
        public string Name { get; set; }
        public string Abbr { get; set; }
        public string Country { get; set; }

    }

    public partial class RegisterViewModel
    {
        private DiaryContainer db = new DiaryContainer();

        public int IcomMembersId { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Zip")]
        public string PostalCode { get; set; }

        [Display(Name = "Date")]
        public Nullable<System.DateTime> DateRegistered { get; set; }

        [Display(Name = "Payment")]
        public Nullable<decimal> AmountPaid { get; set; }

        [Display(Name = "Status")]
        public Nullable<bool> IsDeleted { get; set; }

        [Display(Name = "Charged")]
        public Nullable<decimal> AmountCharged { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        public Nullable<System.DateTime> DOB { get; set; }

        [NotMapped]
        public virtual List<State> States
        {
            get
            {
                var stList = db.states.ToList();
                return stList.Select(s => new State { Name = s.name, Abbr = s.abbreviation, Country = s.country_id == 1 ? "CAN" : "USA" }).ToList();
            }
        }
        public virtual List<State> EditStates
        {
            get
            {
                var cId = Country == "CAN" ? 1 : 2;
                var stList = db.states.Where(s => s.country_id == cId).ToList();
                return stList.Select(s => new State { Name = s.name, Abbr = s.abbreviation, Country = s.country_id == 1 ? "CAN" : "USA" }).ToList();
            }
        }

        [Display(Name = "Family Members")]
        public IEnumerable<FamilyViewModel> Family { get; set; }


        [Display(Name = "Family Members")]
        public IEnumerable<FamilyMembers> Families
        {
            get
            {
                return db.FamilyMembers.Where(s => s.IcomMembersId == IcomMembersId).OrderByDescending(s=>s.PrimaryMember);
            }
            
        }

        public virtual List<countries> Countries
        {
            get
            {
                var cntList = db.countries.ToList();
                cntList.Add(new countries { name = "Select Country", abbreviation = "", id = 0 });
                return cntList;
            }
        }
    }

}

