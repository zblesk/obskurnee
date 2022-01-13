namespace Obskurnee.Models
{
    public static class BookclubClaims
    {
        public const string Operation = "operation";
        public const string UserId = "UserId";
    }

    public static class BookclubRoles
    {
        /// <summary>
        /// Site admin
        /// </summary>
        public const string Admin = "admin";
        /// <summary>
        /// Moderator
        /// </summary>
        public const string Moderator = "moderator";
        /// <summary>
        /// Standard user
        /// </summary>
        public const string Bookworm = "bookworm";
        /// <summary>
        /// Read-only bot
        /// </summary>
        public const string Bot = "bot";
    }
}
