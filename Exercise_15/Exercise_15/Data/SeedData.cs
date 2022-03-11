using Bogus;
using Exercise_15.Helper;
using Exercise_15.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Exercise_15.Data
{
    public static class SeedData
    {
        private static ApplicationDbContext _context;
        private static RoleManager<IdentityRole> roleManager;
        private static UserManager<ApplicationUser> userManager;
        private static Faker faker = new Faker();

        public static async Task Start(ApplicationDbContext context, IServiceProvider service, string adminPW)
        {
            if (string.IsNullOrEmpty(adminPW)) throw new ArgumentNullException("Password is null! Check config!");
            if (context == null) throw new ArgumentNullException(nameof(ApplicationDbContext));

            _context = context;

            //if (await userManager.GetUsersInRoleAsync(RoleHelper.GetAdminRole()) != null)
            //{
            //    return;
            //}

            if (_context.Users.Any())
            {
                var adminRoleId = _context.Roles.FirstOrDefault(r => r.Name == "Admin").Id;
                var anyAdmins = await _context.UserRoles.AnyAsync(ur => ur.RoleId == adminRoleId);
                if (anyAdmins) return;
            }
            


            roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            if (roleManager == null) throw new NullReferenceException(nameof(RoleManager<IdentityRole>));

            userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
            if (userManager == null) throw new NullReferenceException(nameof(UserManager<ApplicationUser>));
            var classes = new[]
            {
                "Zumba",
                "Pilate's",
                "Hot Yoga",
                "Yin Yoga",
                "Spinning",
                "Tennis",
                "Squash",
                "Padel",
                "Afro power dance",
                "Dance"
            };
            var gymClasses = GetGymClasses(classes);
            await _context.AddRangeAsync(gymClasses);

            var roles = new[] { RoleHelper.GetMemberRole(), RoleHelper.GetAdminRole() };
            await AddRoles(roles);

            var admin = await GetAdmin("admin@gym.com", adminPW);

            await AddAdminToAdminRole(admin, roles);

            await _context.SaveChangesAsync();

        }


        private static IEnumerable<GymClass> GetGymClasses(string[] classes)
        {
            var gymClasses = new List<GymClass>();

            foreach(var classname in classes)
            {
                var gymclass = new GymClass
                {
                    Name = classname,
                    StartTime = DateTime.Now.AddDays(faker.Random.Int(0, 30)),
                    Duration = new TimeSpan(0, 45, 0),
                    Description = faker.Lorem.Text()
                };

                gymClasses.Add(gymclass);
            }
            return gymClasses;
        }

        private static async Task AddRoles(string[] roles)
        {
            foreach(var role in roles)
            {
                if (await roleManager.RoleExistsAsync(role)) continue;
                var newRole = new IdentityRole { Name = role };
                var result = await roleManager.CreateAsync(newRole);

                if(!result.Succeeded) throw new Exception(String.Join("\n", result.Errors));
            }
        }
        private static async Task<ApplicationUser> GetAdmin(string email, string PW)
        {
            var admin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Firstname = faker.Name.FirstName(),
                Lastname = faker.Name.LastName(),
               
        };
            _context.Entry(admin).Property(PropertyHelper.GetTimeOfRegistration()).CurrentValue = DateTime.Now;
            var result = await userManager.CreateAsync(admin, PW);
            if (!result.Succeeded) throw new Exception(String.Join("\n", result.Errors));

            return admin;
        }

        private static async Task AddAdminToAdminRole(ApplicationUser admin, string[] roles)
        {
            if(admin == null) throw new NullReferenceException(nameof(admin));

            var adminRole = roles.Where(role => role == RoleHelper.GetAdminRole()).FirstOrDefault();

            if (await userManager.IsInRoleAsync(admin, adminRole)) return;

            var result = await userManager.AddToRoleAsync(admin, adminRole);
            if(!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        }
    }
}
