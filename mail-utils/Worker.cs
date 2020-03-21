using CryptLib;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace mail_utils
{
    class Worker
    {
        /// <summary>
        /// Builds and sends an email message using command-line settings
        /// </summary>
        /// <returns>true if the message was successfully sent</returns>
        public static bool DoWork()
        {
            try
            {
                MimeMessage Message = CreateMessage();
                using (SmtpClient Client = new SmtpClient())
                {
                    Client.Connect(AppSettingsImpl.Server, AppSettingsImpl.Port, GetSSLOption());
                    Client.Authenticate(AppSettingsImpl.Encrypted ? Crypto.Unprotect(AppSettingsImpl.User) : AppSettingsImpl.User,
                        AppSettingsImpl.Encrypted ? Crypto.Unprotect(AppSettingsImpl.Pass) : AppSettingsImpl.Pass);
                    Client.Send(Message);
                    Environment.ExitCode = (int)Enums.ExitCode.Success;
                    return true;
                }
            }
            catch (Exception e)
            {
                Globals.Log.ErrorMessage("An error occurred attempting to send the email: {0}", e.Message);
                Environment.ExitCode = (int)Enums.ExitCode.SMTPError;
                return false;
            }
        }

        /// <summary>
        /// Parses the command-line "SSL Type" arg and translates it into a SecureSocketOptions enum value
        /// </summary>
        /// <returns>The SecureSocketOptions enum value matching the command line</returns>
        private static SecureSocketOptions GetSSLOption()
        {
            return string.IsNullOrEmpty(AppSettingsImpl.SSLType) || ((string)AppSettingsImpl.SSLType).Equals("auto", StringComparison.OrdinalIgnoreCase)
                ? SecureSocketOptions.Auto
                : SecureSocketOptions.SslOnConnect;
        }

        /// <summary>
        /// Parses the "TO", "FROM", "SUBJECT", and "BODY" command line args and creates a MimeMessage instance from them
        /// </summary>
        /// <returns>The initialized MimeMessage instance</returns>
        private static MimeMessage CreateMessage()
        {
            MimeMessage Message = new MimeMessage();
            Message.To.AddRange(Recipients());
            Message.From.Add(MakeAddress(AppSettingsImpl.From));
            Message.Subject = AppSettingsImpl.Subject;
            TextPart Body = new TextPart("plain")
            {
                Text = AppSettingsImpl.Body
            };
            if (AppSettingsImpl.Attachment.Initialized)
            {
                MimePart Attachment = new MimePart("text", "plain")
                {
                    Content = new MimeContent(File.OpenRead(AppSettingsImpl.Attachment), ContentEncoding.Default),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = Path.GetFileName(AppSettingsImpl.Attachment)
                };
                Multipart MultipartBody = new Multipart("mixed");
                MultipartBody.Add(Body);
                MultipartBody.Add(Attachment);
                Message.Body = MultipartBody;
            }
            else
            {
                Message.Body = Body;
            }
            return Message;
        }

        /// <summary>
        /// Parses the command line "To" arg and builds a list of MailboxAddress instances from it
        /// </summary>
        /// <returns>the List, as described</returns>
        private static List<MailboxAddress> Recipients()
        {
            List<MailboxAddress> Addresses = new List<MailboxAddress>();
            string [] Recipients = ((string)AppSettingsImpl.To).Split(new char[] { ',' });
            foreach (string Recipient in Recipients)
            {
                Addresses.Add(MakeAddress(Recipient));
            }
            return Addresses;
        }

        /// <summary>
        /// From the passed address, creates a MimeKit MailboxAddress instance
        /// </summary>
        /// <param name="Address">Email address. Two forms are supported: "bare@domain.com" and "Alias&lt;bare@domain.com&gt;"</param>
        /// <returns>The populated instance</returns>
        private static MailboxAddress MakeAddress(string Address)
        {
            if (Address.Contains('<') || Address.Contains('>'))
            {
                // complex form
                MatchCollection mc = Regex.Matches(Address, @"(.*?)<(.*?)>", RegexOptions.IgnoreCase);
                if (mc.Count == 1 && mc[0].Groups.Count == 3)
                {
                    string Name = mc[0].Groups[1].Value;
                    string Addr = mc[0].Groups[2].Value;
                    return new MailboxAddress(Name.Trim(), Addr.Trim());
                }
                else
                {
                    throw new MailUtilsException(string.Format("Unable to parse To address element: {0}", Address));
                }
            }
            else
            {
                // simple form
                return new MailboxAddress(Address.Trim());
            }

        }
    }
}
