using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mail_utils
{
    class Enums
    {
        /// <summary>
        /// Defines the exit codes supported by the utility
        /// </summary>

        public enum ExitCode : int
        {
            /// <summary>
            /// Indicates successful completion
            /// </summary>
            Success = 0,
            /// <summary>
            /// Indicates that invalid settings or command line args were provided
            /// </summary>
            InvalidParameters = 1,
            /// <summary>
            /// Indicates no source files were found matching source path & file specifier
            /// </summary>
            NoSourceFilesFound = 2,
            /// <summary>
            /// Indicates an issue with any of: source path, source file, dest path, or dest file
            /// </summary>
            InvalidPathSpec = 3,
            /// <summary>
            /// Indicates an issue with any aspect of formulating a message and sending it via SMTP
            /// </summary>
            SMTPError = 4,
            /// <summary>
            /// Indicates that some other error occurred
            /// </summary>
            OtherError = 99
        }
    }
}
