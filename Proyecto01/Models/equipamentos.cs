using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto01.Models
{
    public class Equipamiento
    {
        [Key]
        public int IdEquipamiento { get; set; }

        [Required]
        [StringLength(50)] // Máximo 50 caracteres
        public string NombreEquipamiento { get; set; }

        // Relación con SalasEquipamientos
        public virtual ICollection<SalasEquipamientos> SalasEquipamientos { get; set; }
    }
}