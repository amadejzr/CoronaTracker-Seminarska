using System;
using System.Collections.Generic;

namespace web.Models
{
    public class Odlok
    {
        public int Id {get;set;}
        public DateTime DatumZacetka{get;set;}

        public DateTime DatumKonca{get;set;}

        public ICollection<Uporabnik> Uporabniki { get; set; }
    }
}