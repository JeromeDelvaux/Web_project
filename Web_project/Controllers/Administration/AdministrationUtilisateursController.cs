using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MesModels;

namespace Web_project.Controllers.Administration
{
    public class AdministrationUtilisateursController : Controller
    {
        public IActionResult ListUsers()
        {
            return Redirect(MesUrls.AdministrationUtilisateursListUsers);
        }
        public IActionResult ListeRoles()
        {
            return Redirect(MesUrls.AdministrationUtilisateursListeRoles);
        }
    }
}