using ProjectFinalEMDAAdvanced.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFinalEMDAAdvanced.ViewModels
{
    public class CreateSignOutVM
    {
        [Key]//Primary key
        public int Id { get; set; }
        public Staff Staff { get; set; }//Staff
        public DateTime Day { get; set; }//Day
        public DateTime TimeOut { get; set; }//TimeOut
        public Reasons Reason { get; set; }//Reason
        public int HoursIn { get; set; }//HoursIn

        public bool StaffIn { get; set; }//StaffIn
    }
}
