using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proyecto01.Models
{
    public class Estadistica
    {
        [Key]
        public int IdEstadistica { get; set; }

        [Required]
        public int IdSala { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public decimal HorasUso { get; set; }

        [Required]
        public int NumeroReservas { get; set; }

        [Required]
        public decimal PorcentajeOcupacion { get; set; }

        // Relación
        [ForeignKey("IdSala")]
        public virtual SalasReunion Sala { get; set; }
    }
}