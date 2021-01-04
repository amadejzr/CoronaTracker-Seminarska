using System;
using System.Collections.Generic;

namespace web.Models
{
    public class Prebivalisce
    {
        public int Id { get; set; }

        public string Mesto{get;set;}

        public string Naslov {get;set;}


        public Uporabnik Uporabnik { get; set; }
    }
}