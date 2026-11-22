using System;
using System.Net.Mail;

namespace CIE_2_PROJECT.Models
{
    public sealed class User
    {
        public int UserID { get; private set; }
        public string FullName { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string Role { get; private set; }
        public string Bio { get; private set; }
        public string Location { get; private set; }
        public string ProfileImage { get; private set; }
        public string Skills { get; private set; }
        public string Education { get; private set; }
        public string ContactEmail { get; private set; }
        public string PhoneNumber { get; private set; }
        public string OrganizationName { get; private set; }
        public string PortfolioUrl { get; private set; }
        public string PreferredPaymentMethod { get; private set; }
        public string PaymentAccountLabel { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public User(
            int userId,
            string fullName,
            string username,
            string email,
            string role,
            string bio,
            string location,
            string profileImage,
            string skills,
            string education,
            string contactEmail,
            string phoneNumber,
            string organizationName,
            string portfolioUrl,
            string preferredPaymentMethod,
            string paymentAccountLabel,
            DateTime createdAt)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException("userId");
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("A user's full name is required.", "fullName");
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("A username is required.", "username");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("An email address is required.", "email");
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("A user role is required.", "role");

            UserID = userId;
            FullName = fullName.Trim();
            Username = username.Trim();
            Email = email.Trim();
            Role = role.Trim();
            Bio = CleanOptional(bio);
            Location = CleanOptional(location);
            ProfileImage = CleanOptional(profileImage);
            Skills = CleanOptional(skills);
            Education = CleanOptional(education);
            ContactEmail = CleanOptional(contactEmail);
            PhoneNumber = CleanOptional(phoneNumber);
            OrganizationName = CleanOptional(organizationName);
            PortfolioUrl = CleanOptional(portfolioUrl);
            PreferredPaymentMethod = CleanOptional(preferredPaymentMethod);
            PaymentAccountLabel = CleanOptional(paymentAccountLabel);
            CreatedAt = createdAt;
        }

        public void UpdateProfile(
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
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("A full name is required.", "fullName");
            if (fullName.Trim().Length > 100)
                throw new ArgumentException("Full name must be 100 characters or fewer.", "fullName");
            if (bio != null && bio.Trim().Length > 500)
                throw new ArgumentException("Bio must be 500 characters or fewer.", "bio");
            if (location != null && location.Trim().Length > 100)
                throw new ArgumentException("Location must be 100 characters or fewer.", "location");
            if (skills != null && skills.Trim().Length > 500)
                throw new ArgumentException("Skills must be 500 characters or fewer.", "skills");
            if (education != null && education.Trim().Length > 250)
                throw new ArgumentException("Education must be 250 characters or fewer.", "education");
            if (contactEmail != null && contactEmail.Trim().Length > 100)
                throw new ArgumentException("Contact email must be 100 characters or fewer.", "contactEmail");
            if (!string.IsNullOrWhiteSpace(contactEmail))
            {
                try
                {
                    MailAddress parsedEmail = new MailAddress(contactEmail.Trim());
                    if (!string.Equals(parsedEmail.Address, contactEmail.Trim(), StringComparison.OrdinalIgnoreCase))
                        throw new ArgumentException("Enter a valid contact email address.", "contactEmail");
                }
                catch (FormatException)
                {
                    throw new ArgumentException("Enter a valid contact email address.", "contactEmail");
                }
            }
            if (phoneNumber != null && phoneNumber.Trim().Length > 30)
                throw new ArgumentException("Phone number must be 30 characters or fewer.", "phoneNumber");
            if (organizationName != null && organizationName.Trim().Length > 150)
                throw new ArgumentException("Organization must be 150 characters or fewer.", "organizationName");
            if (portfolioUrl != null && portfolioUrl.Trim().Length > 500)
                throw new ArgumentException("Portfolio link must be 500 characters or fewer.", "portfolioUrl");
            if (preferredPaymentMethod != null && preferredPaymentMethod.Trim().Length > 40)
                throw new ArgumentException("Payment method must be 40 characters or fewer.", "preferredPaymentMethod");
            if (!string.IsNullOrWhiteSpace(preferredPaymentMethod) &&
                !string.Equals(preferredPaymentMethod.Trim(), "UPI", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(preferredPaymentMethod.Trim(), "Bank transfer", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(preferredPaymentMethod.Trim(), "PayPal", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(preferredPaymentMethod.Trim(), "Other", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Choose one of the available payment methods.", "preferredPaymentMethod");
            if (paymentAccountLabel != null && paymentAccountLabel.Trim().Length > 120)
                throw new ArgumentException("Payment reference must be 120 characters or fewer.", "paymentAccountLabel");
            if (profileImage != null && profileImage.Trim().Length > 255)
                throw new ArgumentException("Profile image path is too long.", "profileImage");
            if (!string.IsNullOrWhiteSpace(portfolioUrl))
            {
                Uri parsedUrl;
                if (!Uri.TryCreate(portfolioUrl.Trim(), UriKind.Absolute, out parsedUrl) ||
                    (parsedUrl.Scheme != Uri.UriSchemeHttp && parsedUrl.Scheme != Uri.UriSchemeHttps))
                    throw new ArgumentException("Portfolio link must start with http:// or https://.", "portfolioUrl");
            }

            FullName = fullName.Trim();
            Bio = CleanOptional(bio);
            Location = CleanOptional(location);
            Skills = CleanOptional(skills);
            Education = CleanOptional(education);
            ContactEmail = CleanOptional(contactEmail);
            PhoneNumber = CleanOptional(phoneNumber);
            OrganizationName = CleanOptional(organizationName);
            PortfolioUrl = CleanOptional(portfolioUrl);
            PreferredPaymentMethod = CleanOptional(preferredPaymentMethod);
            PaymentAccountLabel = CleanOptional(paymentAccountLabel);
            ProfileImage = CleanOptional(profileImage);
        }

        private static string CleanOptional(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
