using System;

namespace mail_utils
{
    class MailUtilsException : Exception
    {
        public MailUtilsException(string Message) : base(Message) { }
    }
}
