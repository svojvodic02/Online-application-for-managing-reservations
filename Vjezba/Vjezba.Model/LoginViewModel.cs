using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email je obavezan")]
        [EmailAddress(ErrorMessage = "Neispravan email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
