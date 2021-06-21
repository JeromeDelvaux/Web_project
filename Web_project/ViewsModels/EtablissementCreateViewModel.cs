using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MesModels.ClassesEnum;
using MesModels.Models;

namespace Web_project.ViewsModels
{
    public class EtablissementCreateViewModel
    {
        public EtablissementCreateViewModel()
        {
            this.MonHoraire = new List<Horaires>();

            Horaires lundi = new Horaires(Jours.lundi);
            Horaires mardi = new Horaires(Jours.mardi);
            Horaires mercredi = new Horaires(Jours.mercredi);
            Horaires jeudi = new Horaires(Jours.jeudi);
            Horaires vendredi = new Horaires(Jours.vendredi);
            Horaires samedi = new Horaires(Jours.samedi);
            Horaires dimanche = new Horaires(Jours.dimanche);

            MonHoraire.Add(lundi);
            MonHoraire.Add(mardi);
            MonHoraire.Add(mercredi);
            MonHoraire.Add(jeudi);
            MonHoraire.Add(vendredi);
            MonHoraire.Add(samedi);
            MonHoraire.Add(dimanche);
        }
        public Etablissement etablissement { get; set; }
        public IFormFile Logo { get; set; }
        public List<IFormFile> Photos { get; set; }
        public List<Horaires> MonHoraire { get; set; }
    }
}
