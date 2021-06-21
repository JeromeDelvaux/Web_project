using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MesModels;
using MesModels.Models;
using Newtonsoft.Json;
using Web_project.Models.Interfaces;

namespace Web_project.Models.SqlRepository
{
    public class SQLNewsRepository: INewsRepository
    {
        private readonly HttpClient client;

        public SQLNewsRepository(HttpClient client)
        {
            this.client = client;
        }
        public async Task<News> CreateNewsAsync(News news, string idToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

            var content = JsonConvert.SerializeObject(news);
            var httpResponse = await client.PostAsync(MesUrls.NewsApi, new StringContent(content, Encoding.Default, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de créer la news");
            }

            var createdTask = JsonConvert.DeserializeObject<News>(await httpResponse.Content.ReadAsStringAsync());
            return createdTask;
        }

        public async Task DeleteNewsAsync(int id, string idToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

            var httpResponse = await client.DeleteAsync($"{MesUrls.NewsApi}{id}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de supprimer la news");
            }
        }

        public async Task<List<News>> GetAllNewsAsync()
        {
            var httpResponse = await client.GetAsync(MesUrls.NewsApi);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Aucune news retrouvée");
            }
            var content = await httpResponse.Content.ReadAsStringAsync();


            return JsonConvert.DeserializeObject<List<News>>(content);
        }

        public async Task<News> GetNewsAsync(int id)
        {
            var httpResponse = await client.GetAsync($"{MesUrls.NewsApi}{id}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("La news n'existe pas");
            }
            var content = await httpResponse.Content.ReadAsStringAsync();
            var news = JsonConvert.DeserializeObject<News>(content);

            return news;
        }

        public async Task<News> UpdateNewsAsync(News news, string idToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

            var content = JsonConvert.SerializeObject(news);

            var httpResponse = await client.PutAsync($"{MesUrls.NewsApi}{news.Id}", new StringContent(content, Encoding.Default, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de modifier la news");
            }

            var createdTask = JsonConvert.DeserializeObject<News>(await httpResponse.Content.ReadAsStringAsync());
            return createdTask;
        }
    }
}
