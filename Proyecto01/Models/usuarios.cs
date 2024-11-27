using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto01.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(25)] // Máximo 25 caracteres
        public string Nombre { get; set; }

        [Required]
        [StringLength(255)] // Máximo 255 caracteres
        public string Contrasena { get; set; }

        [Required]
        [StringLength(50)] // Máximo 50 caracteres
        public string Email { get; set; }

        [Required]
        public bool Administrador { get; set; } = false;

        // Relación con Reservas
        public virtual ICollection<reservas> Reservas { get; set; }
    }
}