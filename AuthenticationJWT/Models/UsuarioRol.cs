using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationJWT.Models
{
    public class UsuarioRol
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int RolId { get; set; }
    }
}
