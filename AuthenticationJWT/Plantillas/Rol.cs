using AuthenticationJWT.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationJWT.Plantillas
{
    public class Rol
    {
        private readonly ApplicationDbContext context;

        public Rol(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<string> ObtenerRolesPorUsuarios(int userId)
        {
            return (from userRol in context.UsuarioRol
                    join rol in context.Rol
                    on userRol.RolId equals rol.Id
                    where userRol.UsuarioId == userId
                    select rol.Nombre).ToList();
        }
    }
}
