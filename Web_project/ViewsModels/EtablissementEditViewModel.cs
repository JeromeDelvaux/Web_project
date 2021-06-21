using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_project.ViewsModels
{
    public class EtablissementEditViewModel:EtablissementCreateViewModel
    {
        public int Id { get; set; }
        public List<string> ExistingPhotoPath { get; set; }
        public string ExistingLogoPath { get; set; }
    }
}
