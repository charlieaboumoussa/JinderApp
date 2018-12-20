using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinderAPI.Models
{
    public class CandidateDTO
    {
        public Nullable<int> JinderUserId { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Skills { get; set; }
        public string Certification { get; set; }
        public string JobDescription { get; set; }
        public string RequiredSkills { get; set; }
        public string SalaryRange { get; set; }
        public string OperationHours { get; set; }
        public string Location { get; set; }
    }
}