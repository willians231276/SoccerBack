using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Team
    {
        public int TeamId { get; set; }

        [Required(ErrorMessage = "El campo {0} requerido")]
        [MaxLength(50, ErrorMessage = "La longitud maxima del campo {0} es {1} caracteres.")]
        [Index("Team_Name_LeagueId_Index", IsUnique = true,Order =1)]
        [Display(Name = "Team")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [Required(ErrorMessage = "El campo {0} requerido")]
        [Index("Team_Initials_LeagueId_Index", IsUnique =true,Order =1)]
        [StringLength(3,ErrorMessage = "La longitud maxima del campo {0} es {1} caracteres.",MinimumLength =2)]
        public string Initials { get; set; }

        [Required(ErrorMessage = "El campo {0} requerido")]
        [Index("Team_Name_LeagueId_Index", IsUnique = true,Order =2)]
        [Index("Team_Initials_LeagueId_Index", IsUnique = true, Order = 2)]
        [Display(Name = "League")]
        public int LeagueId { get; set; }

        #region relaciones

        public virtual League League { get; set; }

        public virtual ICollection<User> Fans { get; set; }

        public virtual ICollection<Match> Locals { get; set; }

        public virtual ICollection<Match> Visitors { get; set; }

        public virtual ICollection<TournamentTeam> TournamentTeams { get; set; }

        #endregion
    }
}
