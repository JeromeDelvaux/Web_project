using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Models.Pagination;
using IdentityServer.ViewsModels;
using IdentityServer.ViewsModels.Administration;
using MesModels.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MesModels;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServer.Controllers
{
    [Authorize(Roles = "Administrateur")]
    public class AdministrationUtilisateursController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdministrationUtilisateursController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> ListUsers(int? pageNumber)
        {
            try
            {
                var users = userManager.Users;
                int pageSize = 3;
                return View(await PaginatedList<User>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                User user = await userManager.FindByIdAsync(id);

                if (user != null)
                {
                    UserDetailsViewModel userDetailsViewModels = new UserDetailsViewModel()
                    {
                        User = user,
                        PageTitle = "Details Utilisateur"
                    };
                    return View(userDetailsViewModels);
                }
                ErrorViewModel Error = new ErrorViewModel("L'Id lié à cet utilisateur n'a pas été trouvé");
                return View("Error", Error);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }

        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                User user = await userManager.FindByIdAsync(id);

                UserEditViewModel UserEditViewModel = new UserEditViewModel();

                UserEditViewModel.User = user;
                return View(UserEditViewModel);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model, string id)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    User user = await userManager.FindByIdAsync(id);

                    user.Nom = model.User.Nom;
                    user.Prenom = model.User.Prenom;
                    user.Sexe = model.User.Sexe;
                    user.Email = model.User.Email;
                    user.PhoneNumber = model.User.PhoneNumber;
                    user.DateNaissance = model.User.DateNaissance;
                    user.Professionel = model.User.Professionel;


                    if (model.Password != null && model.ConfirmPassword != null && model.NewPassword != null)
                    {
                        var Valide = await signInManager.CheckPasswordSignInAsync(user, model.Password, true);
                        if (Valide.Succeeded)
                        {
                            var modifPass = await userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                            return RedirectToAction("details", new { id = model.Id });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Mot de passe invalide");
                            return View(model);
                        }

                }

                var result = await userManager.UpdateAsync(user);
                return RedirectToAction("details", new { id = model.Id });
            }
            return View();
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }
        }
        [HttpGet]
        public IActionResult CreateRole()
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
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = model.NomDuRole
                    };

                    IdentityResult result = await roleManager.CreateAsync(identityRole);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListeRoles", "AdministrationUtilisateurs");
                    }

                    foreach (IdentityError error in result.Errors)
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
        public IActionResult ListeRoles()
        {
            try
            {
                var roles = roleManager.Roles;
                return View(roles);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Le rôle avec l'id {id} n'existe pas";
                    return View("NotFound");
                }
                var model = new EditRoleViewModel
                {
                    Id = role.Id,
                    NomduRole = role.Name,
                };

                foreach (var user in userManager.Users)
                {
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        model.Users.Add(user.UserName);
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
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(model.Id);
                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Le rôle avec l'id {model.Id} n'existe pas";
                    return View("NotFound");
                }
                else
                {
                    role.Name = model.NomduRole;
                    var result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListeRoles", "AdministrationUtilisateurs");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            try
            {
                ViewBag.roleId = roleId;
                var role = await roleManager.FindByIdAsync(roleId);
                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Le rôle avec l'id {roleId} n'existe pas";
                    return View("NotFound");
                }
                var model = new List<UserRoleViewModel>();
                foreach (var user in userManager.Users)
                {
                    var userRoleViewModel = new UserRoleViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRoleViewModel.IsSelected = true;
                    }
                    else
                    {
                        userRoleViewModel.IsSelected = false;
                    }
                    model.Add(userRoleViewModel);
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
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            try
            {
                ViewBag.roleId = roleId;
                var role = await roleManager.FindByIdAsync(roleId);
                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Le rôle avec l'id {roleId} n'existe pas";
                    return View("NotFound");
                }
                for (int i = 0; i < model.Count; i++)
                {
                    var user = await userManager.FindByIdAsync(model[i].UserId);
                    IdentityResult result = null;
                    if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!model[i].IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }
                    if (result.Succeeded)
                    {
                        if (i < (model.Count - 1))
                        { continue; }
                        else
                        { return RedirectToAction("EditRole", new { Id = roleId }); }
                    }
                }
                return RedirectToAction("EditRole", new { Id = roleId });
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    ViewBag.ErrorMessage = $"L'utilisateur avec l'id {id} n'existe pas";
                    return View("NotFound");
                }
                else
                {
                    var result= await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListUsers");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ListUsers");
                }
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel(ex.Message);
                return View("Error", erreur);
            }

        }
    }
}