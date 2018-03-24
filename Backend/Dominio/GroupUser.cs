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
    public class GroupUser
    {
        [Key]
        public int GroupUserId { get; set; }

        [Index("GroupUser_GroupId_UserId_Index", IsUnique = true,Order =1)]
        [Display(Name = "Group")]
        public int GroupId { get; set; }

        [Index("GroupUser_GroupId_UserId_Index", IsUnique = true, Order = 2)]
        [Display(Name = "Group")]
        public int UserId { get; set; }

        [Display(Name ="Is accepted?")]
        public bool IsAccepted { get; set; }

        [Display(Name = "Is accepted?")]
        public bool IsBlocked { get; set; }

        public int Points { get; set; }

        #region Relaciones
        [JsonIgnore]
        public virtual Group Group { get; set; }
        #endregion
    }
}
