using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFinalEMDAAdvanced.Models
{
    public class Reasons
    {
        [Key]//Primary key
        public int Id { get; set; }
        public string Reason { get; set; }//Reason
        public int ReasonCount { get; set; }//ReasonCount
    }
}
