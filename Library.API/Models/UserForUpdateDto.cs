using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Models
{
    public class UserForUpdateDto
    {
        public string userName { get; set; }
        public string email { get; set; }
        public bool emailConfirmed { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string phoneNumber { get; set; }
        public bool phoneNumberConfirmed { get; set; }
    }
 
}
