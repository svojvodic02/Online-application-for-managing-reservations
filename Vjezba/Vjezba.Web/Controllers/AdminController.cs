using Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;

namespace Vjezba.Web.Controllers
{
    public class AdminController(ReservationsDbContext _dbContext) : Controller
    {

        public IActionResult Index()
        {
            var currentUserEmail = HttpContext.Session.GetString("UserEmail");
            var currentUser = _dbContext.Users.FirstOrDefault(u => u.Email == currentUserEmail);

            if (currentUser == null || !currentUser.IsAdmin)
            {
                return Forbid();
            }

            ViewBag.UserCount = _dbContext.Users.Count();
            ViewBag.ReservationCount = _dbContext.Rezervacije.Count();

            return View();
        }
    }
}
