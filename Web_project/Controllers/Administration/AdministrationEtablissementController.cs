using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MesModels.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Web_project.Models.Interfaces;
using Web_project.ViewsModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Services.Notification;
using MesModels;
using Web_project.Models.SqlRepository;
using Microsoft.AspNetCore.Http;

namespace Web_project.Controllers
{
    [Authorize(Roles = "Administrateur,Gestionnaire")]
    public class AdministrationEtablissementController : Controller
    {
        private readonly IEtablissementRepository _etablissementRepository;
        private readonly IHoraireRepository _horaireRepository;
        private readonly IPhotosRepository _photosRepository;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IMapQuestRepository _mapQuestRepository;
        private readonly IShortUrlRepository _shortUrlRepository;

        public AdministrationEtablissementController(
            IEtablissementRepository etablissementRepository,
            IHoraireRepository horaireRepository,
            IPhotosRepository photosRepository,
            IWebHostEnvironment hostingEnvironment,
            IMapQuestRepository mapQuestRepository,
            IShortUrlRepository shortUrlRepository)
        {
            this._etablissementRepository = etablissementRepository;
            this._horaireRepository = horaireRepository;
            this._photosRepository = photosRepository;
            this.hostingEnvironment = hostingEnvironment;
            this._mapQuestRepository = mapQuestRepository;
            this._shortUrlRepository = shortUrlRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                if (User.Claims.Any(x => x.Type == "role" && (x.Value == "Gestionnaire")))
                {
                    var userId = User.Claims.FirstOrDefault(x => x.Type.ToString() == "sub").Value;

                    List<Etablissement> _LetablissementsAll = await _etablissementRepository.GetAllEtablissement();
                    List<Etablissement> _LHorairesEtablissement = _LetablissementsAll.Where(x => x.UserId == userId).ToList();
                    return View(_LHorairesEtablissement);
                }
                else
                {
                    List<Etablissement> _LetablissementsAll = await _etablissementRepository.GetAllEtablissement();
                    return View(_LetablissementsAll);
                }
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel() { RequestId = ex.Message };
                return View("Error", erreur);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString() == "sub").Value;

                EtablissementDetailsViewModel etablissementDetailsViewModels = new EtablissementDetailsViewModel()
                {
                    etablissement = await _etablissementRepository.GetEtablissement(id),
                    PageTitle = "Etablissement Details"
                };

