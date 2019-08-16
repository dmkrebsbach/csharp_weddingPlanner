using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace weddingPlanner.Models{  //change projectName to the name of project

    public class DashboardViewModel
    {
        public User thisUser {get;set;}
        public List<Wedding> currentWeddings {get;set;}  // public "ClassName" should match the class name in the file, not the file name itself
        public UserWedding UserWedding {get;set;}

        public List<User> Hosts {get;set;} 

    }
}