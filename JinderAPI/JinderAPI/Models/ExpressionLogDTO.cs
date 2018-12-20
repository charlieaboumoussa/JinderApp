using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinderAPI.Models
{
    public class ExpressionLogDTO
    {
        public int ExpressionLogId { get; set; }
        public Nullable<int> SourceUserId { get; set; }
        public Nullable<int> TargetUserId { get; set; }
        public Nullable<bool> IsInterested { get; set; }
      
    }
}