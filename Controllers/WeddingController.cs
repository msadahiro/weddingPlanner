using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using weddingPlanner.Models;

namespace weddingPlanner.Controllers{
    public class WeddingController:Controller{
        private MyContext _context;
 
        public WeddingController(MyContext context)
        {
            _context = context;
        }
        // GET: /Dashboard
        [HttpGet]
        [RouteAttribute("dashboard")]
        public IActionResult Dashboard(){
            // kicks the user out if not signed in
            if(HttpContext.Session.GetInt32("CurrentUser")==null){
                return RedirectToAction("Login","User");
            }
            else{
                int? getUserId = HttpContext.Session.GetInt32("CurrentUser");
                User SignedInUser = _context.Users.Where(User => User.Id == getUserId).SingleOrDefault();
                // List of ALL Weddings
                List<Wedding> getAllWeddings = _context.Weddings
                        .OrderByDescending(wedding => wedding.WeddingDate)
                        // include the RSVP guests
                        .Include( wedding => wedding.Attendings)
                            // once we add the reserve table, we can then access the USERS based on the reserve table. 
                            .ThenInclude(r => r.User)
                        .ToList();
                // List of all Users
                List<User> getAllUsers = _context.Users
                        // include the Users that RSVP'd to weddings
                        .Include(reserve => reserve.Reserves)
                            // After including the list of reserves, we can access the list of weddings the user attends
                            .ThenInclude(wedding => wedding.Wedding)
                        .ToList();

                ViewBag.CurrentUser = SignedInUser;
                ViewBag.Weddings = getAllWeddings;
                ViewBag.AllUsers = getAllUsers;
                return View ();
            }
        }
        // GET: allows the user to go to dashboard
        [HttpGet]
        [RouteAttribute("back")]
        public IActionResult Back(){
            return RedirectToAction("Dashboard");
        }
        // GET: go to create page
        [HttpGet]
        [RouteAttribute("makeWedding")]
        public IActionResult MakeWedding(){
            var today = DateTime.Today;
            var todayString = today.ToString("yyyy-MM-dd");
            ViewBag.errors = new List<string>();
            ViewBag.Today = todayString;
            return View("Create");
        }
        // POST: Make wedding event
        [HttpPost]
        [RouteAttribute("createevent")]
        public IActionResult CreateEvent(CreateViewModel model, Wedding newWedding){
            if(HttpContext.Session.GetInt32("CurrentUser")==null){
                return RedirectToAction("Login","User");
            }
            if(ModelState.IsValid){
                newWedding.CreatedAt = DateTime.Now;
                newWedding.UpdatedAt = DateTime.Now;
                _context.Add(newWedding);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else{
                ViewBag.errors = ModelState.Values;
                return View("Create");
            }
        }
        // GET: showOne wedding
        [HttpGetAttribute]
        [RouteAttribute("wedding/{id}")]
        public IActionResult ShowOne(int id){
            if(HttpContext.Session.GetInt32("CurrentUser")==null){
                return RedirectToAction("Login","User");
            }
            // show all the weddings where wedding.id == the id that i passed in through the params.
            Wedding ShowOneWedding = _context.Weddings.Where(Wedding => Wedding.Id == id)
                    // include the list of RSVP's for the wedding with the reserve table.
                    .Include( wedding => wedding.Attendings)
                            // after including the reserve table, i can access the users based on the table.
                            .ThenInclude(r => r.User)
                    .SingleOrDefault();
            int count = ShowOneWedding.Attendings.Count();
            ViewBag.ShowOne = ShowOneWedding;
            ViewBag.Count = count;
            return View("ShowOne");
        }
        // GET: button to add guest to RSVP 
        [HttpGetAttribute]
        [RouteAttribute("reserve/{id}")]
        public IActionResult Reserve(int id){
            int? getUserId = HttpContext.Session.GetInt32("CurrentUser");
            Reserve newRsvp = new Reserve(){
                UserId = (int)getUserId,
                WeddingId = id
            };
            _context.Add(newRsvp);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        // GET: button to remove guest from RSVP
        [HttpGetAttribute]
        [RouteAttribute("removeRSVP/{id}")]
        public IActionResult removeRSVP (int id){
            int? getUserId = HttpContext.Session.GetInt32("CurrentUser");
            Reserve remove = _context.Reserves.Where(user => user.UserId == getUserId).Where(wedding => wedding.WeddingId == id).SingleOrDefault();
            _context.Remove(remove);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}