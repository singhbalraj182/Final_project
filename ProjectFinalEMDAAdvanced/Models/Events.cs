using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFinalEMDAAdvanced.Models
{
    public class Events
    {
        [Key]//Primary key
        public int Id { get; set; }

        //[Required] public string ResourceId { get; set; }

        public string EventColor { get; set; }//Event color

        [DataType(DataType.DateTime)]//Datetime
        public DateTime Start { get; set; }

        [DataType(DataType.DateTime)]
        //[ValidateEndDate(ErrorMessage = "Start date must be before End date")]
        public DateTime End { get; set; }
        public string Title { get; set; }//Title
        public bool IsFullDay { get; set; }//using bool
        public int Days { get; set; }//Days
        public int Weeks { get; set; }//Weeks
        public Staff Staff { get; set; }//staff
    }
}
