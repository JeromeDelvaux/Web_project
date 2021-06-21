using MesModels;
using MesModels.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Web_project.Models.Interfaces;

namespace Web_project.Models.SqlRepository
{
    public class SQLPhotosRepository:IPhotosRepository
    {
        private readonly HttpClient client;

        public SQLPhotosRepository(HttpClient client)
        {
            this.client = client;
        }
        public async Task<PhotosEtablissement> CreatePhotoAsync(PhotosEtablissement photo, string idToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

            var content = JsonConvert.SerializeObject(photo);
            var httpResponse = await client.PostAsync(MesUrls.PhotoApi, new StringContent(content, Encoding.Default, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de créer la photo");
            }

            var createdTask = JsonConvert.DeserializeObject<PhotosEtablissement>(await httpResponse.Content.ReadAsStringAsync());
            return createdTask;
        }

        public async Task DeletePhotoAsync(int id, string idToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

            var httpResponse = await client.DeleteAsync($"{MesUrls.PhotoApi}{id}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de supprimer la photo");
            }
        }

        public async Task<List<PhotosEtablissement>> GetAllPhotosAsync()
        {
            var httpResponse = await client.GetAsync(MesUrls.PhotoApi);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de récupérer les établissements");
            }
            var content = await httpResponse.Content.ReadAsStringAsync();


            return JsonConvert.DeserializeObject<List<PhotosEtablissement>>(content);
        }

        public async Task<PhotosEtablissement> GetPhotoAsync(int id)
        {
            var httpResponse = await client.GetAsync($"{MesUrls.PhotoApi}{id}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de récupérer la photo");
            }
            var content = await httpResponse.Content.ReadAsStringAsync();
            var photo = JsonConvert.DeserializeObject<PhotosEtablissement>(content);

            return photo;
        }

        public Task<PhotosEtablissement> UpdatePhotoAsync(PhotosEtablissement photo, string idToken)
        {
            throw new NotImplementedException();
        }
    }
}
