using System.Collections.Generic;
using System.Net.Mail;

namespace RemailCore.Interfaces
{
    public interface IBackupable
    {
        void Backup();
        IEnumerable<MailMessage> Restore();
    }
}