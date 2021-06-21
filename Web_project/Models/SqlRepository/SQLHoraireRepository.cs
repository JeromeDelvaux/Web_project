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
    public class SQLHoraireRepository:IHoraireRepository
    {
        private readonly HttpClient client;

        public SQLHoraireRepository(HttpClient client)
        {
            this.client = client;
        }

        public async Task<Horaires> CreateHoraireAsync(Horaires horaire, string idToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

            var content = JsonConvert.SerializeObject(horaire);
            var httpResponse = await client.PostAsync(MesUrls.HoraireApi, new StringContent(content, Encoding.Default, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de créer l'horaire");
            }

            var createdTask = JsonConvert.DeserializeObject<Horaires>(await httpResponse.Content.ReadAsStringAsync());
            return createdTask;
        }

        public async Task DeleteHoraireAsync(int id, string idToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

            var httpResponse = await client.DeleteAsync($"{MesUrls.HoraireApi}{id}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de supprimer l'horaire");
            }
        }

        public async Task<List<Horaires>> GetAllHorairesAsync()
        {
            var httpResponse = await client.GetAsync(MesUrls.HoraireApi);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de récupérer les horaires");
            }
            var content = await httpResponse.Content.ReadAsStringAsync();


            return JsonConvert.DeserializeObject<List<Horaires>>(content);
        }

        public async Task<Horaires> GetHoraireAsync(int id)
        {
            var httpResponse = await client.GetAsync($"{MesUrls.HoraireApi}{id}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de récupérer l'horaire");
            }
            var content = await httpResponse.Content.ReadAsStringAsync();
            var horaire = JsonConvert.DeserializeObject<Horaires>(content);

            return horaire;
        }

        public async Task<Horaires> UpdateHoraireAsync(Horaires horaire, string idToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

            var content = JsonConvert.SerializeObject(horaire);

            var httpResponse = await client.PutAsync($"{MesUrls.HoraireApi}{horaire.Id}", new StringContent(content, Encoding.Default, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de modifier l'horaire");
            }

            var createdTask = JsonConvert.DeserializeObject<Horaires>(await httpResponse.Content.ReadAsStringAsync());
            return createdTask;
        }
    }
}
