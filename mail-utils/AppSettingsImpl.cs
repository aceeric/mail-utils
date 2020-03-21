using AppSettings;
using System;

namespace mail_utils
{
    /// <summary>
    /// Extends the AppSettingsBase class with settings needed by the utility
    /// </summary>

    class AppSettingsImpl : AppSettingsBase
    {
        /// <summary>
        /// Email recipient
        /// </summary>
        public static StringSetting To { get { return (StringSetting)SettingsDict["To"]; } }

        /// <summary>
        /// Email sender
        /// </summary>
        public static StringSetting From { get { return (StringSetting)SettingsDict["From"]; } }

        /// <summary>
        /// Message body
        /// </summary>
        public static StringSetting Body { get { return (StringSetting)SettingsDict["Body"]; } }

        /// <summary>
        /// Subject
        /// </summary>
        public static StringSetting Subject { get { return (StringSetting)SettingsDict["Subject"]; } }

        /// <summary>
        /// Attachment
        /// </summary>
        public static StringSetting Attachment { get { return (StringSetting)SettingsDict["Attachment"]; } }

        /// <summary>
        /// SMTP account user ID
        /// </summary>
        public static StringSetting User { get { return (StringSetting)SettingsDict["User"]; } }

        /// <summary>
        /// SMTP account password
        /// </summary>
        public static StringSetting Pass { get { return (StringSetting)SettingsDict["Pass"]; } }

        /// <summary>
        /// SMTP server URL
        /// </summary>
        public static StringSetting Server { get { return (StringSetting)SettingsDict["Server"]; } }

        /// <summary>
        /// SMTP server port
        /// </summary>
        public static IntSetting Port { get { return (IntSetting)SettingsDict["Port"]; } }

        /// <summary>
        /// SSL connection type - either "auto" or "onconnect"
        /// </summary>
        public static StringSetting SSLType { get { return (StringSetting)SettingsDict["SSLType"]; } }

        /// <summary>
        /// True if the SMPT account credentials are encrypted on the command line. False if they are plain text
        /// </summary>
        public static BoolSetting Encrypted { get { return (BoolSetting)SettingsDict["Encrypted"]; } }

        /// <summary>
        /// True to encrypt the SMPT account credentials on the command line, display the encrypted values to to console and then exit
        /// </summary>
        public static BoolSetting Encrypt { get { return (BoolSetting)SettingsDict["Encrypt"]; } }

        /// <summary>
        /// Logging target (e.g. file, database, console)
        /// </summary>
        public static StringSetting Log { get { return (StringSetting)SettingsDict["Log"]; } }

        /// <summary>
        /// Specifies which type of events are logged
        /// </summary>
        public static StringSetting LogLevel { get { return (StringSetting)SettingsDict["LogLevel"]; } }

        /// <summary>
        /// Specifies the Job ID
        /// </summary>
        public static StringSetting JobID { get { return (StringSetting)SettingsDict["JobID"]; } }

        public new static string RegKey
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Initializes the instance with an array of settings that the utility supports, as well as usage instructions
        /// </summary>

