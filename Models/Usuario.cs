using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Inmobiliaria23.Models;

public enum enRoles
	{
		SuperAdmin = 1,
		Admin = 2,
		Empleado = 0,
	}
    public class Usuario
        {
            [Key]
            [Display(Name = "Código")]
            public int Id { get; set; }


            [Required]
            public string Nombre { get; set; }


            [Required]
            public string Apellido { get; set; }
            
            
            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, DataType(DataType.Password)]
            public string Clave { get; set; }


            public string? Avatar { get; set; } //AvatarUrl

            [NotMapped] //para Entity Framework
            public IFormFile? AvatarFile { get; set; }


            public int Rol { get; set; }
            [NotMapped]
            public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "Empleado";

            public static IDictionary<int, string> ObtenerRoles()
            {
                SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
                Type tipoEnumRol = typeof(enRoles);
                foreach (var valor in Enum.GetValues(tipoEnumRol))
                {
                    roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
                }
                return roles;
            }
}
