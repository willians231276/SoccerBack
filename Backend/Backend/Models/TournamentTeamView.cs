using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    [NotMapped]
    public class TournamentTeamView: TournamentTeam
    {
        public int LeagueId { get; set; }
    }
}