namespace EM.Common
{
    public class GlobalConstants
    {
        public const long MaxImageSize = 2L * 1024 * 1024 * 1024;

        public const int HomePageResponseCacheDuration = 5 * 60;

        public const byte PaginationDisplayPages = 5;

        public const string VenueImagesFolder = "Venues";

        public const string EventImagesFolder = "Events";

        public const string CaseInsensitiveDefaultCollation = "SQL_Latin1_General_CP1_CI_AS";

        public static TimeSpan SessionCookieIdleTimeout { get; } = TimeSpan.FromMinutes(30);

        public static TimeSpan DisplayTicketAfterExpiration { get; } = TimeSpan.FromMinutes(30);
    }
}