                if(etablissementDetailsViewModels.etablissement.UserId== userId || User.Claims.Any(x => x.Type == "role" && (x.Value == "Administrateur")))
                {
                    return View(etablissementDetailsViewModels);
                }
                else
                {
                    TempData["Message"] = "interdit";
                    return RedirectToAction("index");
                }
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
                EtablissementCreateViewModel model = new EtablissementCreateViewModel();
                return View(model);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel() { RequestId = ex.Message };
                return View("Error", erreur);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(EtablissementCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Verification du nombre de photos +Message Toastr
                    if (model.Photos != null)
                    {
                        if (model.Photos.Count > 5)
                        {
                            TempData["Message"] = "NbPhotosNotOk";
                            return View(model);
                        }
                    }

                    var idToken = await HttpContext.GetTokenAsync("access_token");
                    var userId = User.Claims.FirstOrDefault(x => x.Type.ToString() == "sub").Value;

                    //Creation du path Logo
                    string uniqueFileNameLogo = ProcessUploadedLogo(model.Logo);

                    //Creation de mes coordonnées adresse
                    var _mapQuestCoordonees = await _mapQuestRepository.CoordoneesAsync(model);

                    //Creation ShortUrl
                    if(model.etablissement.ValidShortUrl==true)
                    {
                        string shorturl = await _shortUrlRepository.GetShortURL(model.etablissement.AdresseSiteWeb);
                        model.etablissement.SiteWebShortUrl ="http://"+shorturl;
                    }
                    
                    Etablissement newEtablissement = new Etablissement
                    {
                        UserId = userId,
                        Nom = model.etablissement.Nom,
                        TypeEtab = model.etablissement.TypeEtab,
                        NumeroTva = model.etablissement.NumeroTva,
                        AdresseEmailPro = model.etablissement.AdresseEmailPro,
                        Rue = model.etablissement.Rue,
                        NumeroBoite = model.etablissement.NumeroBoite,
                        Ville = model.etablissement.Ville,
                        CodePostal = model.etablissement.CodePostal,
                        Pays = model.etablissement.Pays,
                        NumeroTelephone = model.etablissement.NumeroTelephone,
                        AdresseEmailEtablissement = model.etablissement.AdresseEmailEtablissement,
                        AdresseSiteWeb = model.etablissement.AdresseSiteWeb,
                        AdresseInstagram = model.etablissement.AdresseInstagram,
                        AdresseFacebook = model.etablissement.AdresseFacebook,
                        AdresseLinkedin = model.etablissement.AdresseLinkedin,
                        estValide = model.etablissement.estValide,
                        ZoneTexteLibre = model.etablissement.ZoneTexteLibre,
                        LogoPath = uniqueFileNameLogo,
                        AddLatitude = _mapQuestCoordonees.etablissement.AddLatitude,
                        AddLongitude = _mapQuestCoordonees.etablissement.AddLongitude,
                        SiteWebShortUrl= model.etablissement.SiteWebShortUrl,
                        ValidShortUrl= model.etablissement.ValidShortUrl

                    };

                    var result = await _etablissementRepository.CreateEtablissementAsync(newEtablissement, idToken);

                    //Création Photos
                    if (model.Photos != null)
                    {
                        List<string> _LpathPhotos = ProcessUploadedPhotos(model.Photos);
                        CreationPhoto(_LpathPhotos, result.Id);
                    }

                    //Creation Horaires
                    CreationHoraire(model.MonHoraire, result.Id);

                    //Message Toastr
                    TempData["Message"] = "CreateOk";

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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString() == "sub").Value;
                Etablissement etablissement = await _etablissementRepository.GetEtablissement(id);

                if (etablissement.UserId == userId || User.Claims.Any(x => x.Type == "role" && (x.Value == "Administrateur")))
                {

                    EtablissementEditViewModel etablissementEditViewModel = new EtablissementEditViewModel
                    {
                        etablissement = etablissement,
                        ExistingLogoPath = etablissement.LogoPath
                    };

                    //Recuperation des horaires 
                    List<Horaires> _LhorairesAll = await _horaireRepository.GetAllHorairesAsync();
                    List<Horaires> _LhorairesEtablissement = _LhorairesAll.Where(x => x.EtablissementId == id).OrderBy(s => s.Jour).ToList();

                    //Si il y a des horaires, popularisation de ma liste "MonHoraire"
                    if (_LhorairesEtablissement.Any())
                    {
                        etablissementEditViewModel.MonHoraire = _LhorairesEtablissement;
                    }

                    //Recuperation des photos
                    List<PhotosEtablissement> _LphotosAll = await _photosRepository.GetAllPhotosAsync();
                    List<PhotosEtablissement> _LphotosEtablissement = _LphotosAll.Where(x => x.EtablissementId == id).ToList();


                    List<string> _LphotosPaths = new List<string>();

                    //Si il y a des photos, creation du chemin complet + popularisation de ma liste "_LphotosPaths"
                    if (_LphotosEtablissement.Any())
                    {
                        foreach (PhotosEtablissement photos in _LphotosEtablissement)
                        {
                            _LphotosPaths.Add(Path.Combine("\\", "images", photos.PhotosPath));
                        }

                        etablissementEditViewModel.ExistingPhotoPath = _LphotosPaths;
                    }
                    return View(etablissementEditViewModel);
                }
                else
                {
                    TempData["Message"] = "interdit";
                    return RedirectToAction("index");
                }
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel() { RequestId = ex.Message };
                return View("Error", erreur);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EtablissementEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Verification du nombre de photos +Message Toastr
                    if (model.Photos != null)
                    {
                        if (model.Photos.Count > 5)
                        {
                            TempData["Message"] = "NbPhotosNotOk";
                            return View(model);
                        }
                    }

                    var idToken = await HttpContext.GetTokenAsync("access_token");

