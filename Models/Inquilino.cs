using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Inmobiliaria23.Models;

    public class Inquilino
        {
            [Key]
            [Display(Name = "Código")]
            public int Id { get; set; }
            [Required]
            public string Nombre { get; set; }
            [Required]
            public string Apellido { get; set; }
            [Required]
            public string DNI { get; set; }

            [Display(Name ="Teléfono")]
            public string? Telefono { get; set; }
            [Required, EmailAddress]
            public string? Email { get; set; }
         public override string ToString()
    {     
        return $"{Nombre} {Apellido}";
    }
}
