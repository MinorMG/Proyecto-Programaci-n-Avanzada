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
    
    public partial class Estadisticas
    {
        public int id_estadistica { get; set; }
        public int id_sala { get; set; }
        public System.DateTime fecha { get; set; }
        public decimal horas_uso { get; set; }
        public int numero_reservas { get; set; }
        public decimal porcentaje_ocupacion { get; set; }
    
        public virtual Salas_reunion Salas_reunion { get; set; }
    }
}
