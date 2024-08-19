using Demo.DAL.Entities;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Utility
{
	public static class MailSetting
	{
		public static void SendEmail( Email email)
		{
			//mail service Gmail

			var client = new SmtpClient("smtp.gmail.com",587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("atalaat635@gmail.com", "ktdmahglwrtmnrox");

			client.Send("atalaat635@gmail.com", email.Recipient, email.Subject, email.Body);

		}
	}
}
