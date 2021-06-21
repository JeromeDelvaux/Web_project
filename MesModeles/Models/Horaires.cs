using MesModels.ClassesEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace MesModels.Models
{
    public class Horaires
    {
        public Horaires()
        {

        }
        public Horaires(Jours jour)
        {
            this.Jour = jour;
        }
        public int Id { get; set; }
        public Jours Jour { get; set; }
        [JsonConverter(typeof(JsonTimeSpanConverter))]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan Heures_Ouverture { get; set; }
        [JsonConverter(typeof(JsonTimeSpanConverter))]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan Heures_Fermeture { get; set; }
        public int EtablissementId { get; set; }
        public Etablissement Etablissement { get; set; }
    }
}
