using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MesModels.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_project.Models.Interfaces;
using Web_project.ViewsModels;

namespace Web_project.Controllers.Administration
{
    [Authorize(Roles = "Administrateur")]
    public class AdministrationNewsController : Controller
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly INewsRepository newsRepository;

        public AdministrationNewsController(IWebHostEnvironment hostingEnvironment, INewsRepository newsRepository)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.newsRepository = newsRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await newsRepository.GetAllNewsAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel() { RequestId = ex.Message };
                return View("Error", erreur);
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                AdministationNewsCreateViewModel model = new AdministationNewsCreateViewModel();
                return View(model);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel() { RequestId = ex.Message };
                return View("Error", erreur); ;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(AdministationNewsCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = ProcessUploadedImage(model.Photo);

                    News news = new News
                    {
                        DatePublication = DateTime.Now,
                        Titre = model.Titre,
                        TexteLibre = model.TexteLibre,
                        PathImage = uniqueFileName,
                        UserId = User.Claims.FirstOrDefault(x => x.Type.ToString() == "sub").Value
                    };
                    var idToken = await HttpContext.GetTokenAsync("access_token");
                    var result = await newsRepository.CreateNewsAsync(news, idToken);
                    TempData["Message"] = "CreateOk";
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    ErrorViewModel erreur = new ErrorViewModel() { RequestId = ex.Message };
                    return View("Error", erreur);
                }
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var news = await newsRepository.GetNewsAsync(id);
                if (news == null)
                {
                    ViewBag.ErrorMessage = $"La news avec l'id {id} n'existe pas";
                    return View("NotFound");
                }
                else
                {
                    var idToken = await HttpContext.GetTokenAsync("access_token");

                    //Suppression du l'image sur le serveur
                    if (news.PathImage != null)
                    {
                        string filePathLogo = Path.Combine(hostingEnvironment.WebRootPath, "images", news.PathImage);
                        System.IO.File.Delete(filePathLogo);
                    }

                    await newsRepository.DeleteNewsAsync(id, idToken);
                    TempData["Message"] = "SuppOk";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel() { RequestId = ex.Message };
                return View("Error", erreur);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                News news = await newsRepository.GetNewsAsync(id);
                AdministationNewsEditViewModel administationNewsEditViewModel = new AdministationNewsEditViewModel
                {
                    Titre = news.Titre,
                    TexteLibre = news.TexteLibre,
                    ExistingPhotoPath = news.PathImage
                };
                return View(administationNewsEditViewModel);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel() { RequestId = ex.Message };
                return View("Error", erreur);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AdministationNewsEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    News news = await newsRepository.GetNewsAsync(model.Id);
                    news.Titre = model.Titre;
                    news.TexteLibre = model.TexteLibre;
                    news.DatePublication = DateTime.Now;
                    news.UserId = User.Claims.FirstOrDefault(x => x.Type.ToString() == "sub").Value;

                    if (model.Photo != null)
                    {
                        if (model.ExistingPhotoPath != null)
                        {
                            string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                            System.IO.File.Delete(filePath);
                        }
                        news.PathImage = ProcessUploadedImage(model.Photo);
                    }

                    var idToken = await HttpContext.GetTokenAsync("access_token");
                    var result = newsRepository.UpdateNewsAsync(news, idToken);

                    //Message Toastr
                    TempData["Message"] = "ModifOk";
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    ErrorViewModel erreur = new ErrorViewModel() { RequestId = ex.Message };
                    return View("Error", erreur);
                }
            }
            return View(model);
        }
        private string ProcessUploadedImage(IFormFile iformfileImage)
        {
            try
            {
                string uniqueFileNameLogo = null;

                if (iformfileImage != null)
                {
                    //Creation du chemin root + images
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");

                    //Creation d'un d'un nouveau Guid + nom de l'image
                    uniqueFileNameLogo = Guid.NewGuid().ToString() + "_" + iformfileImage.FileName;

                    //Association du chemin root + Guid et image
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameLogo);


                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        iformfileImage.CopyTo(fileStream);
                    }

                }

                return uniqueFileNameLogo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}