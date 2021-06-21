using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MesModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web_project.Controllers.Utilisateurs
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            //Local davant Redirect permet de faire uniquement des redirections en local.Protège des attaques(redirection vers l'exterieur)
            return Redirect(MesUrls.IdentityServerAccountRegister);
        }
        [Authorize]
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult Details()
        {
            return Redirect(MesUrls.IdentityServerAccountDetails);
        }
        [Authorize]
        public IActionResult Edit()
        {
            return Redirect(MesUrls.IdentityServerAccountEdit);
        }
        
    }
}