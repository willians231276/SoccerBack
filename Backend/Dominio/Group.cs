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
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "El campo {0} requerido")]
        [MaxLength(50, ErrorMessage = "La longitud maxima del campo {0} es {1} caracteres.")]
        [Index("Group_Name_Index", IsUnique = true)]
        [Display(Name = "Group")]
        public string Name { get; set; }

        public int OnwerId { get; set; }

        #region Relaciones

        [JsonIgnore]
        public virtual User Owner { get; set; }

        [JsonIgnore]
        public virtual ICollection<GroupUser> GroupUsers { get; set; } 
        #endregion
    }
}
