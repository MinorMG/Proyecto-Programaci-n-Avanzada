using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto01.Models
{

        public class Reserva
        {
            [Key]
            public int IdReserva { get; set; }

            [Required]
            public int IdSala { get; set; }

            [Required]
            public string IdUsuario { get; set; }

            [Required]
            public DateTime FechaReserva { get; set; }

            [Required]
            public TimeSpan HoraInicio { get; set; }

            [Required]
            public TimeSpan HoraFin { get; set; }

            [Required]
            [StringLength(20)]
            public string Aprobacion { get; set; } = "Aprobada";

            // Relaciones
            [ForeignKey("IdSala")]
            public virtual SalasReunion Sala { get; set; }

            [ForeignKey("IdUsuario")]
            public virtual ApplicationUser Usuario { get; set; }
        }
    }