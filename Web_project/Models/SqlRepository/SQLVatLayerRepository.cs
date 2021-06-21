using MesModels;
using MesModels.VatLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Web_project.Models.Interfaces;

namespace Web_project.Models.SqlRepository
{
    public class SQLVatLayerRepository: IVatLayerRepository
    {
        private readonly HttpClient client;

        public SQLVatLayerRepository(HttpClient client)
        {
            this.client = client;
        }
        public async Task<VatRoot> VATReponse(string vat)
        {
            string requeteUrl =$"{MesUrls.VatApi}&vat_number={vat}";
            var httpResponse = await client.GetAsync(requeteUrl);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de retrouver le N° de TVA");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var finalVat = JsonConvert.DeserializeObject<VatRoot>(content);

            return finalVat;
        }
    }
}
