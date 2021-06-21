using MesModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IdentityServer.ViewsModels
{
    public class UserDetailsViewModel
    {
        public User User { get; set; }
        public string PageTitle { get; set; }
    }
}
