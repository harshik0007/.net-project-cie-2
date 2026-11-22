using CIE_2_PROJECT.Models;
using System.Collections.Generic;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IUserRepository
    {
        User GetById(int userId);
        LoginAccount GetLoginAccount(string emailOrUsername);
        bool UsernameOrEmailExists(string username, string email);
        int CreateAccount(string fullName, string username, string email, string passwordHash,
            string role, string phoneNumber, string profileImage, string skills);
        IList<User> GetByRole(string role);
        void UpdateProfile(User user);
    }
}
