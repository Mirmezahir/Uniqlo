using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Uniqlo_New.ViewModels.Auths
{
    public class LoginVM
    {

        public string UserNameOrEmail { get; set; } 
   
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
