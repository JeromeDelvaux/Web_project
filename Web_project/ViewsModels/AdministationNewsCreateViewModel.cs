using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_project.ViewsModels
{
    public class AdministationNewsCreateViewModel
    {
        public string Titre { get; set; }
        public string TexteLibre { get; set; }
        public IFormFile Photo { get; set; }

    }
}
