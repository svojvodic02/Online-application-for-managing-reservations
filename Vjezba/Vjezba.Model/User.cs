using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Unesite barem 3 znaka")]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string? Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsAdmin { get; set; } = false;

        public ICollection<Rezervacija> Rezervacije { get; set; } = new List<Rezervacija>();
    }
}
