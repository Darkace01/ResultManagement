using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ResultManagement.Models.Core
{
    public class Unit
    {
        public int Id { get; set; }
        [Display(Name = "Unit Code")]
        [Required]
        public string UnitCode { get; set; }
        [Display(Name = "Unit Title")]
        [Required]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9 ]*$",ErrorMessage ="It is not a valid Title")]
        public string UnitTitle { get; set; }

        [RegularExpression("^[a-zA-Z][a-zA-Z .]*$", ErrorMessage = "It is not a valid Name")]
        [Display(Name = "Unit Cordinator")]
        [Required]
        public string UnitCordinator { get; set; }

        public string PdfPath { get; set; }
        [Display(Name ="OutLine Document")]
        public string PdfTitle { get; set; }
        [NotMapped]
        public HttpPostedFileBase PdfFile { get; set; }


    }
}