using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Contracts.Helpers
{
    public interface IMailHelper
    {
        public Task SendMessageAsync(string subject, string mailMessage, List<string> mailTo, List<FileData> attachmentFiles = null);
    }
}
