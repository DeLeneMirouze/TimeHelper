#region using
using System;
#endregion

#region Note aux développeurs
/* ----------------------------------------------------------------------------------------------------------
 * http://blog.dezfowler.com/2010/07/utc-gotchas-in-net-and-sql-server.html
 * ----------------------------------------------------------------------------------------------------------*/
#endregion

namespace TimeHelper
{
    /// <summary>
    /// Helper time methods
    /// Conversion between local date to UTC date
    /// Conversion to Unix format
    ///</summary>
    public static class TimeHelper
    {
        #region FromUtcToLocal
        ///<summary>
        /// Convert an UTC date to a date converted into the local timezone
        ///</summary>
        ///<param name="timeZone">Id of the local date timezone</param>
        ///<param name="dateTimeUtc">Date UTC</param>
        ///<returns></returns>
        public static DateTime FromUtcToLocal(string timeZone, DateTime dateTimeUtc)
        {
            if (dateTimeUtc.Kind == DateTimeKind.Unspecified)
            {
                dateTimeUtc = DateTime.SpecifyKind(dateTimeUtc, DateTimeKind.Utc);
            }

            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            var date = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, timeZoneInfo);

            return date;
        }
        #endregion

        #region FromLocalToUtc
        ///<summary>
        /// Convert a local date to UTC
        ///</summary>
        ///<param name="timeZone">Id of the local date timezone</param>
        ///<param name="dateTimeLocale">local date</param>
        ///<returns>date UTC</returns>
        public static DateTime FromLocalToUtc(string timeZone, DateTime dateTimeLocale)
        {
            // local date could be different than the system date
            dateTimeLocale = DateTime.SpecifyKind(dateTimeLocale, DateTimeKind.Unspecified);

            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            var date = TimeZoneInfo.ConvertTimeToUtc(dateTimeLocale, timeZoneInfo);

            return date;
        }
        #endregion

        #region GetLocalNow
        ///<summary>
        /// Get current date (server side) expressed in the provided timezone. This is the date that should be displau=yed
        ///</summary>
        ///<param name="timeZone">Id of the local date timezone</param>
        ///<returns>Date dans le format local pour affichage</returns>
        public static DateTime GetLocalNow(string timeZone)
        {
            DateTime dateTime = DateTime.UtcNow;

            return FromUtcToLocal(timeZone, dateTime);
        }
        #endregion

        #region ToIso8601
        ///<summary>
        /// Serialize provided <see cref="DateTime"/> to ISO 8601
        ///</summary>
        ///<param name="date">DateTime to serializer</param>
        ///<returns>DateTime in ISO 8601</returns>
        public static string ToIso8601(DateTime date)
        {
            // http://fr.wikipedia.org/wiki/ISO_8601
            // ex: 2012-03-28T12:08:16.2277876+02:00
            return date.ToString(Iso8601Format);
        }
        #endregion

        #region GetOffsetString
        ///<summary>
        /// Get the formated offset of the local date timezone from UTC
        ///</summary>
        ///<param name="date">Date provided</param>
        ///<param name="timeZone">Id of the local date timezone</param>
        ///<returns></returns>
        public static string GetOffsetString(DateTime date, string timeZone)
        {
            // could not find an easier method

            TimeZoneInfo currentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            var offset = currentTimeZone.GetUtcOffset(date);
            var formated = string.Format("{0}:{1}", offset.Hours, offset.Minutes);
            if (offset < TimeSpan.Zero)
            {
                formated = string.Concat("-", formated);
            }
            else
            {
                formated = string.Concat("+", formated);
            }

            return formated;
        }
        #endregion

        #region GetOffset
        ///<summary>
        /// Get the formated offset of the local date timezone from UTC
        ///</summary>
        ///<param name="date">Date provided</param>
        ///<param name="timeZone">Id of the local date timezone</param>
        ///<returns></returns>
        public static int GetOffset(DateTime date, string timeZone)
        {
            // could not find an easier method
            TimeZoneInfo currentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

            var offset = currentTimeZone.GetUtcOffset(date);

            return offset.Hours;
        }
        #endregion

        #region DateTimeToUnixTimestamp
        /// <summary>
        /// Convert a date to an Unix timestamp
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            long unixTimestamp = dateTime.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }
        #endregion

        ///<summary>
        /// Format ISO 8601 utilisé
        ///</summary>
        public const string Iso8601Format = @"yyyy-MM-ddTHH\:mm\:ss.fffffffzzz";
    }

}
