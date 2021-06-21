using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MesModels.ClassesEnum;
using Microsoft.AspNetCore.Identity;

namespace MesModels.Models
{
    public class User : IdentityUser
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public SexeUser Sexe { get; set; }
        public override string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }
        public bool Professionel { get; set; }
    }
}
