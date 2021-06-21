using MesModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_project.ViewsModels
{
    public class EtablissementMarkerViewModel
    {
        public Etablissement etablissement { get; set; }
        public int MinAvantFermeture { get; set; }
        public bool estAuthentifie { get; set; }
    }
}
