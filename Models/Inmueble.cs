using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria23.Models;

public class Inmueble
{
    [Key]
    [Display(Name ="Código Interno")]
    public int Id { get; set; }
    
    [Display(Name = "Dirección")]
    [Required]
    public string Direccion { get; set; }

    [Required]
    public string Uso { get; set; }
    [Required]
    public string? Tipo { get; set; }

    [Display(Name ="Cantidad de Ambientes")]
    public int? Ambientes { get; set; }

    public int? Superficie { get; set; } 
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }

    public int? Estado { get; set; }

    public decimal? Precio { get; set; }    
    public int PropietarioId { get; set; }

    [ForeignKey(nameof(PropietarioId))]
    public Propietario? Duenio { get; set; }

    public override string ToString()
    {     
        return $"Codigo: {Id}, Direccion: {Direccion}";
    }

    
}