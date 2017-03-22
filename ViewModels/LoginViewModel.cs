using System.ComponentModel.DataAnnotations;

namespace weddingPlanner.Models{
    public class LoginViewModel{
        [Required]
        [EmailAddress]
        public string Email {get;set;}
        
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}