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
        [HttpGet]
        [RouteAttribute("dashboard")]
        public IActionResult Dashboard(){
            
            if(HttpContext.Session.GetInt32("CurrentUser")==null){
                return RedirectToAction("Login","User");
            }
            else{
                int? getUserId = HttpContext.Session.GetInt32("CurrentUser");
                User SignedInUser = _context.Users.Where(User => User.Id == getUserId).SingleOrDefault();
                List<Wedding> getAllWeddings = _context.Weddings
                        .Include( wedding => wedding.Attendings)
                            .ThenInclude(r => r.User)
                        .ToList();
                int count = getAllWeddings.Count();
                ViewBag.CurrentUser = SignedInUser;
                
                ViewBag.Count = count;
                ViewBag.Weddings = getAllWeddings;
                return View ();
            }
        }
        [HttpGet]
        [RouteAttribute("back")]
        public IActionResult Back(){
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [RouteAttribute("makeWedding")]
        public IActionResult MakeWedding(){
            var today = DateTime.Today;
            var todayString = today.ToString("yyyy-MM-dd");
            ViewBag.errors = new List<string>();
            ViewBag.Today = todayString;
            return View("Create");
        }
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
        [HttpGetAttribute]
        [RouteAttribute("wedding/{id}")]
        public IActionResult ShowOne(int id){
            if(HttpContext.Session.GetInt32("CurrentUser")==null){
                return RedirectToAction("Login","User");
            }
            Wedding ShowOneWedding = _context.Weddings.Where(Wedding => Wedding.Id == id)
                    .Include( wedding => wedding.Attendings)
                            .ThenInclude(r => r.User)
                    .SingleOrDefault();
            int count = ShowOneWedding.Attendings.Count();
            ViewBag.ShowOne = ShowOneWedding;
            ViewBag.Count = count;
            return View("ShowOne");
        }
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
    }
}