using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto01.Models
{
    public class Salas
    {
        [Key]
        public int id_sala { get; set; }

        [Required]
        [StringLength(100)] // Máximo 100 caracteres
        public string nombre { get; set; }

        [Required]
        [Range(1, 500)] // Capacidad entre 1 y 500 personas
        public int capacidad { get; set; }

        [Required]
        [StringLength(200)] // Máximo 200 caracteres para ubicación
        public string ubicacion { get; set; }

        [StringLength(500)] // Opcional, descripción de equipo disponible
        public string Equipment { get; set; }

        [Required]
        public TimeSpan hora_inicio { get; set; } // Ejemplo: 09:00

        [Required]
        public TimeSpan hora_fin { get; set; } // Ejemplo: 18:00

        // Relación con Reservation (1 sala tiene muchas reservas)
        public virtual ICollection<Reserva> Reservations { get; set; }



    }
}