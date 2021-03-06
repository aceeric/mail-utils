﻿using System;
using Logging;
using static mail_utils.Globals;
using System.Runtime.CompilerServices;

namespace mail_utils
{
    /// <summary>
    /// Examines the settings and if the application is running as a console application, sends messages to the console,
    /// otherwise sends messages to the logging store indicated by the settings. Extends the base class Logger
    /// </summary>

    class MyLogger : Logger
    {
        /// <summary>
        /// Initialize logging level based on settings. If the optional logging level is not specified on the command line then the default
        /// is Informational, which logs everything
        /// </summary>

        public void InitLoggingSettings()
        {
            Level = LogLevel.Information;

            if (AppSettingsImpl.LogLevel.Initialized)
            {
                switch (AppSettingsImpl.LogLevel.Value.ToString().ToLower())
                {
                    case "err":
                        Level = LogLevel.Error;
                        break;
                    case "warn":
                        Level = LogLevel.Warning;
                        break;
                }
            }

            Output = LogOutput.ToConsole; // output to console unless otherwise specified on the command line

            if (AppSettingsImpl.Log.Initialized)
            {
                switch (((string)AppSettingsImpl.Log).ToLower())
                {
                    // since Console is default, only take action if that is changed by the user
                    case "file":
                        Output = LogOutput.ToFile;
                        break;
                    case "db":
                        Output = LogOutput.ToDatabase;
                        break;
                }
            }
            if (AppSettingsImpl.JobID.Initialized)
            {
                // The settings parser ensures that the GUID is valid so this is safe
                GUID = Guid.Parse(AppSettingsImpl.JobID);
            }
            JobName = "load-file";
        }

        /// <summary>
        /// Invokes the base class constructor
        /// </summary>

        public MyLogger() : base() { }
    }
}
