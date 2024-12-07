using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proyecto01.Models
{
    public class SalasEquipamientos
    {

        [Key, Column(Order = 0)]
        public int IdSala { get; set; }

        [Key, Column(Order = 1)]
        public int IdEquipamiento { get; set; }

        // Relaciones
        [ForeignKey("IdSala")]
        public virtual SalasReunion Sala { get; set; }

        [ForeignKey("IdEquipamiento")]
        public virtual Equipamiento Equipamiento { get; set; }
    }
}