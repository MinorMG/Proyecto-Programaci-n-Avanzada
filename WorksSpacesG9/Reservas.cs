//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorksSpacesG9
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservas
    {
        public int id_reserva { get; set; }
        public int id_sala { get; set; }
        public int id_usuario { get; set; }
        public System.DateTime fecha_reserva { get; set; }
        public System.TimeSpan hora_inicio { get; set; }
        public System.TimeSpan hora_fin { get; set; }
        public string aprobacion { get; set; }
    
        public virtual Salas_reunion Salas_reunion { get; set; }
        public virtual Usuarios Usuarios { get; set; }
    }
}
