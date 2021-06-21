using MesModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_project.Models.Interfaces
{
    public interface INewsRepository
    {
        Task<List<News>> GetAllNewsAsync();

        Task<News> GetNewsAsync(int id);

        Task<News> CreateNewsAsync(News news, string idToken);

        Task<News> UpdateNewsAsync(News news, string idToken);

        Task DeleteNewsAsync(int id, string idToken);
    }
}
