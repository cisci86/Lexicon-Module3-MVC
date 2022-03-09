using Microsoft.AspNetCore.Identity;

namespace Exercise_15.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        ICollection<ApplicationUserGymClass> AttendingClasses { get; set; }
    }
}
