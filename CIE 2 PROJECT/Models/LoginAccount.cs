namespace CIE_2_PROJECT.Models
{
    public sealed class LoginAccount
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PasswordHash { get; set; }
    }
}
