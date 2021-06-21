using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MesModels.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web_project.Models.Interfaces;
using Web_project.ViewsModels;

namespace Web_project.Controllers.Utilisateurs
{
    public class EtablissementController : Controller
    {
        private readonly IEtablissementRepository _etablissementRepository;
        private readonly IHoraireRepository _horaireRepository;
        private readonly IPhotosRepository _photosRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public EtablissementController(
            IEtablissementRepository etablissementRepository,
            IHoraireRepository horaireRepository,
            IPhotosRepository photosRepository,
            IWebHostEnvironment hostingEnvironment)
        {
            this._etablissementRepository = etablissementRepository;
            this._horaireRepository = horaireRepository;
            this._photosRepository = photosRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> ListeEtablissements()
        {
            try
            {
                var model = await _etablissementRepository.GetAllEtablissement();
                var etab = model.Where(x => x.estValide == true);
                return View(etab);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel() { RequestId = ex.Message };
                return View("Error", erreur);
            }
        }
        [HttpGet]
        public async Task<List<string>> GetAllEtab ()
        {
            try
            {
                List<string> etablissementJson = new List<string>();

                List<Etablissement> _LetabblissementsAll = await _etablissementRepository.GetAllEtablissement();
                List<Etablissement> _EtabValidList = new List<Etablissement>(_LetabblissementsAll.Where(x => x.estValide == true));

                foreach (Etablissement etablissement in _EtabValidList)
                {
                    EtablissementMarkerViewModel marker = new EtablissementMarkerViewModel
                    {
                        etablissement = etablissement

                    };

                    //recuperation du nombre de minutes avant la fermeture
                    marker.MinAvantFermeture = await FermetureHoraire(etablissement.Id);

                    //verification si authentifié
                    marker.estAuthentifie = User.Identity.IsAuthenticated;

                    List<Horaires> _LhorairesAll = await _horaireRepository.GetAllHorairesAsync();
                    List<Horaires> _Lhoraires = new List<Horaires>(_LhorairesAll.Where(x => x.EtablissementId == etablissement.Id).ToList());

                    foreach (Horaires item in _Lhoraires)
                    {
                        marker.etablissement._HoraireList.Add(item);
                    }

                    string markerSerial = JsonConvert.SerializeObject(marker);
                    etablissementJson.Add(markerSerial);
                }

                return etablissementJson;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                EtablissementDetailsViewModel etablissementDetailsViewModels = new EtablissementDetailsViewModel()
                {
                    etablissement = await _etablissementRepository.GetEtablissement(id ?? 1),
                    PageTitle = "Etablissement Details"
                };
                List<Horaires> _LhorairesAll = await _horaireRepository.GetAllHorairesAsync();
                List<Horaires> _Lhoraires = new List<Horaires>(_LhorairesAll.Where(x => x.EtablissementId == id).ToList());

                foreach (Horaires item in _Lhoraires)
                {
                    etablissementDetailsViewModels.etablissement._HoraireList.Add(item);
                }

                List<PhotosEtablissement> lPhotosPasses = await _photosRepository.GetAllPhotosAsync();
                List<PhotosEtablissement> lPhotos = new List<PhotosEtablissement>(lPhotosPasses.Where(x => x.EtablissementId == id).ToList());

                foreach (PhotosEtablissement item in lPhotos)
                {
                    PhotosEtablissement photosEtablissement = new PhotosEtablissement
                    {
                        Id = item.Id,
                        PhotosPath = Path.Combine("\\", "images", item.PhotosPath),
                        EtablissementId = item.EtablissementId,
                        Etablissement = item.Etablissement

                    };

                    etablissementDetailsViewModels.etablissement._PhotosList.Add(photosEtablissement);
                }

                return View(etablissementDetailsViewModels);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel() { RequestId = ex.Message };
                return View("Error", erreur);
            }
        }
        public async Task<int> FermetureHoraire(int etablissementId)
        {
            try
            {
                int nbMinAvantFermeture = 0;

                List<Horaires> _LhoraireAll = await _horaireRepository.GetAllHorairesAsync();

                if (_LhoraireAll.Any(x => x.EtablissementId == etablissementId))
                {
                    List<Horaires> _LhorairesEtablissement = new List<Horaires>();
                    _LhorairesEtablissement = _LhoraireAll.Where(x => x.EtablissementId == etablissementId).ToList();

                    //récuperation de l'heure courante
                    TimeSpan thisHeure = DateTime.Now.TimeOfDay;

                    //récupération de la culture dans laquelle le programme est fait (fr-BE)
                    CultureInfo culture = CultureInfo.CurrentCulture;

                    //récuperation du jour courant gràce à culture
                    string thisJour = culture.DateTimeFormat.GetDayName(DateTime.Now.Date.DayOfWeek).ToString().ToLower();
                    
                    //comparaison des des critères => jour, heure_ouv, heure_ferm 
                    Horaires thisHoraireJour = _LhorairesEtablissement.FirstOrDefault(x => x.Jour.ToString().Equals(thisJour) && x.Heures_Ouverture <= thisHeure && x.Heures_Fermeture >= thisHeure);

                    if (thisHoraireJour != null)
                    { 
                        //division par 1 afin d'avoir un nb. entier
                        nbMinAvantFermeture = (int)(thisHoraireJour.Heures_Fermeture.TotalMinutes - thisHeure.TotalMinutes) / 1;
                    }
                }
                return nbMinAvantFermeture;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}