using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Rent.Core.Common.Configuration;
using Rent.Core.Contracts.Helpers;

namespace Rent.Core.Helpers
{
    public class MailHelper : IMailHelper
	{
		private readonly MailSettings _mailSettings;

		public MailHelper(
			MailSettings mailSettingsData)
		{
			_mailSettings = mailSettingsData;
		}

		public async Task SendMessageAsync(string subject, string mailMessage, List<string> mailTo, List<FileData> attachmentFiles = null)
		{
			if (string.IsNullOrWhiteSpace(_mailSettings.Host))
			{
				throw new Exception("В настройках системы не задан хост для отправки почты");
			}

			if (mailTo.Count == 0)
			{
				return;
			}

			using var message = new MailMessage();

			message.From = new MailAddress(_mailSettings.Email);
			foreach (var mailToStr in mailTo)
			{
				message.Bcc.Add(new MailAddress(mailToStr));
			}

			message.IsBodyHtml = true;
			message.BodyEncoding = Encoding.UTF8;
			message.SubjectEncoding = Encoding.UTF8;
			message.Subject = subject;
			message.Body = mailMessage;

			var msList = new List<MemoryStream>();
			if (attachmentFiles != null)
			{
				foreach (var attachFiles in attachmentFiles)
				{
					var ms = new MemoryStream();
					ms.Write(attachFiles.Content, 0, attachFiles.Content.Length);
					ms.Position = 0;
					var fileName = attachFiles.Name + "." + attachFiles.Extension;
					message.Attachments.Add(new Attachment(ms, fileName));
					msList.Add(ms);
				}
			}

			using var client = new SmtpClient(_mailSettings.Host);
			client.Credentials = new NetworkCredential(_mailSettings.Login, _mailSettings.Password);
			client.EnableSsl = _mailSettings.IsSendMailWithSSL;
			client.Port = _mailSettings.Port;
			await client.SendMailAsync(message);

			foreach (var ms in msList)
			{
				ms.Close();
				ms.Dispose();
			}
			message.Dispose();
			client.Dispose();
		}
	}
}
