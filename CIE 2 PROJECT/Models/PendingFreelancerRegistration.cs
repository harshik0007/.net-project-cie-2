using System;

namespace CIE_2_PROJECT.Models
{
    [Serializable]
    public sealed class PendingFreelancerRegistration
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string ProfileImage { get; set; }
    }
}
