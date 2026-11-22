namespace CIE_2_PROJECT.Helpers
{
    public static class TestUserContext
    {
        // Compatibility for existing page handlers while the app moves off demo identities.
        public static int ClientID { get { return AuthenticatedUser.UserID; } }
        public static int FreelancerID { get { return AuthenticatedUser.UserID; } }
    }
}
