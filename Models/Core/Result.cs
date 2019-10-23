using System;
using System.Collections.Generic;
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
        public Semester Semester { get; set; }
        public int Year { get; set; }
        public double AssessmentScore1 { get; set; }
        public double AssessmentScore2 { get; set; }
        public double ExamScore { get; set; }
    }
}