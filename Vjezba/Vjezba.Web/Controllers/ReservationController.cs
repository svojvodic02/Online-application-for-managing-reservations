using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Vjezba.Web.Controllers
{
    public class ReservationController(ReservationsDbContext _dbContext) : Controller
    {
        public IActionResult Index(DateOnly? date, TimeOnly? time)
        {
            var stolovi = _dbContext.Stolovi
                .Include(s => s.Rezervacije)
                .ToList();

            ViewBag.SelectedDate = date;
            ViewBag.SelectedTime = time;

            return View(stolovi);

        }

        [HttpGet]
        public IActionResult Form(int id, string date, string time)
        {
            DateOnly.TryParse(date, out var dateOnly);
            TimeOnly.TryParse(time, out var timeOnly);

            var model = new Rezervacija
            {
                Id_Stol = id,
                Datum_Rezervacije = dateOnly,
                Vrijeme_Rezervacije = timeOnly
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Form(Rezervacija model)
        {

            int? id = HttpContext.Session.GetInt32("UserId");

            model.Id_Korisnika = (int)id;

            if (ModelState.IsValid)
            {
                model.Id = 0;
                _dbContext.Rezervacije.Add(model);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult All(string search, string sortOrder)
        {
            var currentUserEmail = HttpContext.Session.GetString("UserEmail");
            var currentUser = _dbContext.Users.FirstOrDefault(u => u.Email == currentUserEmail);

            if (currentUser == null || !currentUser.IsAdmin)
            {
                return Forbid();
            }

            ViewData["DateSort"] = System.String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["UserSort"] = sortOrder == "user" ? "user_desc" : "user";
            ViewData["CurrentFilter"] = search;

            var reservations = _dbContext.Rezervacije
                .Include(r => r.User)
                .Include(r => r.Stol)
                .AsQueryable();


            if (!string.IsNullOrEmpty(search))
            {
                reservations = reservations.Where(r =>
                    r.User.Email.Contains(search) ||
                    r.User.FirstName.Contains(search) ||
                    r.User.LastName.Contains(search) ||
                    r.Datum_Rezervacije.ToString().Contains(search));
            }

            reservations = sortOrder switch
            {
                "date_desc" => reservations.OrderByDescending(r => r.Datum_Rezervacije),
                "user" => reservations.OrderBy(r => r.User.Email),
                "user_desc" => reservations.OrderByDescending(r => r.User.Email),
                _ => reservations.OrderBy(r => r.Datum_Rezervacije)
            };

            return View(reservations.ToList());
        }

        public IActionResult Details(int id)
        {
            var res = _dbContext.Rezervacije
                .Include(r => r.User)
                .Include(r => r.Stol)
                .FirstOrDefault(r => r.Id == id);

            if (res == null) return NotFound();

            return View(res);
        }

        public IActionResult Delete(int id)
        {
            var res = _dbContext.Rezervacije.FirstOrDefault(r => r.Id == id);
            if (res == null) return NotFound();

            _dbContext.Rezervacije.Remove(res);
            _dbContext.SaveChanges();

            return RedirectToAction("All");
        }

        public IActionResult Edit(int id)
        {
            var rez = _dbContext.Rezervacije
                              .Include(r => r.User)
                              .Include(r => r.Stol)
                              .FirstOrDefault(r => r.Id == id);

            if (rez == null)
                return NotFound();

            ViewBag.Users = new SelectList(
                _dbContext.Users.Select(u => new {
                    u.Id,
                    FullName = u.FirstName + " " + u.LastName
                }),
                "Id", "FullName", rez.Id_Korisnika);

            ViewBag.Stolovi = new SelectList(
                _dbContext.Stolovi.Select(s => new {
                    s.Id,
                    Label = s.Broj_Stolica + " stolica"
                }),
                "id", "Label", rez.Id_Stol);

            return View(rez);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Rezervacija model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
               
                ViewBag.Users = new SelectList(_dbContext.Users, "Id", "FirstName", model.Id_Korisnika);
                ViewBag.Stolovi = new SelectList(_dbContext.Stolovi, "Id", "Naziv", model.Id_Stol);
                return View(model);
            }

            _dbContext.Update(model);
            _dbContext.SaveChanges();

            return RedirectToAction("All");
        }



    }
}
