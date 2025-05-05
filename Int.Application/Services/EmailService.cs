using Int.Domain.Services.Contrct;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Int.Application.Services
{
	public class EmailService : IEmailService
	{
		private readonly IConfiguration _config;
		public EmailService(IConfiguration config)
		{
			_config = config;
		}

		public async Task SendEmailAsync(string to, string subject, string body)
		{
			var client = new SmtpClient(_config["SmtpSettings:Host"], int.Parse(_config["SmtpSettings:Port"]))
			{
				Credentials = new NetworkCredential(_config["SmtpSettings:UserName"], _config["SmtpSettings:Password"]),
				EnableSsl = bool.Parse(_config["SmtpSettings:EnableSsl"])
			};
			Console.WriteLine($"Trying to send email...");
			Console.WriteLine($"Host: {_config["SmtpSettings:Host"]}");
			Console.WriteLine($"Port: {_config["SmtpSettings:Port"]}");
			Console.WriteLine($"Email: {_config["SmtpSettings:UserName"]}");

			var mailMessage = new MailMessage
			{
				From = new MailAddress(_config["SmtpSettings:UserName"]),
				Subject = subject,
				Body = body,
				IsBodyHtml = true
			};
			mailMessage.To.Add(to);

			await client.SendMailAsync(mailMessage);
			Console.WriteLine("✅ Email sent successfully");

		}
	}


}
