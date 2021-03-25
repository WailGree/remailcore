using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RemailCore.Library.Models
{
    public class Email
    {
        [Key] public int Id { get; set; }
        [Required] public bool Unread { get; set; }
        [Required] [MaxLength(100)] public string From { get; set; }
        [Required] [MaxLength(200)] public string Subject { get; set; }
        [Required] public DateTime Date { get; set; }
        [Required] [MaxLength] public string Body { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(24)")]
        public int UId { get; set; }


        public Email(bool seen, string sender, string subject, DateTime date, string body, int uId)
        {
            Unread = !seen;
            From = sender.Replace(@"""", String.Empty);
            Subject = subject;
            Date = date;
            Body = body;
            UId = uId;
        }

        public Email()
        {
        }
    }
}