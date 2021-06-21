using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web_project.Models.Interfaces;
using Web_project.ViewsModels;

namespace Web_project.Models.SqlRepository
{
    public class SQLShortUrlRepository: IShortUrlRepository
    {
        private readonly HttpClient client;

        public SQLShortUrlRepository(HttpClient client)
        {
            this.client = client;
        }
        public async Task<string> GetShortURL(string longUrl)
        {
            var payload = new
            {
                destination = longUrl,
                domain = new
                {
                    fullName = "rebrand.ly"
                }
                //, slashtag = "A_NEW_SLASHTAG"
                //, title = "Rebrandly YouTube channel"
            };

            using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.rebrandly.com") })
            {
                httpClient.DefaultRequestHeaders.Add("apikey", "6777cbb873dd43b4bfb588a5f8d39b9c");
                //httpClient.DefaultRequestHeaders.Add("workspace", "YOUR_WORKSPACE_ID");

                var body = new StringContent(
                    JsonConvert.SerializeObject(payload), UnicodeEncoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("/v1/links", body))
                {
                    response.EnsureSuccessStatusCode();

                    var link = JsonConvert.DeserializeObject<dynamic>(
                        await response.Content.ReadAsStringAsync());
                    return (link.shortUrl);
                }
            }  
        }
    }
}
