using System;
using System.Collections.Generic;

namespace weddingPlanner.Models{
    public class Wedding{
        public int Id {get;set;}
        public string SpouseName1 {get;set;}
        public string SpouseName2 {get;set;}
        public DateTime WeddingDate {get;set;}
        public int CreatedByUserId {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public List<Reserve> Attendings {get;set;}
        public Wedding (){
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Attendings = new List<Reserve>();
        }
    }
}