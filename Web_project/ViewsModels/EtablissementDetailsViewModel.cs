using MesModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_project.Models;

namespace Web_project.ViewsModels
{
    public class EtablissementDetailsViewModel
    {
        public Etablissement etablissement { get; set; }
        public string PageTitle { get; set; }
    }
}
