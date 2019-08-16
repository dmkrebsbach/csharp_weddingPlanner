using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace weddingPlanner.Models{
    public class User{
        [Key]
        public int UserId{get;set;}
        [Required]
        [MinLength(2)]
        public string FirstName{get;set;}
        [Required]
        [MinLength(2)]
        public string LastName{get;set;}
        [Required]
        [EmailAddress]
        public string Email{get;set;}
        [Required]
        [MinLength(8)]
        public string Password{get;set;}

        public List<UserWedding> UserWeddings{get;set;}

        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdatedAt{get;set;} = DateTime.Now;

        [NotMapped]
        [Compare("Password")]
        [MinLength(8)]
        public string ConfirmPassword{get;set;}
    }

    public class LoginUser{
        public string Email{get;set;}
        public string Password{get;set;}
    }
}


