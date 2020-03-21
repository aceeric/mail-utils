using System;
using System.Text.RegularExpressions;

namespace mail_utils
{
    /// <summary>
    /// Various extension methods
    /// </summary>

    static class Extensions
    {
        /// <summary>
        /// Returns TRUE if the string is found in the specified list. This is a case-insensitive comparison
        /// </summary>
        /// <param name="this">string to search</param>
        /// <param name="List">list to search for this string in</param>
        /// <returns></returns>

        public static bool In(this string @this, params string[] List)
        {
            foreach (string s in List)
            {
                if (@this.ToLower() == s.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns TRUE if the string is a valid GUID. Otherwise returns FALSE
        /// </summary>
        /// <param name="this">The string to validate</param>
        /// <returns></returns>

        public static bool IsGuid(this string @this)
        {
            Guid TmpGuid;
            return Guid.TryParse(@this, out TmpGuid);
        }
    }
}
