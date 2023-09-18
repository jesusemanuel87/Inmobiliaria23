using MySql.Data.MySqlClient;

namespace Inmobiliaria23.Models;

public class UsuarioRepositorio
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public UsuarioRepositorio(){}
    public List<Usuario> GetUsuarios()
    {
        List<Usuario> usuarios = new List<Usuario>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id, Nombre, Apellido, Avatar, Email, Clave, Rol FROM usuarios";

            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Usuario usuario = new Usuario
                        {
                            Id = reader.GetInt32(nameof(Usuario.Id)),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Avatar = (reader.IsDBNull(3)) ? "" : reader.GetString(3),
                            Email = reader.GetString(nameof(Usuario.Email)),
                            Clave = reader.GetString(nameof(Usuario.Clave)),
                            Rol = reader.GetInt32(nameof(Usuario.Rol)),
                        };
                        usuarios.Add(usuario);
                    }
                }
            }
            connection.Close();
        }
        return usuarios;
    }


    public Usuario GetUsuario(int id)
    {
        Usuario? p = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query =  @"SELECT Id, Nombre, Apellido, Avatar, Email, Clave, Rol 
                                FROM usuarios  WHERE Id=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int16).Value = id;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    p = new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Avatar = (reader.IsDBNull(3)) ? "" : reader.GetString(3),
                        Email = reader.GetString("Email"),
                        Clave = reader.GetString("Clave"),
                        Rol = reader.GetInt32("Rol"),
                    };
                }
                connection.Close();
            }
        }
        return p;
    }


    public int Alta(Usuario Usuario)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO usuarios 
                                    (Nombre, Apellido, Avatar, Email, Clave, Rol) 
                                VALUES (@nombre, @apellido, @avatar, @email, @clave, @rol);
                                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nombre", Usuario.Nombre);
                command.Parameters.AddWithValue("@apellido", Usuario.Apellido);
                    if (String.IsNullOrEmpty(Usuario.Avatar))
                        command.Parameters.AddWithValue("@avatar","/Uploads/sinFoto.png");
                    else
                        command.Parameters.AddWithValue("@avatar", Usuario.Avatar);
                command.Parameters.AddWithValue("@email", Usuario.Email);
                command.Parameters.AddWithValue("@clave", Usuario.Clave);
                command.Parameters.AddWithValue("@rol", Usuario.Rol);

                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                Usuario.Id = res;
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
            string query = "DELETE FROM usuarios WHERE Id = @id";
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


    public int Modificacion(Usuario p)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE usuarios 
                                SET Nombre=@nombre, Apellido=@apellido, Avatar=@avatar, Email=@email, Clave=@clave, Rol=@rol 
                                WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nombre", p.Nombre);
                command.Parameters.AddWithValue("@apellido", p.Apellido);
                command.Parameters.AddWithValue("@avatar", p.Avatar);
                command.Parameters.AddWithValue("@email", p.Email);
                command.Parameters.AddWithValue("@clave", p.Clave);
                command.Parameters.AddWithValue("@rol", p.Rol);
                command.Parameters.AddWithValue("@id", p.Id);

                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }


    public Usuario ObtenerPorEmail(string email)
    {
        Usuario? p = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT Id, Nombre, Apellido, Avatar, Email, Clave, Rol FROM usuarios WHERE Email=@email";
           
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    p = new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Avatar = reader.GetString("Avatar"),
                        Email = reader.GetString("Email"),
                        Clave = reader.GetString("Clave"),
                        Rol = reader.GetInt32("Rol"),
                    };
                }
                connection.Close();
            }
        }
        return p;
    }

}