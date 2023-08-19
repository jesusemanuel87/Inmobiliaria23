using MySql.Data.MySqlClient;

namespace Inmobiliaria23.Models;

public class RepositorioPropietario
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public RepositorioPropietario()
    {

    }
    public List<Propietario> GetPropietarios()
    {
        List<Propietario> propietarios = new List<Propietario>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id, Nombre, Apellido, DNI, Telefono, Email FROM propietarios";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Propietario propietario = new Propietario
                        {
                            Id = reader.GetInt32(nameof(Propietario.Id)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            DNI = reader.GetString(nameof(Propietario.DNI)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Email = reader.GetString(nameof(Propietario.Email)),
                        };
                        propietarios.Add(propietario);
                    }
                }
            }
            connection.Close();
        }
        return propietarios;
    }

    public int Alta(Propietario Propietario)
    {
        int res = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO propietarios (Nombre, Apellido, DNI, Telefono, Email)
                    VALUES (@Nombre, @Apellido, @DNI, @Telefono, @Email);
                    SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", Propietario.Nombre);
                command.Parameters.AddWithValue("@Apellido", Propietario.Apellido);
                command.Parameters.AddWithValue("@DNI", Propietario.DNI);
                command.Parameters.AddWithValue("@Telefono", Propietario.Telefono);
                command.Parameters.AddWithValue("@Email", Propietario.Email);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                Propietario.Id = res;
                connection.Close();
            }
        }
        return res;
    }

    public Propietario GetPropietario(int id)
    {
        Propietario? p = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT Id, Nombre, Apellido, Dni, Telefono, Email  
					FROM propietarios
					WHERE Id=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int16).Value = id;
                //command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    p = new Propietario
                    {
                        Id = reader.GetInt32(nameof(Propietario.Id)),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        DNI = reader.GetString("DNI"),
                        Telefono = reader.GetString("Telefono"),
                        Email = reader.GetString("Email"),
                    };
                }
                connection.Close();
            }
        }
        return p;
    }

    public int Modificacion(Propietario p)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE propietarios 
					SET Nombre=@nombre, Apellido=@apellido, DNI=@DNI, Telefono=@telefono, Email=@email 
					WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // command.CommandType = CommandType.Text;
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
            string query = "DELETE FROM propietarios WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                //command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

}