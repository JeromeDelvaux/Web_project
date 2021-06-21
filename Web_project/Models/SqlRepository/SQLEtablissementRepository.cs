using MesContexts.Models;
using MesModels;
using MesModels.Models;
using Microsoft.Extensions.Options;
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
    public class SQLEtablissementRepository: IEtablissementRepository
    {
        private readonly HttpClient client;

        public SQLEtablissementRepository(HttpClient client)
        {
            this.client = client;

        }
        public async Task<Etablissement> CreateEtablissementAsync(Etablissement etablissement, string idToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

            var content = JsonConvert.SerializeObject(etablissement);
            var httpResponse = await client.PostAsync(MesUrls.EtablissementApi, new StringContent(content, Encoding.Default, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de créer l'établissement");
            }

            var createdTask = JsonConvert.DeserializeObject<Etablissement>(await httpResponse.Content.ReadAsStringAsync());
            return createdTask;
        }

        public async Task Delete(int id, string idToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

            var httpResponse = await client.DeleteAsync($"{MesUrls.EtablissementApi}{id}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de supprimer l'établissement");
            }
        }

        public async Task<List<Etablissement>> GetAllEtablissement()
        {
            var httpResponse = await client.GetAsync(MesUrls.EtablissementApi);
            if(!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Aucun etablissement retrouvé");
            }
            var content = await httpResponse.Content.ReadAsStringAsync();
            var task = JsonConvert.DeserializeObject<List<Etablissement>>(content);
            return task;
        }

        public async Task<Etablissement> GetEtablissement(int Id)
        {
            var httpResponse = await client.GetAsync($"{MesUrls.EtablissementApi}{Id}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("L'établissement n'existe pas");
            }
            var content = await httpResponse.Content.ReadAsStringAsync();
            var etablissement = JsonConvert.DeserializeObject<Etablissement>(content);

            return etablissement;
        }

        public async Task<Etablissement> Update(Etablissement etablissement, string idToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

            var content = JsonConvert.SerializeObject(etablissement);

            var httpResponse = await client.PutAsync($"{MesUrls.EtablissementApi}{etablissement.Id}", new StringContent(content, Encoding.Default, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de modifier l'établissement");
            }

            var createdTask = JsonConvert.DeserializeObject<Etablissement>(await httpResponse.Content.ReadAsStringAsync());
            return createdTask;
        }
    }
}
