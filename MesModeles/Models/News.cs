using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MesModels.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string TexteLibre { get; set; }
        public string PathImage { get; set; }
        [DataType(DataType.Date)]
        public DateTime DatePublication { get; set; }
        public string UserId { get; set; }
        public string NomAuteur { get; set; }
        public User User { get; set; }
    }
}
