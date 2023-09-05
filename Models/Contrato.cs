using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Inmobiliaria23.Models;

    public class Contrato
        {
            [Key]
            public int Id { get; set; }
            
            [Display(Name = "Fecha Inicio")]
            public DateTime FechaInicio { get; set; }
           
            
            [Display(Name = "Fecha Fin")]
            public DateTime FechaFin { get; set; }


            public decimal? Precio { get; set; }


            [Display(Name = "Inquilino Id")]
            public int InquilinoId { get; set; }
            [ForeignKey(nameof(InquilinoId))]



            [Display(Name = "Inmueble Id")]
            public int InmuebleId { get; set; }
            [ForeignKey(nameof(InmuebleId))]


            public Inquilino inquilino { get; set; }
            public Inmueble inmueble { get; set; }
            public Propietario propietario { get; set; }


            public Contrato(){}

            public Contrato(DateTime FechaInicio, DateTime FechaFin, decimal Precio,int InquilinoId, int InmuebleId, Inquilino inquilino, Inmueble inmueble, Propietario propietario)
                {
                    this.FechaInicio = FechaInicio;
                    this.FechaFin = FechaFin;
                    this.Precio = Precio;
                    this.InquilinoId = InquilinoId;
                    this.InmuebleId = InmuebleId;
                    this.inquilino = inquilino;
                    this.inmueble = inmueble;
                    this.propietario = propietario;
                }


         public override string ToString()
    {     
        return "Contrato ID: " + Id;
    }
}
