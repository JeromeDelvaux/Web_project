using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.ViewsModels;
using IdentityServer.ViewsModels.Account;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using MesModels.Models;
using MesModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Services.DelegatedAuthorization;

namespace IdentityServer.Controllers
{
  
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 IIdentityServerInteractionService interaction,
                                 IClientStore clientStore,
                                 IAuthenticationSchemeProvider schemeProvider,
                                 IEventService events)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._interaction = interaction;
            this._clientStore = clientStore;
            this._schemeProvider = schemeProvider;
            this._events = events;
        }
        [HttpGet]
        public IActionResult Register()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new User
                    { UserName = model.User.Email, 
                      Email = model.User.Email,
                      Nom=model.User.Nom,
                      Prenom = model.User.Prenom,
                      Sexe = model.User.Sexe,
                      PhoneNumber = model.User.PhoneNumber,
                      DateNaissance = model.User.DateNaissance,
                      Professionel = model.User.Professionel

                    };
                    var result= await _userManager.CreateAsync(user, model.Password);

                    if(result.Succeeded)
                    {
                        if(user.Professionel==true)
                        {
                            await _userManager.AddToRoleAsync(user, "Gestionnaire");
                        }
                        await _userManager.AddToRoleAsync(user, "Utilisateur");
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    

                        return Redirect (MesUrls.webProjectAccountLogin);
                    }

                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel model, string button,string returnUrl)
        {
            try
            {
                var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
                if (button.Equals("S'identifier"))
                {
                    if (ModelState.IsValid)
                    {
                        var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.SeRappelerDeMoi, false);

                        if (result.Succeeded)
                            if (!string.IsNullOrEmpty(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return Redirect(MesUrls.webProjectHomeIndex); 
                            //return Redirect(model.ReturnUrl);
                            }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Utilisateur ou mot de passe invalide");
                        }
                        
                    }
                    return View(BuildLoginInputModel(model));
                }
                else
                {
                    if (context != null)
                    {
                        // if the user cancels, send a result back into IdentityServer as if they 
                        // denied the consent (even if this client does not require consent).
                        // this will send back an access denied OIDC error response to the client.
                        await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                        return Redirect(model.ReturnUrl);
                    }
                    else
                        return Redirect("~/");
                }
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }
        }
        public async Task<IActionResult> Logout(UserLogoutInputViewModel model)
        {
            try
            {
                // build a model so the logged out page knows what to display
                var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

                if (User?.Identity.IsAuthenticated == true)
                {
                    // delete local authentication cookie
                    await _signInManager.SignOutAsync();

                    // raise the logout event
                    await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
                }

                // check if we need to trigger sign-out at an upstream identity provider
                if (vm.TriggerExternalSignout)
                {
                    // build a return URL so the upstream provider will redirect back
                    // to us after the user has logged out. this allows us to then
                    // complete our single sign-out processing.
                    string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                    // this triggers a redirect to the external provider for sign-out
                    return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
                }

            return View("LoggedOut", vm);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            try
            {
                if (User?.Identity.IsAuthenticated == true)
                {
                var userId = User.GetSubjectId();
                User user = await _userManager.FindByIdAsync(userId);

                    if (user != null && user.Id == userId)
                    {
                        UserDetailsViewModel userDetailsViewModels = new UserDetailsViewModel()
                        {
                            User = user,
                            PageTitle = "Details Utilisateur"
                        };
                        return View(userDetailsViewModels);
                    }
                    else
                    {
                        ErrorViewModel ErreurProfil = new ErrorViewModel("Le profil auxquel vous essayez d'acceder n'est pas le vôtre");
                        return View("Error", ErreurProfil);
                    }
                }

                ErrorViewModel Erreur = new ErrorViewModel("Vous devez d'abord vous connecter afin d'avoir accès aux détails de votre compte utilisateur");
                return View("Error", Erreur);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }

        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            try
            {
                if (User?.Identity.IsAuthenticated == true)
                {
                    var userId = User.GetSubjectId();
                    User user = await _userManager.FindByIdAsync(userId);

                    if(user != null && user.Id == userId)
                    {
                        UserEditViewModel UserEditViewModel = new UserEditViewModel();

                        UserEditViewModel.User = user;
                        return View(UserEditViewModel);
                    }
                    else
                    {
                        ErrorViewModel Error = new ErrorViewModel("Le profil auxquel vous essayez d'acceder n'est pas le vôtre");
                        return View("Error", Error);
                    }
                }
                ErrorViewModel erreur = new ErrorViewModel("Veuillez vous connecter pour accéder à votre compte");
                return View("Error", erreur);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.GetSubjectId();
                    User user = await _userManager.FindByIdAsync(userId);

                    user.Nom = model.User.Nom;
                    user.Prenom = model.User.Prenom;
                    user.Sexe = model.User.Sexe;
                    user.Email = model.User.Email;
                    user.PhoneNumber = model.User.PhoneNumber;
                    user.DateNaissance = model.User.DateNaissance;
                    user.Professionel = model.User.Professionel;


                    if (model.Password != null && model.ConfirmPassword != null && model.NewPassword != null)
                    {
                        var Valide = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);
                        if (Valide.Succeeded)
                        {
                            var modifPass = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                            return RedirectToAction("details", new { id = model.Id });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Mot de passe invalide");
                            return View(model);
                        }

                    }

                    var result = await _userManager.UpdateAsync(user);
                    return RedirectToAction("details", new { id = model.Id });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                var userId = User.GetSubjectId();
                var user = await _userManager.FindByIdAsync(userId);
            
                if (user == null)
                {
                    ViewBag.ErrorMessage = $"L'utilisateur avec l'id {userId} n'existe pas";
                    return View("NotFound");
                }
                else
                {
                    await _signInManager.SignOutAsync();
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                    
                        return Redirect(MesUrls.webProjectHomeIndex);
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return Redirect(MesUrls.webProjectHomeIndex);
                }
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }

        }
        private UserLoginViewModel BuildLoginInputModel(UserLoginViewModel model)
        {

            UserLoginViewModel vm = new UserLoginViewModel();
            vm.ReturnUrl = model.ReturnUrl;
            vm.Username = model.Username;
            vm.SeRappelerDeMoi = model.SeRappelerDeMoi;
            return model;
        }
        private async Task<UserLogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new UserLogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {

            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }
            return vm;
        }
    }
}