using System;
using MailKit;

namespace RemailCore.Models
{
    public class Email
    {
        public bool Unread { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public UniqueId UId { get; set; }


        public Email(bool seen, string sender, string subject, DateTime date, string body, UniqueId uId)
        {
            Unread = !seen;
            From = sender.Replace(@"""", String.Empty);
            Subject = subject;
            Date = date;
            Body = body;
            UId = uId;
        }

        public Email() { }

    }
}
