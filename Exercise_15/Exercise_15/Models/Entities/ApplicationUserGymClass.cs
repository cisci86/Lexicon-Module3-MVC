﻿namespace Exercise_15.Models.Entities
{
    public class ApplicationUserGymClass
    {
        public int GymClassId { get; set; }
        public string ApplicationUserId { get; set; }

        public GymClass GymClass { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