                    var _mapQuestCoordonees = await _mapQuestRepository.CoordoneesAsync(model);
                    Etablissement etablissement = await _etablissementRepository.GetEtablissement(model.etablissement.Id);

                    string temp = etablissement.AdresseSiteWeb;
                    bool tempValid = etablissement.ValidShortUrl;

                    etablissement = model.etablissement;
                    etablissement.SiteWebShortUrl = model.etablissement.SiteWebShortUrl;
                    etablissement.LogoPath = model.ExistingLogoPath;
                    etablissement.AddLatitude = _mapQuestCoordonees.etablissement.AddLatitude;
                    etablissement.AddLongitude = _mapQuestCoordonees.etablissement.AddLongitude;

                    //Creation ShortUrl si changement d'adresse site web
                    if ((model.etablissement.ValidShortUrl == true && model.etablissement.AdresseSiteWeb != temp) || (model.etablissement.ValidShortUrl == true && tempValid == false))
                    {
                        string shorturl = await _shortUrlRepository.GetShortURL(etablissement.AdresseSiteWeb);
                        etablissement.SiteWebShortUrl = "http://" + shorturl;
                    }

                    //Suppression de la shortUrl
                    if(model.etablissement.ValidShortUrl==false)
                    {
                        etablissement.SiteWebShortUrl = null;
                        etablissement.ValidShortUrl = false;
                    }

                    //Suppression des photos existantes si modification de photos
                    if (model.ExistingPhotoPath == null)
                    {
                        List<PhotosEtablissement> lPhotosPasses = await _photosRepository.GetAllPhotosAsync();
                        await DeletePhotos(lPhotosPasses.Where(x => x.EtablissementId == etablissement.Id).ToList());
                    }

                    //Création Photos
                    if (model.Photos != null)
                    {
                        List<string> _LpathPhotos = ProcessUploadedPhotos(model.Photos);
                        CreationPhoto(_LpathPhotos, model.etablissement.Id);
                    }

                    //Création Logo
                    if (model.Logo != null)
                    {
                        if (model.ExistingLogoPath != null)
                        {
                            string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingLogoPath);
                            System.IO.File.Delete(filePath);
                        }
                        etablissement.LogoPath = ProcessUploadedLogo(model.Logo);
                    }

                    //Supression des horaires existants
                    List<Horaires> _LhorairesAll = await _horaireRepository.GetAllHorairesAsync();
                    await DeleteHoraire(_LhorairesAll.Where(x => x.EtablissementId == etablissement.Id).ToList());

                    //Creation des horaires
                    CreationHoraire(model.MonHoraire, etablissement.Id);

                    var result = await _etablissementRepository.Update(etablissement, idToken);

