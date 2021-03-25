using System.Collections.Generic;
using System.Net.Mail;

namespace RemailCore.Library.Interfaces
{
    public interface IBackupable
    {
        void Backup();
        IEnumerable<MailMessage> Restore();
    }
}