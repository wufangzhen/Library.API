using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Models
{
    public class LoginToken
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }

        public string UserId { get; set; }
    }
}
