using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.ViewsModels.Account
{
    public class UserLoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool SeRappelerDeMoi { get; set; }
        public string ReturnUrl { get; set; }
    }
}
