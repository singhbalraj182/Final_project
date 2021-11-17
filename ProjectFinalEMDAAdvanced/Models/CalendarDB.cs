using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFinalEMDAAdvanced.Models
{
    public class CalendarDB
    {
        [Key()]//Primary key
        public string EventID { get; set; }//Event ID
        public string Subject { get; set; }//Subject
        public string Description { get; set; }//Description
        public DateTime Start { get; set; }// Start Datetime
        public DateTime End { get; set; }//End datetime
        public string ThemeColor { get; set; }// themecolor
        public bool IsFullDay { get; set; }// using bool
        public string delete { get; set; }//Delete
    }
}
