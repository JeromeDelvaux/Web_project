using MesModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_project.Models.Interfaces
{
    public interface IHoraireRepository
    {
        Task<List<Horaires>> GetAllHorairesAsync();

        Task<Horaires> GetHoraireAsync(int id);

        Task<Horaires> CreateHoraireAsync(Horaires horaire, string idToken);

        Task<Horaires> UpdateHoraireAsync(Horaires horaire, string idToken);

        Task DeleteHoraireAsync(int id, string idToken);
    }
}
