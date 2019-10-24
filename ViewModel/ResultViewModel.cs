using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ResultManagement.ViewModel
{
    public class ResultViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Unit Code")]
        [Required]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9 ]*$", ErrorMessage = "It is not a valid code")]
        public string UnitCode { get; set; }
        public int StudentId { get; set; }
        //public s MyProperty { get; set; }
        [Range(1, 2)]
        public int Semester { get; set; }
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
        [Display(Name = "Image")]
        public string ImgPath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImgFile { get; set; }
    }
}