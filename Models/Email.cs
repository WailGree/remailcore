﻿using System;
using System.ComponentModel.DataAnnotations;
using MailKit;

namespace RemailCore.Models
{
    public class Email
    {
        public int Id { get; set; }
        [Required] public bool Seen { get; set; }
        [Required] [MaxLength(100)] public string Sender { get; set; }
        [Required] [MaxLength(100)] public string Subject { get; set; }
        [Required] public DateTime Date { get; set; }
        [Required] [MaxLength] public string Body { get; set; }
        [Required] public UniqueId UId { get; set; }


        public Email(bool seen, string sender, string subject, DateTime date, string body, UniqueId uId)
        {
            Seen = seen;
            Sender = sender.Replace(@"""", String.Empty);
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