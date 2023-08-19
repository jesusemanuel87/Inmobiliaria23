using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria23.Models;

public class Propietario
{
    [Key]
    [Display(Name ="Código Interno")]
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
    public string? Clave { get; set; }
    // public Propietario(string Nombre, string Apellido, string DNI, string Telefono, string Email, string Clave)
    // {
    //     this.Nombre = Nombre;
    //     this.Apellido = Apellido;
    //     this.DNI = DNI;
    //     this.Telefono = Telefono;
    //     this.Email = Email;
    //     this.Clave = Clave;
    // }    
    public override string ToString()
    {     
        return $"{Nombre} {Apellido}";
    }
}