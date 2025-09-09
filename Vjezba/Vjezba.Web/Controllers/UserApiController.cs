using Microsoft.AspNetCore.Mvc;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Web.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserApiController : Controller
    {

        private ReservationsDbContext _dbDontext;
        public UserApiController(ReservationsDbContext dbContext)
        {
            this._dbDontext = dbContext;
        }
        public IActionResult Get()
        {
            var clients = this._dbDontext.Users
                .Select(c => new UserDTO
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Password = c.Password,
                    Email = c.Email,
                    CreatedAt = c.CreatedAt,
                    IsAdmin = c.IsAdmin
                })
                .ToList();
            return Ok(clients);
        }

        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var client = this._dbDontext.Users
                .Where(c => c.Id == id)
                .Select(c => new UserDTO
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Password = c.Password,
                    Email = c.Email,
                    CreatedAt = c.CreatedAt,
                    IsAdmin = c.IsAdmin
                })
                .FirstOrDefault();

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [Route("pretraga/{q}")]
        public IActionResult Get(string q)
        {
            var client = this._dbDontext.Users
                .Where(c => c.FirstName.Contains(q) || c.LastName.Contains(q))
                .Select(c => new UserDTO
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Password = c.Password,
                    Email = c.Email,
                    CreatedAt = c.CreatedAt,
                    IsAdmin = c.IsAdmin
                })
                .FirstOrDefault();
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            this._dbDontext.Users.Add(user);
            this._dbDontext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put(int id, [FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            var userDb = this._dbDontext.Users.First(c => c.Id == id);

            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Password = user.Password;
            userDb.Email = user.Email;
            userDb.CreatedAt = user.CreatedAt;
            userDb.IsAdmin = user.IsAdmin;
            this._dbDontext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var userDb = this._dbDontext.Users.First(c => c.Id == id);
            if (userDb == null)
            {
                return NotFound();
            }
            this._dbDontext.Users.Remove(userDb);
            this._dbDontext.SaveChanges();
            return Get();
        }
    }


    public class UserDTO
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public string? Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsAdmin { get; set; } = false;

    }
}
