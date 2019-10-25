using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static ResultManagement.Helpers.Enums;

namespace ResultManagement.Models.Core
{
    public class Result
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        [Display(Name = "Unit Code")]
        [Required]
        [RegularExpression("^[a-zA-Z]{3,3}[0-9]{4,4}", ErrorMessage = "It is not a valid code")]
        public string UnitCode { get; set; }
        public int StudentId { get; set; }
        //public s MyProperty { get; set; }
        [Range(1,2)]
        public int Semester { get; set; }
        [RegularExpression("^[1-2][0-9]{3,3}",ErrorMessage ="It is not a valid year")]
        public int Year { get; set; }
        [Range(0, 20)]
        [Display(Name = "Assessment Score 1")]
        public double AssessmentScore1 { get; set; }
        [Range(0, 20)]
        [Display(Name = "Assessment Score 2")]
        public double AssessmentScore2 { get; set; }
        [Range(0, 60)]
        [Display(Name = "Exam Score")]
        public double ExamScore { get; set; }
        [Display(Name ="Image")]
        public string ImgPath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImgFile { get; set; }
    }
}