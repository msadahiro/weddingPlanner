using System;
using System.ComponentModel.DataAnnotations;

namespace weddingPlanner.Models{
    public class CreateViewModel{
        [Required]
        [MinLengthAttribute(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string SpouseName1 {get;set;}
        [Required]
        [MinLengthAttribute(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string SpouseName2 {get;set;}

        [Required]
        public DateTime WeddingDate {get;set;}
    }
}