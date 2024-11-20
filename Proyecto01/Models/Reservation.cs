using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto01.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [Required]
        public int RoomId { get; set; } // Llave foránea a Room

        [ForeignKey("RoomId")]
        public virtual Rooms Rooms { get; set; } // Relación con Room

        [ForeignKey("UserId")]
        public virtual User user{ get; set; } // Relación con Room

        [Required]
        public DateTime ReservationDate { get; set; } // Fecha de la reserva

        [Required]
        public TimeSpan StartTime { get; set; } // Hora de inicio

        [Required]
        public TimeSpan EndTime { get; set; } // Hora de fin
    }
}