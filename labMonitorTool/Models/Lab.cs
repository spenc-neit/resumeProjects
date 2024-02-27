using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace labMonitor.Models
{
    public class Lab
    {
        public int labID { get; set; }
        public string labName { get; set; }

        public string labRoom { get; set; }

        public int deptHead { get; set; }

        public int deptID { get; set; }
    }
}