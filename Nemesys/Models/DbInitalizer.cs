using Microsoft.AspNetCore.Identity;
using Nemesys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public class DbInitalizer
    {
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole("Reporter")).Wait();
                roleManager.CreateAsync(new IdentityRole("Investigator")).Wait();
                roleManager.CreateAsync(new IdentityRole("Admistrator")).Wait();
            }

        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var reporter = new IdentityUser()
                {
                    Email = "a@b.c",
                    NormalizedEmail = "A@B.C",
                    UserName = "a@b.c",
                    NormalizedUserName = "A@B.C",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                IdentityResult result = userManager.CreateAsync(reporter, "R>v8XX").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(reporter, "Reporter").Wait();
                }


                var investigator = new IdentityUser()
                {
                    Email = "b@b.c",
                    NormalizedEmail = "B@B.C",
                    UserName = "b@b.c",
                    NormalizedUserName = "B@B.C",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                result = userManager.CreateAsync(investigator, "?3ATu=").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(investigator, "Investigator").Wait();
                }

                var admin = new IdentityUser()
                {
                    Email = "bb@b.c",
                    NormalizedEmail = "BB@B.C",
                    UserName = "bb@b.c",
                    NormalizedUserName = "BB@B.C",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                result = userManager.CreateAsync(admin, "D4!z]M").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, "Administrator").Wait();
                }
            }
        }

        public static void SeedData(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            if (!context.Reports.Any())
            {
                var reporter = userManager.GetUsersInRoleAsync("Reporter").Result.FirstOrDefault();
                context.AddRange(
                    new Report()
                    {
                        Title = "This is a report",
                        DateOfCreation = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        CreatedBy = reporter,
                        Location = "Faculty of ICT",
                        DateSpotted = DateTime.UtcNow,
                        TypeOfHazard = "Unsafe act",
                        Description = "Something something",
                        Status = "Open",
                        ImageUrl = "/images/img01.jpg",
                    },
                    new Report()
                    {
                        Title = "This is another report",
                        DateOfCreation = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        CreatedBy = reporter,
                        Location = "Faculty of Engineering",
                        DateSpotted = DateTime.UtcNow,
                        TypeOfHazard = "Structure",
                        Description = "Something something",
                        Status = "Open",
                        ImageUrl = "/images/img02.jpg"
                    },
                    new Report()
                    {
                        Title = "You know what this is",
                        DateOfCreation = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        CreatedBy = reporter,
                        Location = "Faculty of Faculties",
                        DateSpotted = DateTime.UtcNow,
                        TypeOfHazard = "Unsafe act ",
                        Description = "Something something",
                        Status = "Open",
                        ImageUrl = "/images/img03.jpg"
                    }
                );
                context.SaveChanges();
            }
        }
}
}
