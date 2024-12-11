using System.Reflection.Metadata.Ecma335;

namespace Uniqlo_New.Helpers
{
	public class SmtpOptions
	{
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}
