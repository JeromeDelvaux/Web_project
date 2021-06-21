using MesModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.ViewsModels
{
    public class UserRegisterViewModel
    {
        public User User { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirmer votre mot de passe")]
        public string ConfirmPassword { get; set; }
        public bool RememberLogin { get; set; }
    }
}
