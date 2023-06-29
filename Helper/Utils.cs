namespace LearnApiWeb.Helper
{
    public class Utils
    {
        public static DateTime ConvertUnixTimeToDateTimeUTC(long secondSinceUnixEcho)
        {
            DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return d.AddSeconds(secondSinceUnixEcho);
        }
    }
}