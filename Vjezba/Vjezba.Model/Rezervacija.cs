using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Rezervacija
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Ime { get; set; }

        [Required]
        public string Prezime { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Mobilni_broj { get; set; }

        [ForeignKey(nameof(Stol))]
        public int? Id_Stol { get; set; }

        [ForeignKey(nameof(User))]
        public int Id_Korisnika { get; set; }

        [Required]
        public DateOnly Datum_Rezervacije { get; set; }

        [Required]
        public TimeOnly Vrijeme_Rezervacije { get; set; }

        [Required]
        public TimeSpan Vrijeme_Trajanja_Rezervacije { get; set; }  
        public User? User { get; set; }

        public Stol? Stol { get; set; }
    }
}
