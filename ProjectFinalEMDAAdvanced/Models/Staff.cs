using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFinalEMDAAdvanced.Models
{
    public class Staff
    {
        [Key]//Primary key
        public int Id { get; set; }
        public string FirstName { get; set; }//FirstName
        public string LastName { get; set; }//Lastname
        public DateTime TimeIn { get; set; }//TimeIN
        public DateTime TimeOut { get; set; }//Timeout
        public bool In { get; set; }//Using bool

    }
}
