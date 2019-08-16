using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http; // FOR USE OF SESSIONS
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using weddingPlanner.Models; //change projectName to the name of project

namespace weddingPlanner.Controllers  //change projectName to the name of project
{
    public class HomeController : Controller{
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]               // GETS Main Registration and Login Page
        public IActionResult Index(){
            return RedirectToAction("Login");
        }

        [HttpGet("loginreg")]               // GETS Main Registration and Login Page
        public IActionResult Login(){
            return View("Login");
        }

        // The rest of the Controller Code goes here (routes, Posts, Gets, Linq, etc)

        [HttpPost("register")]
        public IActionResult CreateUser(LoginViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == viewModel.newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                viewModel.newUser.Password = Hasher.HashPassword(viewModel.newUser, viewModel.newUser.Password);

                dbContext.Users.Add(viewModel.newUser);
                dbContext.SaveChanges();

                HttpContext.Session.SetInt32("userInSess", viewModel.newUser.UserId);

                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult LoginUser(LoginViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var dbUser = dbContext.Users.FirstOrDefault(u => u.Email == viewModel.newLogin.loginEmail);
                if(dbUser == null)
                {
                    ModelState.AddModelError("Email", "Email does not exist; please create account");
                    return View("Index");
                }

                var hasher = new PasswordHasher<Login>();
                var result = hasher.VerifyHashedPassword(viewModel.newLogin, dbUser.Password, viewModel.newLogin.loginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("Password", "Password does not match Account on File");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("userInSess", dbUser.UserId);

                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("/dashboard")]
        public IActionResult Dashboard(){
            if(HttpContext.Session.GetString("userInSess") != null)
            {
                DashboardViewModel viewModel = new DashboardViewModel();

                viewModel.currentWeddings = dbContext.Weddings
                    .Include(w => w.UserWeddings)
                    .ThenInclude(uw => uw.User)
                    .ToList();

                viewModel.thisUser = dbContext.Users
                    .FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userInSess"));


                return View("Dashboard", viewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet("/addWedding")]
        public IActionResult AddWedding()
        {
            if(HttpContext.Session.GetString("userInSess") != null)
            {
                CreateViewModel viewModel = new CreateViewModel();
                return View("Create", viewModel);
            }
            else{
                return RedirectToAction("Index");
            }
        }

        [HttpPost("createWedding")]
        public IActionResult CreateWedding(CreateViewModel viewModel)
        {
            if(HttpContext.Session.GetString("userInSess") != null)
            {
                if(ModelState.IsValid)
                {
                Wedding newWedding = viewModel.newWedding;
                newWedding.Host = dbContext.Users
                    .FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userInSess"));

                dbContext.Add(newWedding);
                dbContext.SaveChanges();

                return RedirectToAction("Dashboard");
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("wedding/{WeddingId}")]
        public IActionResult Wedding(int WeddingId)
        {
            if(HttpContext.Session.GetString("userInSess") != null)
            {
                WeddingViewModel viewModel = new WeddingViewModel();

                Wedding thisWedding = dbContext.Weddings
                    .Include(uw => uw.UserWeddings)
                    .ThenInclude(ug => ug.User)
                    .FirstOrDefault(p => p.WeddingId == WeddingId);
                    
            return View("Wedding", thisWedding);
            }
        return RedirectToAction("Index");
        }

        [HttpGet("RSVP/{WeddingId}/{UserId}")]
        public IActionResult RSVP(int WeddingId, int UserId){

            UserWedding userWedding= new UserWedding();
            userWedding.UserId = UserId;
            userWedding.WeddingId = WeddingId;

            dbContext.UserWeddings.Add(userWedding);
            dbContext.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet("removeRSVP/{WeddingId}/{UserId}")]
        public IActionResult removeRSVP(int WeddingId, int UserId){
            User user = dbContext.Users
                .FirstOrDefault(u => u.UserId == UserId);

            Wedding wedding = dbContext.Weddings
                .FirstOrDefault(w => w.WeddingId == WeddingId);

            UserWedding userWedding = dbContext.UserWeddings
                .Where(uw => uw.WeddingId == WeddingId && uw.UserId == UserId)
                .FirstOrDefault();

            dbContext.UserWeddings.Remove(userWedding);
            dbContext.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet("/delete/{WeddingId}")]
        public IActionResult DeleteWedding(int WeddingId){
            Wedding wedding = dbContext.Weddings
                .Include(w => w.Host)
                .FirstOrDefault(w => w.WeddingId == WeddingId);

            dbContext.Weddings.Remove(wedding);
            dbContext.SaveChanges();

            return RedirectToAction("Dashboard");
        }
    }
}