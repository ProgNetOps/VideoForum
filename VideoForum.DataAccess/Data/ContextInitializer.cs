using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoForum.Core.Entities;
using VideoForum.Utility;

namespace VideoForum.DataAccess.Data;

//This class is responsible for updating the database; migrations, seeding the database etc
public static class ContextInitializer
{
    public static async Task InitializeAsync(AppDbContext? appDbContext,
        UserManager<AppUser>? userManager,
        RoleManager<AppRole>? roleManager)
    {
        //Check if there is any pending migration and effect db migration
        if (appDbContext?.Database.GetPendingMigrations().Count() > 0)
        {
            appDbContext.Database.Migrate();
        }

        //Check if there is no role and create some roles
        if (roleManager?.Roles.Any() is false)
        {
            foreach (var role in SD.Roles)
            {
                await roleManager.CreateAsync(new AppRole { Name = role });
            }
        }
        
        //Check if there is no user and create some users, assigning roles to them
        if (userManager?.Users.Any() is false)
        {
            AppUser admin = new()
            {
                Name = "Admin",
                Email = "admin@example.com",
                UserName = "admin"
            };
            await userManager.CreateAsync(admin, "Password123");

            //assign roles to the admin user
            await userManager.AddToRolesAsync(admin, [SD.AdminRole, SD.ModeratorRole, SD.UserRole]);



            AppUser john = new()
            {
                Name = "John",
                Email = "john@example.com",
                UserName = "john"
            };
            await userManager.CreateAsync(john, "Password123");

            //assign roles to the john user
            await userManager.AddToRoleAsync(john, SD.UserRole);



            AppUser mary = new()
            {
                Name = "Mary",
                Email = "mary@example.com",
                UserName = "mary"
            };
            await userManager.CreateAsync(mary, "Password123");

            //assign roles to the mary user
            await userManager.AddToRoleAsync(mary, SD.ModeratorRole);
        }


    }





}