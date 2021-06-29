using AuthenticationJWT.Encryptado;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationJWT.Models
{
    public class Usuarios
    {
        //public Usuarios()
        //{
        //    this.Id = Guid.NewGuid();
        //}
        
        [ScaffoldColumn(false)]
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "usuario no válido")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password, ErrorMessage = "contraseña no válida.")]
        public string Pass { get; set; }
    }
}
