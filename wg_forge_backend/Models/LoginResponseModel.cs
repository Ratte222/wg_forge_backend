using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wg_forge_backend.Models
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; }
        public string UserName { get; set; }
    }
}
