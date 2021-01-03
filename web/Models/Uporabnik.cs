using System;
using System.Collections.Generic;

namespace web.Models
{
    public class Uporabnik
    {
        public int Id { get; set; }

        public string Ime { get; set; }
        public string Priimek { get; set; }
        public string Email { get; set; }

        public string Telefon { get; set; }

        public ICollection<Odlok> Oldoki { get; set; }

        public Prebivalisce Prebivalisce {get;set;} 


    }
}