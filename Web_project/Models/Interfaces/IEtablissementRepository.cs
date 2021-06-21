using MesModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_project.Models.Interfaces
{
    public interface IEtablissementRepository
    {
        Task<Etablissement> GetEtablissement(int Id);
        Task<List<Etablissement>> GetAllEtablissement();
        Task<Etablissement> CreateEtablissementAsync(Etablissement etablissement, string idToken);
        Task<Etablissement> Update(Etablissement etablissement,string idToken);
        Task Delete(int id, string idToken);
    }
}