                    //Message Toastr
                    TempData["Message"] = "ModifOk";
                    return RedirectToAction("index");


                }
                return View(model);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel() { RequestId = ex.Message };
                return View("Error", erreur);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteEtablissement(int id)
        {
            try
            {
                Etablissement etablissements = await _etablissementRepository.GetEtablissement(id);
                if (etablissements == null)
                {
                    ViewBag.ErrorMessage = $"L'établisssement avec l'id {id} n'existe pas";
                    return View("NotFound");
                }
                else
                {
                    var idToken = await HttpContext.GetTokenAsync("access_token");

                    List<PhotosEtablissement> _LphotosAll = await _photosRepository.GetAllPhotosAsync();
                    List<PhotosEtablissement> _LphotosDelete = new List<PhotosEtablissement>(_LphotosAll.Where(x => x.EtablissementId == id).ToList());

                    //Suppression des photos sur le serveur
                    if (_LphotosDelete != null)
                    {
                        for (int i = 0; i < _LphotosDelete.Count; i++)
                        {
                            string filePathPhotos = Path.Combine(hostingEnvironment.WebRootPath, "images", _LphotosDelete[i].PhotosPath);
                            System.IO.File.Delete(filePathPhotos);
                        }
                    }

                    //Suppression du logo sur le serveur
                    if (etablissements.LogoPath != null)
                    {
                        string filePathLogo = Path.Combine(hostingEnvironment.WebRootPath, "images", etablissements.LogoPath);
                        System.IO.File.Delete(filePathLogo);
                    }

                    //Suppression en cascade de l"etablissement
                    await _etablissementRepository.Delete(id, idToken);

                    //Message Toastr
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
        [HttpPost]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> ValidationEtablissement(int id)
        {
            try
            {
                Etablissement etablissement = await _etablissementRepository.GetEtablissement(id);

                if (etablissement.estValide == false)
                {
                    var idToken = await HttpContext.GetTokenAsync("access_token");
                    etablissement.estValide = true;
                    var result = await _etablissementRepository.Update(etablissement, idToken);
                }
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel() { RequestId = ex.Message };
                return View("Error", erreur);
            }

            return RedirectToAction("index");

        }

        private async void CreationHoraire(List<Horaires> _Lhoraires,int id)
        {
            try
            {
                var idToken = await HttpContext.GetTokenAsync("access_token");

                foreach (Horaires item in _Lhoraires)
                {
                    Horaires nvHoraire = new Horaires();
                    nvHoraire.Heures_Fermeture = item.Heures_Fermeture;
                    nvHoraire.Heures_Ouverture = item.Heures_Ouverture;
                    nvHoraire.Jour = item.Jour;
                    nvHoraire.EtablissementId = id;
                    var result = await _horaireRepository.CreateHoraireAsync(nvHoraire, idToken);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<string> ProcessUploadedPhotos(List<IFormFile> _Liformfilephotos)
        {
            try
            {
                List<string> uniqueFileNamePhoto = new List<string>();

                if (_Liformfilephotos != null && _Liformfilephotos.Count > 0)
                {
                    foreach (IFormFile photo in _Liformfilephotos)
                    {
                        //Creation du chemin root + images
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");

                        //Creation d'un d'un nouveau Guid + nom de la photo
                        string uploadPath = Guid.NewGuid().ToString() + "_" + photo.FileName;

                        //Association du chemin root + Guid et image
                        string filePath = Path.Combine(uploadsFolder, uploadPath);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            photo.CopyTo(fileStream);
                        }

                        uniqueFileNamePhoto.Add(uploadPath);
                    }
                }

                return uniqueFileNamePhoto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ProcessUploadedLogo(IFormFile iformfileLogo)
        {
            try
            {
                string uniqueFileNameLogo = null;

                if (iformfileLogo != null)
                {
                    //Creation du chemin root + images
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");

                    //Creation d'un d'un nouveau Guid + nom de la photo
                    uniqueFileNameLogo = Guid.NewGuid().ToString() + "_" + iformfileLogo.FileName;

                    //Association du chemin root + Guid et image
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameLogo);


                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        iformfileLogo.CopyTo(fileStream);
                    }

                }
                return uniqueFileNameLogo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void CreationPhoto(List<string> _LphotosEtablissement, int id)
        {
            try
            {
                var idToken = await HttpContext.GetTokenAsync("access_token");

                foreach (string path in _LphotosEtablissement)
                {
                    PhotosEtablissement photo = new PhotosEtablissement();
                    photo.PhotosPath = path;
                    photo.EtablissementId = id;
                    var result = await _photosRepository.CreatePhotoAsync(photo, idToken);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task DeleteHoraire(List<Horaires> _Lhoraires)
        {
            try
            {
                var idToken = await HttpContext.GetTokenAsync("access_token");

                if (_Lhoraires != null)
                {
                    for (int i = 0; i < _Lhoraires.Count; i++)
                    {
                        await _horaireRepository.DeleteHoraireAsync(_Lhoraires[i].Id, idToken);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task DeletePhotos(List<PhotosEtablissement> _LphotosEtablissement)
        {
            try
            {
                var idToken = await HttpContext.GetTokenAsync("access_token");

                if (_LphotosEtablissement != null)
                {
                    for (int i = 0; i < _LphotosEtablissement.Count; i++)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", _LphotosEtablissement[i].PhotosPath);

                        //Suppression des photos sur le serveur
                        System.IO.File.Delete(filePath);

                        //Suppression des photos dasn la base de données
                        await _photosRepository.DeletePhotoAsync(_LphotosEtablissement[i].Id, idToken);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}