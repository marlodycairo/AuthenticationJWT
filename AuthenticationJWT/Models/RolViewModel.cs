using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationJWT.Models
{
    public class RolViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public string Nombre { get; set; }
    }
}
