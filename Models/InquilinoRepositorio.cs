using System.Data;
using Humanizer;
using MySql.Data.MySqlClient;

namespace Inmobiliaria23.Models;

public class InquilinoRepositorio
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public InquilinoRepositorio()
    {

    }
    public List<Inquilino> GetInquilinos()
    {
        List<Inquilino> inquilinos = new List<Inquilino>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id, Nombre, Apellido, DNI, Telefono, Email FROM inquilinos";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inquilino inquilino = new Inquilino
                        {
                            Id = reader.GetInt32(nameof(Inquilino.Id)),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            DNI = reader.GetString(nameof(Inquilino.DNI)),
                            Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                            Email = reader.GetString(nameof(Inquilino.Email)),
                        };
                        inquilinos.Add(inquilino);
                    }
                }
            }
            connection.Close();
        }
        return inquilinos;
    }

    public int Alta(Inquilino Inquilino)
    {
        int res = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO inquilinos (Nombre, Apellido, DNI, Telefono, Email)
                    VALUES (@Nombre, @Apellido, @DNI, @Telefono, @Email);
                    SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", Inquilino.Nombre);
                command.Parameters.AddWithValue("@Apellido", Inquilino.Apellido);
                command.Parameters.AddWithValue("@DNI", Inquilino.DNI);
                command.Parameters.AddWithValue("@Telefono", Inquilino.Telefono);
                command.Parameters.AddWithValue("@Email", Inquilino.Email);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                Inquilino.Id = res;
                connection.Close();
            }
        }
        return res;
    }

    public List<Contrato> ObtenerContratosDelInquilino(int inquilinoId)
        {
            List<Contrato> contratos = new List<Contrato>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"SELECT c.Id AS ContratoId, c.InmuebleId, c.FechaInicio, c.FechaFin, c.Precio , i.Direccion, i.Tipo
                                FROM contratos c
                                INNER JOIN inmuebles i ON c.InmuebleId = i.Id
                                WHERE InquilinoId = @inquilinoId";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.Add("@inquilinoId", MySqlDbType.Int16).Value = inquilinoId;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contrato contrato = new Contrato
                        {
                            Id = reader.GetInt32("ContratoId"),
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            FechaInicio = reader.GetDateTime("FechaInicio"),
                            FechaFin = reader.GetDateTime("FechaFin"),
                            Precio = reader.GetDecimal("Precio"),
                            inmueble = new Inmueble
                            {
                                Id = reader.GetInt32("InmuebleId"),
                                Tipo = reader.GetString("Tipo"),
                                Direccion = reader.GetString("Direccion"),
                            }                                                     
                        };
                        contratos.Add(contrato);
                    }
                    connection.Close();
                }
            }
            return contratos;
        }

    public Inquilino GetInquilino(int id)
    {
        Inquilino? p = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT Id, Nombre, Apellido, Dni, Telefono, Email  
					FROM inquilinos
					WHERE Id=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int16).Value = id;
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    p = new Inquilino
                    {
                        Id = reader.GetInt32(nameof(Inquilino.Id)),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        DNI = reader.GetString("DNI"),
                        Telefono = reader.GetString("Telefono"),
                        Email = reader.GetString("Email"),
                        Contratos = new List<Contrato>()
                    };
                }
                connection.Close();
            }
        }
        return p;
    }

    public int Modificacion(Inquilino p)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE inquilinos 
					SET Nombre=@nombre, Apellido=@apellido, DNI=@DNI, Telefono=@telefono, Email=@email 
					WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", p.Id);
                command.Parameters.AddWithValue("@nombre", p.Nombre);
                command.Parameters.AddWithValue("@apellido", p.Apellido);
                command.Parameters.AddWithValue("@DNI", p.DNI);
                command.Parameters.AddWithValue("@telefono", p.Telefono);                
                command.Parameters.AddWithValue("@email", p.Email);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public int Baja(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "DELETE FROM inquilinos WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

}