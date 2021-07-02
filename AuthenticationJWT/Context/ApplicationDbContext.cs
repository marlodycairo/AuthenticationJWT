using AuthenticationJWT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationJWT.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuarios> Users { get; set; }
        public DbSet<RolViewModel> Rol { get; set; }
        public DbSet<UsuarioRol> UsuarioRol { get; set; }
    }
}
