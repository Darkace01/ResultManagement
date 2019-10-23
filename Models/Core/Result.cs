using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static ResultManagement.Helpers.Enums;

namespace ResultManagement.Models.Core
{
    public class Result
    {
        public int Id { get; set; }
        public int UnitCode { get; set; }
        public int StudentId { get; set; }
        //public s MyProperty { get; set; }
        [Range(1,2)]
        public int Semester { get; set; }
        public int Year { get; set; }
        [Range(0, 20)]
        public double AssessmentScore1 { get; set; }
        [Range(0, 20)]
        public double AssessmentScore2 { get; set; }
        [Range(0, 60)]
        public double ExamScore { get; set; }
        
    }
}