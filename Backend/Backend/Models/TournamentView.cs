using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class TournamentView:Tournament
    {
        public HttpPostedFileBase LogoFile { get; set; }
    }
}