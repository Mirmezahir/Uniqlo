using Microsoft.AspNetCore.Builder.Extensions;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using Uniqlo_New.Helpers;
using Uniqlo_New.Services.Abstracts;
using Microsoft.VisualBasic;

namespace Uniqlo_New.Services.Implements
{
	public class EmailService : IEmailService
	{
		readonly SmtpClient _client;
		readonly MailAddress _from;
		readonly HttpContext context;
		public EmailService(IOptions<SmtpOptions> option, IHttpContextAccessor acc)
		{
			var opt = option.Value;
			_client = new SmtpClient(opt.Host, opt.Port);
			_client.Credentials = new NetworkCredential(opt.Username, opt.Password);
			_client.EnableSsl = true;
			_from = new MailAddress(opt.Username, "Uniqlo");
	
			context = acc.HttpContext;
		}

		public void SendEmailConfirmation(string reciever, string name, string token)
		{
			MailAddress to = new(reciever);
			MailMessage message = new MailMessage(_from, to);
			message.Subject = "Confirm your email adress";
			message.Body = EmailTemplates.VerifyEmail;
			string url = context.Request.Scheme + "://" + context.Request.Host + "/Account/VerifyEmail?token=" + token+"&user="+name;
			message.Body = EmailTemplates.VerifyEmail.Replace("__$name", name).Replace("__$link", url);
		    message.IsBodyHtml = true;
			_client.Send(message);


		}
	}
}
