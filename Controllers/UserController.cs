using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using weddingPlanner.Models;

namespace weddingPlanner.Controllers
{
    public class UserController : Controller
    {
        private MyContext _context;
 
        public UserController(MyContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [RouteAttribute("register")]
        public IActionResult Register(){
            ViewBag.errors = new List<string>();
            ViewBag.RegEmailError = "";
            return View("Register");
        }
        [HttpGet]
        [RouteAttribute("login")]
        public IActionResult Login(){
            ViewBag.LogError = "";
            return View("Login");
        }
        [HttpPostAttribute]
        [RouteAttribute("create")]
        public IActionResult Create(RegisterViewModel model, User newUser){
            if(ModelState.IsValid){
                User RegCheckEmail = _context.Users.Where(User => User.Email == newUser.Email).SingleOrDefault();
                if(RegCheckEmail == null){
                    newUser.CreatedAt = DateTime.Now;
                    newUser.UpdatedAt = DateTime.Now;
                    _context.Add(newUser);
                    _context.SaveChanges();
                    var CurrentUserId = newUser.Id;
                    HttpContext.Session.SetInt32("CurrentUser", (int)CurrentUserId);
                    return RedirectToAction ("Dashboard","Wedding");
                }
                else{
                    ViewBag.RegEmailError = "Email already used";
                }
            }
            else{
                ViewBag.RegEmailError = "";
            }
            ViewBag.errors = ModelState.Values;
            return View("Register");
        }
        [HttpPost]
        [RouteAttribute("login")]
        public IActionResult LoginUser(string Email, string Password, LoginViewModel model){
            if(ModelState.IsValid){
                User SignInUser = _context.Users.Where(User => User.Email == Email).SingleOrDefault();
                if(SignInUser != null && Password != null){
                    if(SignInUser.Password == Password){
                        HttpContext.Session.SetInt32("CurrentUser",(int)SignInUser.Id);
                        return RedirectToAction("Dashboard","Wedding");
                    }
                }
            }
            ViewBag.LogError = "Invalid Login";
            return View("Login");
        }
        [HttpGet]
        [RouteAttribute("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction ("Index");
        }
        [HttpGet]
        [RouteAttribute("{FirstName}/{id}")]
        public IActionResult ShowPerson(int id){
            if(HttpContext.Session.GetInt32("CurrentUser")==null){
                return RedirectToAction("Login","User");
            }
            User ShowOneUser = _context.Users.Where(User => User.Id == id)
                    .Include( reserve => reserve.Reserves)
                            .ThenInclude(w => w.Wedding)
                    .SingleOrDefault();
            ViewBag.ShowPerson = ShowOneUser;     
            return View();
        }
    }
}
