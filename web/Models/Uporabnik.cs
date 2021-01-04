using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace web.Models
{
    public class Uporabnik : IdentityUser
    {

        public string Ime { get; set; }
        public string Priimek { get; set; }

        public string Telefon { get; set; }

        public Odlok Odloki { get; set; }

        public Prebivalisce Prebivalisca { get; set; }


    }
}