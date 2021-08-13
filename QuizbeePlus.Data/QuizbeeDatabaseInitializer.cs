using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QuizbeePlus.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizbeePlus.Data
{
    public class QuizbeeDatabaseInitializer : CreateDatabaseIfNotExists<QuizbeeContext>
    {
        protected override void Seed(QuizbeeContext context)
        {
            //add these roles to database if does not exists
            SeedRoles(context);

            //add these users to database if not exists
            SeedUsers(context);
        }
        
        public void SeedRoles(QuizbeePlus.Data.QuizbeeContext context)
        {
            List<IdentityRole> rolesInQuizbee = new List<IdentityRole>();

            rolesInQuizbee.Add(new IdentityRole() { Name = "Administrator" });
            rolesInQuizbee.Add(new IdentityRole() { Name = "User" });
            rolesInQuizbee.Add(new IdentityRole() { Name = "MockUser" });

            var rolesStore = new RoleStore<IdentityRole>(context);
            var rolesManager = new RoleManager<IdentityRole>(rolesStore);
            
            foreach (IdentityRole role in rolesInQuizbee)
            {
                if (!rolesManager.RoleExists(role.Name))
                {
                    var result = rolesManager.Create(role);

                    if (result.Succeeded) continue;
                }
            }
        }

        public void SeedUsers(QuizbeePlus.Data.QuizbeeContext context)
        {
            var usersStore = new UserStore<QuizbeeUser>(context);
            var usersManager = new UserManager<QuizbeeUser>(usersStore);

            QuizbeeUser admin = new QuizbeeUser();
            admin.Email = "admin@email.com";
            admin.UserName = "admin";
            var password = "admin123";

            if (usersManager.FindByEmail(admin.Email) == null)
            {
                var result = usersManager.Create(admin, password);

                if(result.Succeeded)
                {
                    //add necessary roles to admin
                    usersManager.AddToRole(admin.Id, "Administrator");
                    usersManager.AddToRole(admin.Id, "User");
                }
            }


            // Seed Mock User 
            //QuizbeeUser mockuser = new QuizbeeUser();
            //admin.Email = "mock@email.com";
            //admin.UserName = "mockuser";
            //var passwordmock = "mockuser123";

            //if (usersManager.FindByEmail(admin.Email) == null)
            //{
            //    var result = usersManager.Create(mockuser, passwordmock);

            //    if (result.Succeeded)
            //    {
            //        //add necessary roles to admin
            //        usersManager.AddToRole(admin.Id, "mockuser");                   
            //    }
            //}
        }
    }
}