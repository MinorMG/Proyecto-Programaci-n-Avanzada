using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto01.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)] // Máximo 50 caracteres para el nombre
        public string UserName { get; set; }

        [Required]
        [StringLength(100)] // Máximo 100 caracteres para el correo
        [EmailAddress] // Validación de formato de correo
        public string Email { get; set; }

        [Required]
        [StringLength(50)] // Contraseña basica
        public string Password { get; set; }



    }
}