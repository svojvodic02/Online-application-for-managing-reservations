using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Web.Controllers
{
    public class UserController(ReservationsDbContext _dbContext) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Users.Add(model);
                _dbContext.SaveChanges();

                return RedirectToAction("Login", "User");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _dbContext.Users
                .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Neispravni podaci za prijavu.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserEmail", user.Email);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home"); 
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
                return RedirectToAction("Login");

            var user = _dbContext.Users
                .Include(u => u.Rezervacije)
                .ThenInclude(r => r.Stol) 
                .FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

        [HttpPost]
        public IActionResult EditProfile(User model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return View(model);
            }
                

            var userInDb = _dbContext.Users.FirstOrDefault(u => u.Id == model.Id);
            if (userInDb == null)
                return NotFound();

            userInDb.FirstName = model.FirstName;
            userInDb.LastName = model.LastName;
 

            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        public IActionResult All()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null || !user.IsAdmin)
            {
                return Forbid();
            }

            var users = _dbContext.Users.ToList();
            return View(users);
        }

        public IActionResult Details(int id)
        {
            var user = _dbContext.Users
                .Include(u => u.Rezervacije)
                .FirstOrDefault(u => u.Id == id);

            if (user == null) return NotFound();

            return View(user);
        }

        public IActionResult Delete(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return RedirectToAction("All");
        }

        public IActionResult Edit(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(int id, User model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            _dbContext.Update(model);
            _dbContext.SaveChanges();

            return RedirectToAction("All"); 
        }
    }
}
