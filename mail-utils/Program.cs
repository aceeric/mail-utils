using AppSettings;
using CryptLib;
using System;
using static mail_utils.Globals;

namespace mail_utils
{
    /// <summary>
    /// Entry point
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point. Does initialization and then calls the DoWork method to actually do the work
        /// </summary>
        /// <param name="args">provided by .Net</param>

        static void Main(string[] args)
        {
#if DEBUG
            args = new string[] {
              "-to", "Eric Ace <eric@zzzzzzzz.com>"
             ,"-from", "Eric Ace MailTest <eric@zzzzzzzzz.com>"
             ,"-body", "Attachment Test"
             ,"-subject", "This is a test of attachment functionality"
             ,"-attachment", ".\foo.txt"
             ,"-server", "smtp.zzzzzzz.com"
             ,"-port", "587"
             ,"-user", "0x...AN ENCRYPTED USERNAME GOES HERE..."
             ,"-pass", "0x...AN ENCRYPTED PASSWORD GOES HERE..."
             ,"-ssltype", "auto"
             ,"-encrypted"
//           ,"-encrypt"
             ,"-log", "con"
             ,"-loglevel", "info"
             ,"-jobid", "12345678-1234-1234-1234-123456789012"
            };
#endif
            try
            {
                if (!ParseArgs(args))
                {
                    Environment.ExitCode = (int)Enums.ExitCode.InvalidParameters;
                    return;
                }
                if (AppSettingsImpl.Encrypt)
                {
                    HandleEncryptDecrypt();
                    return;
                }
                Log.InitLoggingSettings();
                Log.InformationMessage("Started");
                if (DoValidations())
                {
                    if (Worker.DoWork()) // sets the exit code
                    {
                        Log.InformationMessage("Normal completion");
                    }
                    else
                    {
                        Log.InformationMessage("Abnormal completion");
                    }
                }
            }
            catch (Exception Ex)
            {
                if (Ex is MailUtilsException)
                {
                    Log.ErrorMessage(Ex.Message);
                }
                else
                {
                    Log.ErrorMessage("An unhandled exception occurred. The exception was: {0}. Stack trace follows:\n{1}", Ex.Message, Ex.StackTrace);
                }
                Log.InformationMessage("Abnormal completion");
                Environment.ExitCode = (int)Enums.ExitCode.OtherError;
            }
        }

        /// <summary>
        /// Stub: perform validations and set exit codes
        /// </summary>
        /// <returns>true if the utility can proceed, else false: the utility is unable to proceed</returns>

        public static bool DoValidations()
        {
            // TODO
            return true;
        }

        /// <summary>
        /// Parses command-line args. If there are parse errors, displays an error message. If required args are not provided, then
        /// displays usage instructions to the console.
        /// </summary>
        /// <param name="args">From .Net</param>
        /// <returns>True if all required args are specified, and no invalid values are supplied, else false.</returns>

        private static bool ParseArgs(string[] args)
        {
            if (!AppSettingsImpl.Parse(SettingsSource.CommandLine, args))
            {
                if (AppSettingsImpl.ParseErrorMessage != null)
                {
                    Console.WriteLine(AppSettingsImpl.ParseErrorMessage);
                }
                else
                {
                    AppSettingsImpl.ShowUsage();
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Handles when the -encrypt or -decrypt args are provided
        /// </summary>

        private static void HandleEncryptDecrypt()
        {
            if (string.IsNullOrEmpty(AppSettingsImpl.User) || string.IsNullOrEmpty(AppSettingsImpl.Pass))
            {
                Log.ErrorMessage("The -encrypt option was specified, but either one or both of the credential components were not provided");
                Environment.ExitCode = (int)Enums.ExitCode.InvalidParameters;
                return;
            }
            // regardless of the output setting (to log or console or db) write this to the console so it
            // is not persisted if the Decrypt function is specified

            Console.WriteLine("User ID:");
            Console.WriteLine(EncryptOrDecryptKey(AppSettingsImpl.User, AppSettingsImpl.Encrypt));
            Console.WriteLine("Password:");
            Console.WriteLine(EncryptOrDecryptKey(AppSettingsImpl.Pass, AppSettingsImpl.Encrypt));
        }

        /// <summary>
        /// If Encrypt == true, then encrypt and return the key. The Key arg is treated as plain text. Else
        /// decrypt and return the key. If decrypt, the Key arg is treated as encrypted coming in to the method.
        /// </summary>
        /// <param name="Key">The key to encrypt or decrypt</param>
        /// <param name="Encrypt">True to encrypt. False to decrypt</param>
        /// <returns>The encrypted or decrypted key value</returns>

        private static string EncryptOrDecryptKey(string Key, bool Encrypt)
        {
            return Encrypt ? Crypto.Protect(Key) : Crypto.Unprotect(Key);
        }
    }
}
