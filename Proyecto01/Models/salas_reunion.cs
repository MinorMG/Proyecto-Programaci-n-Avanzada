using Proyecto01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto01.Models
{
    //salas
    public class SalasReunion
    {
        [Key]
        public int IdSala { get; set; }

        [Required]
        [StringLength(20)] // Máximo 20 caracteres
        public string Nombre { get; set; }

        [Required]
        public int Capacidad { get; set; }

        [Required]
        [StringLength(50)] // Máximo 50 caracteres
        public string Ubicacion { get; set; }

        [Required]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        public TimeSpan HoraFin { get; set; }

        public virtual ICollection<Equipamiento> Equipamientos { get; set; }


        // Relación con SalasEquipamientos
        public virtual ICollection<SalasEquipamientos> SalasEquipamientos { get; set; }

        // Relación con Reservas
        public virtual ICollection<Reserva> Reservas { get; set; }

        // Relación con Estadisticas
        public virtual ICollection<Estadistica> Estadisticas { get; set; }
    }
}