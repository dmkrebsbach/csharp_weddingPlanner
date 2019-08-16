using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace weddingPlanner.Models{
    public class UserWedding{
        [Key]
        public int id{get;set;}

        public int UserId{get;set;}
        public int WeddingId{get;set;}

        public User User{get;set;}
        public Wedding Wedding{get;set;}

    }
}