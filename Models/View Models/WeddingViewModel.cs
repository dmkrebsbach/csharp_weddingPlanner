using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace weddingPlanner.Models{  //change projectName to the name of project

    public class WeddingViewModel
    {
        public Wedding thisWedding {get;set;}  // public "ClassName" should match the class name in the file, not the file name itself
        public UserWedding UserWedding {get;set;}

        public List<UserWedding> UserWeddings {get; set;}

        public List<User> Users {get;set;} 
        
        public User thisUser {get;set;}// include all classes and a new instance of the class within this file
    }
}