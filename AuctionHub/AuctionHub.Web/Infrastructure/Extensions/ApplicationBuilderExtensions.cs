namespace AuctionHub.Web.Infrastructure.Extensions
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<AuctionHubDbContext>().Database.Migrate();

                //var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                //var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                //
                //Task
                //  .Run(async () =>
                //  {
                //      var adminName = WebConstants.AdministratorRole;
                //
                //      var roles = new[]
                //      {
                //          adminName
                //      };
                //
                //      foreach (var role in roles)
                //      {
                //          var roleExists = await roleManager.RoleExistsAsync(role);
                //
                //          if (!roleExists)
                //          {
                //              await roleManager.CreateAsync(new IdentityRole
                //              {
                //                  Name = role
                //              });
                //          }
                //      }
                //
                //      var adminEmail = "admin@mysite.com";
                //
                //      var adminUser = await userManager.FindByEmailAsync(adminEmail);
                //
                //      if (adminUser == null)
                //      {
                //          adminUser = new User
                //          {
                //              Email = adminEmail,
                //              UserName = adminName,
                //              Name = adminName,
                //              Birthdate = DateTime.UtcNow
                //          };
                //
                //          await userManager.CreateAsync(adminUser, "admin12");
                //
                //          await userManager.AddToRoleAsync(adminUser, adminName);
                //      }
                //  })
                //  .Wait();
            }

            return app;
        }

        /// <summary>
        /// This is a workaround for missing seed data functionality in EF 7.0-rc1
        /// More info: https://github.com/aspnet/EntityFramework/issues/629
        /// </summary>
        /// <param name="app">
        /// An instance that provides the mechanisms to get instance of the database context.
        /// </param>
        public static void SeedData(this IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                AuctionHubDbContext db = serviceScope.ServiceProvider.GetService<AuctionHubDbContext>();

                Town initialTown = db.Towns.FirstOrDefault(t => t.Name == "Other");
                if (initialTown == null)
                {
                    db.Towns.Add(new Town()
                    {
                        Name = "Other"
                    });
                }
                db.SaveChanges();
            }
        }
    }
}
