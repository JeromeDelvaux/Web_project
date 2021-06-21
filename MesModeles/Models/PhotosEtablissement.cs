using System;
using System.Collections.Generic;
using System.Text;

namespace MesModels.Models
{
    public class PhotosEtablissement
    {
        public int Id { get; set; }
        public string PhotosPath { get; set; }
        public int EtablissementId { get; set; }
        public Etablissement Etablissement { get; set; }
    }
}
