using System.ComponentModel.DataAnnotations;

namespace weddingPlanner.Models{
    public class Reserve{
        [Key]
        public int id {get;set;}
        public int UserId {get;set;}
        public User User {get;set;}

        public int WeddingId {get;set;}
        public Wedding Wedding {get;set;}
    }
}