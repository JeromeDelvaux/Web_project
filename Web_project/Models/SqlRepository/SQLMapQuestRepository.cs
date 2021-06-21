
using MesModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Web_project.Models.Interfaces;
using Web_project.ViewsModels;

namespace Web_project.Models.SqlRepository
{
    public class SQLMapQuestRepository: IMapQuestRepository
    {
        private readonly HttpClient client;

        public SQLMapQuestRepository(HttpClient client)
        {
            this.client = client;
        }
        public async Task<EtablissementCreateViewModel> CoordoneesAsync(EtablissementCreateViewModel model)
        {
            string requeteUrl = $"{MesUrls.MapQuestApi}{model.etablissement.NumeroBoite}+{model.etablissement.Rue}+{model.etablissement.CodePostal}+{model.etablissement.Ville}.{model.etablissement.Pays}";
            
            var coordonnées = await client.GetAsync(requeteUrl);
            if (!coordonnées.IsSuccessStatusCode)
            {
                throw new Exception("Impossible de récupérer les coordonnées");
            }

            var reponse = await coordonnées.Content.ReadAsStringAsync();
            var _obj = JsonConvert.DeserializeObject<dynamic>(reponse);

            model.etablissement.AddLatitude=_obj.results[0].locations[0].displayLatLng.lat;
            model.etablissement.AddLongitude=_obj.results[0].locations[0].displayLatLng.lng;

            return model;
        }
    }
}
