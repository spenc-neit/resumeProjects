using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace labMonitor.Models
{
    public class Log
    {
        public int logID { get; set; }

        public string studentName { get; set; }

        public int studentID { get; set; } // For export

        public int deptID { get; set; } //use monitor's deptID

        public DateTime timeIn { get; set; } // For export

        public DateTime timeOut { get; set; } // For export

        public string itemsBorrowed { get; set; } // For export
    }
}