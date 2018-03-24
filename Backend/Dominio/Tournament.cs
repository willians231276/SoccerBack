using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Tournament
    {
        [Key]
        public int TournamentId { get; set; }

        [Required(ErrorMessage = "El campo {0} requerido")]
        [MaxLength(50, ErrorMessage = "La longitud maxima del campo {0} es {1} caracteres.")]
        [Index("Tournament_Name_Index", IsUnique = true)]
        [Display(Name = "Tournament")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        [Display(Name="Order")]
        public int Order { get; set; }

        [JsonIgnore]
        public ICollection<TournamentGroup> TournamentGroups { get; set; }

        [JsonIgnore]
        public virtual ICollection<Date> Dates { get; set; }
    }
}
