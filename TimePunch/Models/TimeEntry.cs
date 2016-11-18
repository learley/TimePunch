using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TimePunch.Models
{
    public class TimeEntry
    {
        public int ID { get; set; }
        [DisplayName("Start")]
        public DateTime StartTime { get; set; }
        [DisplayName("End")]
        public DateTime? EndTime { get; set; }
        public string Notes { get; set; } // reserved for later implementation
    }
}