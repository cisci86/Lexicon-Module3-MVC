using Microsoft.AspNetCore.Identity;

namespace Exercise_15.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ApplicationUserGymClass> GymClasses { get; set; }
    }
}
