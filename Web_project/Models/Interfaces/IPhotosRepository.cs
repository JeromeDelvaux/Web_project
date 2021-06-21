using MesModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_project.Models.Interfaces
{
    public interface IPhotosRepository
    {
        Task<List<PhotosEtablissement>> GetAllPhotosAsync();

        Task<PhotosEtablissement> GetPhotoAsync(int id);

        Task<PhotosEtablissement> CreatePhotoAsync(PhotosEtablissement photo, string idToken);

        Task<PhotosEtablissement> UpdatePhotoAsync(PhotosEtablissement photo, string idToken);

        Task DeletePhotoAsync(int id, string idToken);
    }
}
