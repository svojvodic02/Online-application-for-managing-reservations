using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Stol
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int Broj_Stolica { get; set; }

        public ICollection<Rezervacija> Rezervacije { get; set; } = new List<Rezervacija>();

    }
}
