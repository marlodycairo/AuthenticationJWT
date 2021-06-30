using AuthenticationJWT.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationJWT
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = true;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            //Controle si la audiencia será validada durante la validación del token.
                            //Asegurarse de que el destinatario del token esté autorizado para recibirlo.
                            ValidateAudience = true,
                            //Controle si el emisor será validado durante la validación del token.
                            //Validar el servidorr que creó ese token
                            ValidateIssuer = true,
                            //Verifica que la clave utilizada para firmar el token entrante sea parte de una lista de claves confiables.
                            ValidateIssuerSigningKey = true,
                            //Comprueba que el token no esté caducado y que la clave de firma del emisor sea válida.
                            ValidateLifetime = false,
                            ValidAudience = Configuration["Jwt:Issuer"],
                            
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                        };
                    });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ConnectionDB"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthenticationJWT", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Autentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                {
                    { securityScheme, new string[] { }}
                });

                //var basicSecurityScheme = new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.Http,
                //    Scheme = "basic",
                //    Reference = new OpenApiReference { Id = "BasicAuth", Type = ReferenceType.SecurityScheme }
                //};

                //c.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                //{
                //    { basicSecurityScheme, new string[] { } }
                //});
                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    //**Bearer token authentication**
                //    BearerFormat = "JWT",
                //    Description = "Autorización con Bearer y token JWT",
                //    In = ParameterLocation.Header,
                //    Name = "Authorization",
                //    Scheme = "bearer",
                //    Type = SecuritySchemeType.Http
                //});
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            //**Make sure swagger UI requires a Bearer token specified**
                //            Reference = new OpenApiReference
                //            {
                //                Id = JwtBearerDefaults.AuthenticationScheme,
                //                Type = ReferenceType.SecurityScheme
                //            }
                //        }, Array.Empty<string>()
                //    }
                //});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthenticationJWT v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
