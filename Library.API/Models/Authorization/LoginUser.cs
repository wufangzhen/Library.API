using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;

namespace Library.API.Models
{
    public class LoginUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string ReturnUrl { get; set; } 
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

    }
}