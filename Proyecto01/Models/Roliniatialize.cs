using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Proyecto01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sem9_v2.Models
{
    public class RolInitialize
    {
        public static void Inicializar()
        {
            var rolManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));//Estos nos ayudan agregar lo procesos
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));//Estos nos ayudan agregar lo procesos

            //Roles prefeterminados
            List<string> roles = new List<string>();
            roles.Add("Admin");
            roles.Add("User");

            foreach (var rol in roles)
            {
                if (!rolManager.RoleExists(rol)) ;
                {
                    //Creo el rol
                    rolManager.Create(new IdentityRole(rol));
                }

            }
            //Usuario por defecto
            var adminUser = new ApplicationUser { UserName = "admin@ufide.ac.cr", Email = "admin@ufide.ac.cr" };
            string Contraseña = "Admin123";
            if (userManager.FindByEmail(adminUser.Email) == null)
            {
                var creacion = userManager.Create(adminUser, Contraseña);
                if (creacion.Succeeded)
                {
                    userManager.AddToRole(adminUser.Id, "Admin");
                }

            }
        }
    }
}