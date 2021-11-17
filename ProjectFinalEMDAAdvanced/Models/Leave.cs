using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFinalEMDAAdvanced.Models
{
    public class Leave
    {
        [Key]//Primary key
        public int Id { get; set; }
        public Staff Staff { get; set; }//Staff
        public DateTime StartDate { get; set; }//Startdate
        public DateTime EndDate { get; set; }//Enddate
        public int TotalDays { get; set; }//Total days
        public bool Accepted { get; set; }//Accepted
    }
}
