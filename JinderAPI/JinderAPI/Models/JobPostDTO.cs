using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinderAPI.Models
{
    public class JobPostDTO
    {
        public int JobPostId { get; set; }
        public Nullable<int> PosterId { get; set; }
        public string JobDescription { get; set; }
        public string RequiredSkills { get; set; }
        public string SalaryRange { get; set; }
        public string OperationHours { get; set; }
        public string Location { get; set; }
    }
}