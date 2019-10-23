using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResultManagement.Models.Core
{
    public class Unit
    {
        public int Id { get; set; }
        [Display(Name = "Unit Code")]
        public string UnitCode { get; set; }
        [Display(Name = "Unit Title")]
        public string UnitTitle { get; set; }
        [Display(Name = "Unit Cordinator")]
        public string UnitCordinator { get; set; }
    }
}