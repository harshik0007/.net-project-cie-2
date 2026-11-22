using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IUserService
    {
        LoginAccount GetLoginAccount(string emailOrUsername);
        bool UsernameOrEmailExists(string username, string email);
        int CreateAccount(string fullName, string username, string email, string passwordHash,
            string role, string phoneNumber, string profileImage, string skills);
        User GetUser(int userId);
        IList<User> GetUsersByRole(string role);
        void UpdateProfile(int userId, string fullName, string bio, string location, string skills,
            string education, string contactEmail, string phoneNumber, string organizationName,
            string portfolioUrl, string preferredPaymentMethod, string paymentAccountLabel,
            string profileImage);
    }
}
