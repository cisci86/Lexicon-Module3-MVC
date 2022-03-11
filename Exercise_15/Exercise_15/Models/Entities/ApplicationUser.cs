using Microsoft.AspNetCore.Identity;
#nullable disable
namespace Exercise_15.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Fullname => $"{Firstname} {Lastname}";
        ICollection<ApplicationUserGymClass> AttendingClasses { get; set; }
    }
}
