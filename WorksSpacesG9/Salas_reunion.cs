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
    
    public partial class Salas_reunion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Salas_reunion()
        {
            this.Estadisticas = new HashSet<Estadisticas>();
            this.Reservas = new HashSet<Reservas>();
            this.Equipamientos = new HashSet<Equipamientos>();
        }
    
        public int id_sala { get; set; }
        public string nombre { get; set; }
        public int capacidad { get; set; }
        public string ubicacion { get; set; }
        public System.TimeSpan hora_inicio { get; set; }
        public System.TimeSpan hora_fin { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Estadisticas> Estadisticas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservas> Reservas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Equipamientos> Equipamientos { get; set; }
    }
}