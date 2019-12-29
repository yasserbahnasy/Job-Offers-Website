using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace Job_Offers_Website.Models
{
    public class ApplayForJob
    {
        public int id { get; set; }

        [Display(Name = "الرسالة المرسلة")]
        public string message { get; set; }

        [Display(Name = "تاريخ التقديم")]
        public DateTime ApplayDate { get; set; }
        public int JobId { get; set; }
        public string UserId { get; set; }

        public virtual Job job { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}