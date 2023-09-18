using MySql.Data.MySqlClient;

namespace Inmobiliaria23.Models;

public class InmuebleRepositorio
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public InmuebleRepositorio()
    {

    }
    public List<Inmueble> GetInmuebles()
    {
        List<Inmueble> inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT i.Id, Direccion, Ambientes, Superficie, Latitud, Longitud, 
            PropietarioId, Tipo, p.Nombre, p.Apellido, Estado, Precio, Uso  
            FROM inmuebles i INNER JOIN propietarios p ON i.PropietarioId = p.Id";

            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inmueble inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Superficie = reader.GetInt32(3),
                            Latitud = reader.GetDecimal(4),
                            Longitud = reader.GetDecimal(5),
                            PropietarioId = reader.GetInt32(6),
                            Tipo = reader.GetString(7),
                            Estado = reader.GetInt32(10),
                            Precio = (reader.IsDBNull(11)) ? 0 : reader.GetDecimal(11),//reader.GetDouble(11),
                            Uso = reader.GetString(12),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                            }
                        };
                        inmuebles.Add(inmueble);
                    }
                }
            }
            connection.Close();
        }
        return inmuebles;
    }

    public int Alta(Inmueble Inmueble)
    {
        int res = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO inmuebles 
                    (Direccion, Ambientes, Superficie, Latitud, Longitud, PropietarioId, Tipo, Precio, Uso)
					VALUES (@direccion, @ambientes, @superficie, @latitud,  
                    @longitud, @propietarioId, @Tipo, @precio, @Uso);
					SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@direccion", Inmueble.Direccion);
                command.Parameters.AddWithValue("@ambientes", Inmueble.Ambientes);
                command.Parameters.AddWithValue("@superficie", Inmueble.Superficie);
                command.Parameters.AddWithValue("@latitud", Inmueble.Latitud);
                command.Parameters.AddWithValue("@longitud", Inmueble.Longitud);
                command.Parameters.AddWithValue("@propietarioId", (Inmueble.Id == 0) ? Inmueble.PropietarioId : Inmueble.Id);
                command.Parameters.AddWithValue("@Tipo", Inmueble.Tipo);
                command.Parameters.AddWithValue("@precio", Inmueble.Precio);
                command.Parameters.AddWithValue("@Uso", Inmueble.Uso);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                Inmueble.Id = res;
                connection.Close();
            }
        }
        return res;
    }

    public Inmueble GetInmueble(int id)
    {
        Inmueble? p = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @$"SELECT i.Id, Direccion, Ambientes, Superficie,
                    Latitud, Longitud, PropietarioId, Tipo,  
                    p.Nombre, p.Apellido, Estado, Precio, Uso 
                    FROM inmuebles i 
                    JOIN propietarios p ON i.PropietarioId = p.Id
					WHERE i.Id=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int16).Value = id;                
                
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    p = new Inmueble
                    {
                        Id = reader.GetInt32(nameof(Inmueble.Id)),
                        Direccion = reader.GetString("Direccion"),
                        Ambientes = reader.GetInt32("Ambientes"),
                        Superficie = reader.GetInt32("Superficie"),
                        Latitud = reader.GetDecimal("Latitud"),
                        Longitud = reader.GetDecimal("Longitud"),
                        PropietarioId = (reader.IsDBNull(11)) ? 0 :reader.GetInt32("PropietarioId"),
                        Tipo = reader.GetString("Tipo"),
                        Estado = reader.GetInt32("Estado"),
                        Precio = (reader.IsDBNull(11)) ? 0 : reader.GetDecimal(11),
                        Uso = reader.GetString("Uso"),
                        Propietario = new Propietario
                        {
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                        }
                    };
                }
                connection.Close();
            }
        }
        return p;
    }

    public int Modificacion(Inmueble Inmueble)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE inmuebles 
                    SET Direccion=@direccion, Ambientes=@ambientes, 
                    Superficie=@superficie, Latitud=@latitud, Longitud=@longitud, PropietarioId=@propietarioId, 
                    Tipo=@tipo, Estado=@estado, Precio=@precio, Uso=@Uso    
                    WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@direccion", Inmueble.Direccion);
                command.Parameters.AddWithValue("@ambientes", Inmueble.Ambientes);
                command.Parameters.AddWithValue("@superficie", Inmueble.Superficie);
                command.Parameters.AddWithValue("@latitud", Inmueble.Latitud);
                command.Parameters.AddWithValue("@longitud", Inmueble.Longitud);
                command.Parameters.AddWithValue("@propietarioId", Inmueble.PropietarioId);
                command.Parameters.AddWithValue("@tipo", Inmueble.Tipo);
                command.Parameters.AddWithValue("@id", Inmueble.Id);
                command.Parameters.AddWithValue("@estado", Inmueble.Estado);
                command.Parameters.AddWithValue("@precio", Inmueble.Precio);
                command.Parameters.AddWithValue("@Uso", Inmueble.Uso);
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
            string query = "DELETE FROM inmuebles WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public IList<Inmueble> BuscarPorPropietario(int idPropietario)
    {
        List<Inmueble> res = new List<Inmueble>();
        Inmueble inmueble = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"
					SELECT i.Id, Direccion, Ambientes, Superficie, Latitud, Longitud, PropietarioId, 
                    Tipo, p.Nombre, p.Apellido, Estado, Precio, Uso 
					FROM inmuebles i JOIN propietarios p ON i.PropietarioId = p.Id
					WHERE p.Id=@idPropietario";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@idPropietario", MySqlDbType.Int32).Value = idPropietario;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inmueble = new Inmueble
                    {
                        Id = reader.GetInt32(0),
                        Direccion = reader.GetString(1),
                        Ambientes = reader.GetInt32(2),
                        Superficie = reader.GetInt32(3),
                        Latitud = reader.GetDecimal(4),
                        Longitud = reader.GetDecimal(5),
                        Tipo = reader.GetString(7),
                        PropietarioId = reader.GetInt32(6),
                        Estado = reader.GetInt32(10),
                        Precio = reader.GetDecimal(11),
                        Uso = reader.GetString(12),
                        Propietario = new Propietario
                        {
                            Id = reader.GetInt32(6),
                            Nombre = reader.GetString(8),
                            Apellido = reader.GetString(9),
                        }
                    };
                    res.Add(inmueble);
                }
                connection.Close();
            }
        }
        return res;
    }

    public IList<Inmueble> GetInmueblesDisponibles()
    {
        List<Inmueble> Inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT i.Id, Direccion, Ambientes, Superficie, Latitud, Longitud, 
            PropietarioId, Tipo, p.Nombre, p.Apellido, Estado, Precio, Uso   
            FROM inmuebles i 
            INNER JOIN propietarios p 
            ON i.PropietarioId = p.Id 
            WHERE Estado = 1";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inmueble Inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Superficie = reader.GetInt32(3),
                            Latitud = reader.GetDecimal(4),
                            Longitud = reader.GetDecimal(5),
                            PropietarioId = reader.GetInt32(6),
                            Tipo = reader.GetString(7),
                            Estado = reader.GetInt32(10),
                            Precio = (reader.IsDBNull(11)) ? 0 : reader.GetDecimal(11),//reader.GetDouble(11),
                            Uso = reader.GetString(12),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                            }
                        };
                        Inmuebles.Add(Inmueble);
                    }
                }
            }
            connection.Close();
        }
        return Inmuebles;
    }

    public IList<Inmueble> GetInmueblesDisponiblesPorFechas(DateTime desde, DateTime hasta)
    {
        List<Inmueble> Inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT distinct i.Id, Direccion, Ambientes, Superficie, Latitud, Longitud, 
            PropietarioId, Tipo, Estado, i.Precio, Uso, p.Nombre, p.Apellido  
            FROM inmuebles i
            INNER JOIN propietarios p ON i.PropietarioId = p.Id
            LEFT JOIN contratos c ON i.Id = c.InmuebleId
            WHERE c.Id IS NULL
            OR i.Id NOT IN (SELECT InmuebleId    
            FROM contratos c 
            INNER JOIN inmuebles i
			ON  c.InmuebleId = i.Id
			WHERE @desde BETWEEN c.FechaInicio AND c.FechaFin 
            OR @hasta BETWEEN c.FechaInicio AND c.FechaFin)";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@desde", MySqlDbType.Date).Value = desde;
                command.Parameters.Add("@hasta", MySqlDbType.Date).Value = hasta;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inmueble Inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Superficie = reader.GetInt32(3),
                            Latitud = reader.GetDecimal(4),
                            Longitud = reader.GetDecimal(5),
                            PropietarioId = reader.GetInt32(6),
                            Tipo = reader.GetString(7),
                            Estado = reader.GetInt32(8),
                            Precio = (reader.IsDBNull(9)) ? 0 : reader.GetDecimal(9),//reader.GetDouble(11),
                            Uso = reader.GetString(10),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(11),
                                Apellido = reader.GetString(12),
                            }
                        };
                        Inmuebles.Add(Inmueble);
                    }
                }
            }
            connection.Close();
        }
        return Inmuebles;
    }


}