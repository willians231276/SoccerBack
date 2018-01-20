using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    public class League
    {
        [Key]
        public int LeagueId { get; set; }

        [Required(ErrorMessage ="El campo {0} requerido")]
        [MaxLength(50,ErrorMessage ="La longitud maxima del campo {0} es {1} caracteres.")]
        [Index("League_Name_Index",IsUnique =true)]
        [Display(Name = "League")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }


        #region Relaciones
        public virtual ICollection<Team> Teams { get; set; }
        #endregion
    }
}
