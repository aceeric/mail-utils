using System.Runtime.CompilerServices;

namespace mail_utils
{
    /// <summary>
    /// Contains global methods and fields
    /// </summary>

    class Globals
    {
        /// <summary>
        /// Copyright notice for the utility
        /// </summary>

        public static string Copyright = "Copyright (C) 2020 Eric Ace";

        /// <summary>
        /// The instance of the logger for the utility
        /// </summary>

        public static MyLogger Log = new MyLogger();

        /// <summary>
        /// The implementation of application settings functionality for the utility
        /// </summary>

        public static AppSettingsImpl Cfg = new AppSettingsImpl();
    }
}
