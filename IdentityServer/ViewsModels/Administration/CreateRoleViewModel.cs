using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.ViewsModels.Administration
{
    public class CreateRoleViewModel
    {
        [Required]
        public string NomDuRole { get; set; }
    }
}
