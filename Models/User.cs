using System;
using System.Collections.Generic;

namespace weddingPlanner.Models{
    public class User{
        public int Id {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Email {get;set;}
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public List<Reserve> Reserves {get;set;}
        public User (){
            Reserves = new List<Reserve>();
        }
    }
}