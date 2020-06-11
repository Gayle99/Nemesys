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
                roleManager.CreateAsync(new IdentityRole("Administrator")).Wait();
            }

        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var reporter = new IdentityUser()
                {
                    Email = "charles@gmail.com",
                    NormalizedEmail = "CHARLES@GMAIL.COM",
                    UserName = "Charles",
                    NormalizedUserName = "CHARLES",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                IdentityResult result = userManager.CreateAsync(reporter, "D4!z]Mqa").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(reporter, "Reporter").Wait();
                }

                var reporter2 = new IdentityUser()
                {
                    Email = "annie@hotmail.com",
                    NormalizedEmail = "ANNIE@HOTMAIL.COM",
                    UserName = "Annie",
                    NormalizedUserName = "ANNIE",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                IdentityResult result2 = userManager.CreateAsync(reporter2, "V5!gazr]").Result;
                if (result2.Succeeded)
                {
                    userManager.AddToRoleAsync(reporter2, "Reporter").Wait();
                }

                var reporter3 = new IdentityUser()
                {
                    Email = "max@hotmail.com",
                    NormalizedEmail = "MAX@HOTMAIL.COM",
                    UserName = "Max",
                    NormalizedUserName = "MAX",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                IdentityResult result3 = userManager.CreateAsync(reporter3, "T!Vzjg]e").Result;
                if (result3.Succeeded)
                {
                    userManager.AddToRoleAsync(reporter3, "Reporter").Wait();
                }


                var investigator = new IdentityUser()
                {
                    Email = "patrick@gmail.com",
                    NormalizedEmail = "PATRICK@GMAIL.COM",
                    UserName = "Patrick",
                    NormalizedUserName = "PATRICK",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                result = userManager.CreateAsync(investigator, "?3ATu=").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(investigator, "Investigator").Wait();
                }

                var investigator2 = new IdentityUser()
                {
                    Email = "evelynn@yahoo.com",
                    NormalizedEmail = "EVELYNN@YAHOO.COM",
                    UserName = "Evelynn",
                    NormalizedUserName = "EVELYNN",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                result2 = userManager.CreateAsync(investigator2, "F4?1!e").Result;
                if (result2.Succeeded)
                {
                    userManager.AddToRoleAsync(investigator2, "Investigator").Wait();
                }

                var admin = new IdentityUser()
                {
                    Email = "nemesys@gmail.com",
                    NormalizedEmail = "NEMESYS@GMAIL.COM",
                    UserName = "NemesysAdmin",
                    NormalizedUserName = "NEMESYSADMIN",
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
                var reporter = userManager.GetUsersInRoleAsync("Reporter").Result.ElementAtOrDefault(0);
                var reporter2 = userManager.GetUsersInRoleAsync("Reporter").Result.ElementAtOrDefault(1);
                var reporter3 = userManager.GetUsersInRoleAsync("Reporter").Result.ElementAtOrDefault(2);
                context.AddRange(
                    new Report()
                    {
                        Title = "Electrical wire hanging from ceiling",
                        DateOfCreation = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        CreatedBy = reporter,
                        Location = "Faculty of ICT",
                        DateSpotted = DateTime.UtcNow,
                        TypeOfHazard = "Dangerous hazard",
                        Description = "There is an electrical wire hanging from the ceiling at the Faculty of ICT which poses a risk if someone makes contact with it. This has been there for at least two days and nothing has been done about it.",
                        Status = "Open",
                        ImageUrl = "/images/img01.jpg",
                    },
                    new Report()
                    {
                        Title = "Construction worker working without safety helmet",
                        DateOfCreation = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        CreatedBy = reporter2,
                        Location = "Faculty of Engineering",
                        DateSpotted = DateTime.UtcNow,
                        TypeOfHazard = "Unsafe act",
                        Description = "A construction worker has been seen working without a safety helmet these last few days. We are creating this report because we believe that it is important for workers to be wearing safety equipment to not risk getting hurt on campus and we do not want something tragic to happen to this person.",
                        Status = "Open",
                        ImageUrl = "/images/img02.jpg"
                    },
                    
                    new Report()
                    {
                        Title = "Two construction workers spotted having chainsaw duels",
                        DateOfCreation = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        CreatedBy = reporter3,
                        Location = "Student Housing Area",
                        DateSpotted = DateTime.UtcNow,
                        TypeOfHazard = "Unsafe act",
                        Description = "Two workers were spotted fighting with chainsaws in front of the student housing over who will go to the toilet first as the rest of the toilets were occupied",
                        Status = "Open",
                        ImageUrl = "/images/img03.png"
                    }
                );
                context.SaveChanges();
            }
        }
}
}