        public AppSettingsImpl()
        {
            SettingList = new Setting[] {
                new StringSetting("To", "recipients", null,  Setting.ArgTyp.Mandatory, true, false,
                    "A comma-separated list of email recipients. Two forms are supported. The simple form is: user@domain.ext. " +
                    "E.g. \"yourbuddy@aol.com\". The complex form is: Display Name <user@domain.ext>. E.g. \"Joe Smith <joe.smith@domain.ext>\". " +
                    "The recipients in the comma-separated list can mix and match forms."),
                new StringSetting("From", "sender", null,  Setting.ArgTyp.Mandatory, true, false,
                    "A single sender in one of the forms supported for \"To\" addresses. This sender address will appear in the \"from\" address " +
                    "of the email."),
                new StringSetting("Body", "body", null,  Setting.ArgTyp.Mandatory, true, false,
                    "The message body, in plain text."),
                new StringSetting("Subject", "text", null,  Setting.ArgTyp.Optional, true, false,
                    "The message subject, in plain text."),
                new StringSetting("Attachment", "path", null,  Setting.ArgTyp.Optional, true, false,
                    "The path to a file to include as an attachment."),
                new StringSetting("Server", "server", null,  Setting.ArgTyp.Mandatory, true, false,
                    "The SMPT server URL. E.g. \"smtp.zzzzz.com\"."),
                new IntSetting("Port", "nnn", null,  Setting.ArgTyp.Mandatory, true, false,
                    "The SMPT port number to use."),
                new StringSetting("User", "userid", null,  Setting.ArgTyp.Mandatory, true, false,
                    "A user ID that has an account on the specified SMPT server. This is likely to be the same as the \"from\" arg (assuming " +
                    "you are using the utility to send mail on your own behalf.) But that is not a requirement."),
                new StringSetting("Pass", "password", null,  Setting.ArgTyp.Mandatory, true, false,
                    "The password of the specified user having an account on the specified SMPT server."),
                new StringSetting("SSLType", "auto|onconnect", null,  Setting.ArgTyp.Optional, true, false,
                    "Allowed literals are \"auto\" and \"onconnect\". If auto, then the SSL connection type is determined by the server. " +
                    "If onconnect, then the client attempts to initiate an SSL connection immediately, creating a secure channel within which " +
                    "to negotiate the rest of the SSL connection. If the server does not support this, then the connection fails. This is the most " +
                    "secure option because the entire handshake is encrypted. If \"auto\" is specified, the server may elect to perform part of the " +
                    "handshake in plain text."),
                new BoolSetting("Encrypted", false,  Setting.ArgTyp.Optional, true, false,
                    "If specified, then the user ID & password on the command line are taken to be encrypted values. (See the -encrypt " +
                    "option below.) The utility will decrypt the provided credentials when accessing the SMTP server. If not provided, " +
                    "then the user ID & password are interpreted as plain text values."),
                new BoolSetting("Encrypt", false,  Setting.ArgTyp.Optional, false, false,
                    "Encrypts the provided user ID and password. Displays the encrypted values to the console, and immediately exits with " +
                    "no further processing. This option is used to generate encrypted credentials to use subsequently with the utility."),
                new StringSetting("Log", "file|db|con", "con",  Setting.ArgTyp.Optional, true, false,
                    "Determines how the utility communicates errors, status, etc. If not supplied, then all output goes to the console. " +
                    "If \"file\" is specified, then the utility logs to a log file in the same directory that the utility is run from. " +
                    "The log file will be named load-file.log. " +
                    "If \"db\" is specified, then logging occurs to the database. If \"con\" is specified, then output goes to the console " +
                    "(same as if the arg were omitted.) If logging to file or db is specified then the utility runs silently " +
                    "with no console output. If db logging is specified, then the required logging components must be " +
                    "installed in the database. If the components are not installed and db logging is specified, then the utility " +
                    "will automatically fail over to file-based logging."),
                new StringSetting("LogLevel", "err|warn|info", "info",  Setting.ArgTyp.Optional, true, false,
                    "Defines the logging level. \"err\" specifies that only errors will be reported. \"warn\" means errors and warnings, " +
                    "and \"info\" means all messages. The default is \"info\"."),
                new StringSetting("JobID", "guid", null,  Setting.ArgTyp.Optional, true, false,
                    "Defines a job ID for the logging subsystem. A GUID value is supplied in the canonical 8-4-4-4-12 form. If provided, " +
                    "then the logging subsystem is initialized with the provided GUID. The default behavior is for the logging subsystem " +
                    "to generate its own GUID. Only takes effect if the database logging option is specified.")
            };

            Usage =
                "Provides SMTP email client functionality. The current release only sends email. Valid credentials are required in order to " +
                "connect to an SMPT server. These credentials are supplied on the command line in either encrypted - or plain text - form.";
        }

        /// <summary>
        /// Performs custom arg validation for the utility, after invoking the base class parser.
        /// </summary>
        /// <param name="Settings">A settings instance to parse</param>
        /// <param name="CmdLineArgs">Command-line args array</param>
        /// <returns>True if args are valid, else False</returns>

        public new static bool Parse(SettingsSource Settings, string[] CmdLineArgs = null)
        {
            if (AppSettingsBase.Parse(Settings, CmdLineArgs))
            {
                if (JobID.Initialized && !((string)JobID).IsGuid())
                {
                    ParseErrorMessage = "-jobid arg must be a GUID (nnnnnnnn-nnnn-nnnn-nnnn-nnnnnnnnnnnn)";
                    return false;
                }
                if (Attachment.Initialized && !System.IO.File.Exists(Attachment))
                {
                    ParseErrorMessage = "Attachment file does not exist or is not accessible: " + Attachment;
                    return false;
                }
                if (SSLType.Initialized && !SSLType.Value.In("auto", "onconnect"))
                {
                    ParseErrorMessage = "Unsupported value in the -ssltype arg: " + SSLType;
                    return false;
                }
                if (LogLevel.Initialized && !LogLevel.Value.In("info", "warn", "error"))
                {
                    ParseErrorMessage = "Unsupported value in the loglevel arg: " + LogLevel;
                    return false;
                }
                return true;
            }
            return false;
        }

        public static void Show()
        {
            Console.WriteLine(string.Format("To={0}", To.Value));
            Console.WriteLine(string.Format("From={0}", From.Value));
            Console.WriteLine(string.Format("Body={0}", Body.Value));
            Console.WriteLine(string.Format("Subject={0}", Subject.Value));
            Console.WriteLine(string.Format("Attachment={0}", Attachment.Value));
            Console.WriteLine(string.Format("User={0}", User.Value));
            Console.WriteLine(string.Format("Pass={0}", Pass.Value));
            Console.WriteLine(string.Format("Server={0}", Server.Value));
            Console.WriteLine(string.Format("Port={0}", Port.Value));
            Console.WriteLine(string.Format("SSLType={0}", SSLType.Value));
            Console.WriteLine(string.Format("Encrypted={0}", Encrypted.Value));
            Console.WriteLine(string.Format("Encrypt={0}", Encrypt.Value));
            Console.WriteLine(string.Format("LogLevel={0}", LogLevel.Value));
            Console.WriteLine(string.Format("JobID={0}", JobID.Value));

        }
    }
}
