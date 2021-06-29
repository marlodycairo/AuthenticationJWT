using AuthenticationJWT.Context;
using AuthenticationJWT.Encryptado;
using AuthenticationJWT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public UsuariosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Usuarios> ObtenerUsuarios()
        {
            return context.Users.ToList();
        }

        [HttpPost]
        public ActionResult<Usuarios> CrearUsuarios(Usuarios usuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }
            usuarios.Pass = Encrypt.ObtenerSHA256(usuarios.Pass);
            
            context.Users.Add(usuarios);
            context.SaveChanges();

            return Ok(usuarios);
        }
    }
}
