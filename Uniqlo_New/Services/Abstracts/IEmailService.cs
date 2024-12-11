namespace Uniqlo_New.Services.Abstracts
{
	public interface IEmailService
	{
		public void SendEmailConfirmation(string reciever,string name,string token);
	}
}
