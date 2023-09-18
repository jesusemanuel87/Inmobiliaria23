using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Inmobiliaria23.Models;

    public class Pago
        {
            [Key]
            [Display(Name = "Código Pago")]
            public int Id { get; set; }


            [Display(Name = "Fecha de pago")]
            public DateTime? FechaPagado { get; set; }


            [Display(Name = "N° Cuota")]
            public int Mes { get; set; }


            [Display(Name = "Código Contrato")]
            public int ContratoId { get; set; }

            public Decimal? Importe { get; set; }
            public Contrato contrato { get; set; }
            public Inquilino inquilino { get; set; }

            public Pago(DateTime FechaPagado, int Mes, int ContratoId, Contrato contrato)
                {
                    this.FechaPagado = FechaPagado;
                    this.Mes = Mes;
                    this.ContratoId = ContratoId;
                    this.contrato = contrato;
                }

    public Pago() { }
    
    public override string ToString()
    {
        return $"Id({Id}),Mes:{Mes},Contrato:{ContratoId}";
    }
}
