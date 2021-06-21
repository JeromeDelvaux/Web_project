using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_project.ViewsModels;

namespace Web_project.Models.Interfaces
{
    public interface IMapQuestRepository
    {
        Task<EtablissementCreateViewModel> CoordoneesAsync(EtablissementCreateViewModel etablissement);
    }
}
