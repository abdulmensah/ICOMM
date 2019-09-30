


namespace FullCalendar_MVC
{

    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class DonationViewModel
    {

        public System.Guid DonationID { get; set; }


         [Required]
        [Display(Name = "Name")]
        public string DonationType { get; set; }


        [Required]
       [Display(Name = "Target Amount")]
        public decimal TargetAmount { get; set; }

        [Required]
        [Display(Name = "Current Amount")]
        public decimal CurrentAmount { get; set; }

        [Required]
        [Display(Name = "Brief Description")]
        public string Description { get; set; }

        [Display(Name = "Banner Image")]
        public string Image { get; set; }

        [Display(Name = "Banner Background")]
        public string Background { get; set; }

        [Display(Name = "Last Modified")]
        public Nullable<System.DateTime> DateSet { get; set; }

    }

}

