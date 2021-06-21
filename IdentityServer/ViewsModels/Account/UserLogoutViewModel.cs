using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.ViewsModels.Account
{
    public class UserLogoutViewModel : UserLogoutInputViewModel
    {
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}
