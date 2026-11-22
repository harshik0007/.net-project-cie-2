using System;
using System.Collections.Generic;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository users;

        public UserService(IUserRepository users)
        {
            if (users == null)
                throw new ArgumentNullException("users");
            this.users = users;
        }

        public LoginAccount GetLoginAccount(string emailOrUsername)
        {
            if (string.IsNullOrWhiteSpace(emailOrUsername))
                throw new ArgumentException("Enter your email address or username.", "emailOrUsername");
            return users.GetLoginAccount(emailOrUsername.Trim());
        }

        public bool UsernameOrEmailExists(string username, string email)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("A username is required.", "username");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("An email address is required.", "email");
            return users.UsernameOrEmailExists(username.Trim(), email.Trim());
        }

        public int CreateAccount(string fullName, string username, string email, string passwordHash,
            string role, string phoneNumber, string profileImage, string skills)
        {
            return users.CreateAccount(fullName, username, email, passwordHash,
                role, phoneNumber, profileImage, skills);
        }

        public User GetUser(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException("userId");
            return users.GetById(userId);
        }

        public IList<User> GetUsersByRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("A user role is required.", "role");
            return users.GetByRole(role.Trim());
        }

        public void UpdateProfile(
            int userId,
            string fullName,
            string bio,
            string location,
            string skills,
            string education,
            string contactEmail,
            string phoneNumber,
            string organizationName,
            string portfolioUrl,
            string preferredPaymentMethod,
            string paymentAccountLabel,
            string profileImage)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException("userId");

            User user = users.GetById(userId);
            if (user == null)
                throw new InvalidOperationException("The profile could not be found.");

            user.UpdateProfile(fullName, bio, location, skills, education, contactEmail,
                phoneNumber, organizationName, portfolioUrl, preferredPaymentMethod,
                paymentAccountLabel, profileImage);
            users.UpdateProfile(user);
        }
    }
}
