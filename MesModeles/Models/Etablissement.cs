using MesModels.ClassesEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace MesModels.Models
{
    public class Etablissement
    {
        public Etablissement()
        {
            this._HoraireList = new List<Horaires>();
            this._PhotosList = new List<PhotosEtablissement>();
        }
        public int Id { get; set; }

        public TypeEtablissement TypeEtab { get; set; }

        public string Nom { get; set; }

        public string NumeroTva { get; set; }

        public string AdresseEmailPro { get; set; }

        public string ZoneTexteLibre { get; set; }

        public string LogoPath { get; set; }

        public string CodePostal { get; set; }

        public string Ville { get; set; }

        public Pays Pays { get; set; }

        public string Rue { get; set; }

        public int NumeroBoite { get; set; }

        public string NumeroTelephone { get; set; }

        public string AdresseEmailEtablissement { get; set; }

        public string AdresseSiteWeb { get; set; }

        public string AdresseInstagram { get; set; }

        public string AdresseFacebook { get; set; }
        public string AdresseLinkedin { get; set; }
        public bool estValide { get; set; }
        public string PhotosPath { get; set; }
        public string AddLongitude { get; set; }
        public string AddLatitude { get; set; }
        public bool ValidShortUrl { get; set; }
        public string SiteWebShortUrl { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public List<Horaires> _HoraireList { get; set; }
        public List<PhotosEtablissement> _PhotosList { get; set; }

    }
}
