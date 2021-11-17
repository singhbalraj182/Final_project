using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFinalEMDAAdvanced.Models
{
    public class SignOuts
    {
        [Key]//primary key
        public int Id { get; set; }
        public Staff Staff { get; set; }//Staff
        public DateTime Day { get; set; }//Day
        public DateTime TimeOut { get; set; }//Timeout
        public Reasons Reason { get; set; }//Reasons
        public string HoursIn { get; set; }//HoursIn
    }
}
