using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class Odlok
    {
        public int Id {get;set;}

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DatumZacetka{get;set;}

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DatumKonca{get;set;}

        public ICollection<Uporabnik> Uporabnik { get; set; }
    }
}