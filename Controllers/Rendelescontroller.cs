using Etterem.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Etterem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RendelesController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;password=;database=etterem;";

        [HttpGet]
        public List<Rendeles> GetRendelesek()
        {
            List<Rendeles> rendelesek = new List<Rendeles>();

            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = "SELECT * FROM `rendeles`;";
            var cmd = new MySqlCommand(sql, connection);
            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                var rendeles = new Rendeles
                {
                    Id = data.GetInt32("id"),
                    Dish = data.GetString("dish"),
                    Description = data.GetString("description"),
                    OrderTime = data.GetDateTime("orderTime"),
                    UpdateTime = data.GetDateTime("updateTime"),
                    VendegId = data.GetInt32("vendegId")
                };

                rendelesek.Add(rendeles);
            }

            connection.Close();
            return rendelesek;
        }
        [HttpGet("byId")]
        public object GetRendelesById(int id)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"SELECT * FROM `rendeles` WHERE `id` = @id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            object? data = null;

            if (datareader.Read() == true)
            {
                string? description = null;

                var rendeles = new Rendeles
                {
                    Id = datareader.GetInt32(0),
                    Dish = datareader.GetString(1),
                    Description = description,
                    OrderTime = datareader.GetDateTime(3),
                    UpdateTime = datareader.GetDateTime(4),
                    VendegId = datareader.GetInt32(5)
                };

                data = new { message = "Sikeres lekérdezés.", result = rendeles };
            }
            else
            {
                data = new { message = "Nincs ilyen rendelés.", result = "" };
            }

            connection.Close();

            return data;
        }

        [HttpPost]
        public object AddNewRendeles([FromBody] CreateRendelesDto createRendelesDto)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"INSERT INTO `rendeles`(`dish`, `description`, `orderTime`, `updateTime`, `vendegId`) VALUES (@dish, @description, @orderTime, @updateTime, @vendegId)";

            var cmd = new MySqlCommand(sql, connection);

            var now = DateTime.Now;

            cmd.Parameters.AddWithValue("@dish", createRendelesDto.Dish);
            cmd.Parameters.AddWithValue("@description", createRendelesDto.Description);
            cmd.Parameters.AddWithValue("@orderTime", now);
            cmd.Parameters.AddWithValue("@updateTime", now);
            cmd.Parameters.AddWithValue("@vendegId", createRendelesDto.VendegId);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres felvétel.", result = createRendelesDto };
        }
        [HttpDelete]
        public object DeleteRendeles(int id)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"DELETE FROM `rendeles` WHERE `id` = @id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres törlés.", result = "" };
        }
        [HttpPut]
        public object UpdateRendeles([FromQuery] int id, [FromBody] UpdateRendelesDto updateRendelesDto)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            var sql = @"UPDATE `rendeles` SET `dish`=@dish, `description`=@description, `updateTime`=@updateTime WHERE `id` = @id;";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@dish", updateRendelesDto.Dish);
            cmd.Parameters.AddWithValue("@description", updateRendelesDto.Description);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres frissítés", result = updateRendelesDto };
        }
        [HttpGet("vendegAdatai")]
        public object GetVendegAdatai(int vendegId)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = @"SELECT `name`, `email` FROM `vendeg` WHERE `id` = @id;";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", vendegId);

            var datareader = cmd.ExecuteReader();
            object? data = null;

            if (datareader.Read() == true)
            {
                var vendegDto = new VendegAdataiDto
                {
                    Name = datareader.GetString(0),
                    Email = datareader.GetString(1)
                };

                data = new { message = "Sikeres lekérdezés.", result = vendegDto };
            }
            else
            {
                data = new { message = "Nincs ilyen vendég.", result = "" };
            }

            connection.Close();
            return data;

        }
        [HttpGet("vendegRendelesei")]
        public object GetVendegRendelesei(int vendegId)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = @"SELECT v.name, r.dish, r.description 
                           FROM `vendeg` v 
                           LEFT JOIN `rendeles` r ON v.id = r.vendegId 
                           WHERE v.id = @vendegId;";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@vendegId", vendegId);

            var datareader = cmd.ExecuteReader();
            VendegRendeleseiDto? vendegRendelesei = null;

            while (datareader.Read())
            {
                if (vendegRendelesei == null)
                {
                    vendegRendelesei = new VendegRendeleseiDto
                    {
                        Name = datareader.GetString(0),
                        Rendelesek = new List<RendelesInfo>()
                    };
                }

                if (datareader.IsDBNull(1) == false)
                {
                    string? description = null;

                    if (datareader.IsDBNull(2) == false)
                    {
                        description = datareader.GetString(2);
                    }

                    var rendelesInfo = new RendelesInfo
                    {
                        Dish = datareader.GetString(1),
                        Description = description
                    };

                    vendegRendelesei.Rendelesek.Add(rendelesInfo);
                }
            }

            object? data = null;

            if (vendegRendelesei != null)
            {
                data = new { message = "Sikeres lekérdezés.", result = vendegRendelesei };
            }
            else
            {
                data = new { message = "Nincs ilyen vendég.", result = "" };
            }

            connection.Close();
            return data;
        }
        [HttpGet("osszesRendelesSzama")]
        public object GetOsszesRendelesSzama()
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = @"SELECT COUNT(*) FROM `rendeles`;";
            var cmd = new MySqlCommand(sql, connection);

            var datareader = cmd.ExecuteReader();
            object? data = null;

            if (datareader.Read() == true)
            {
                int count = datareader.GetInt32(0);
                data = new { message = "Sikeres lekérdezés.", result = new { osszesRendeles = count }};
            }

            connection.Close();
            if (data == null)
            {
                data = new { message = "Hiba történt a lekérdezés során.", result = "" };
            }
            return data;
        }
        [HttpGet("vendegRendeleseinekSzama")]
        public object GetVendegRendeleseinekSzama(int vendegId)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = @"SELECT COUNT(*) FROM `rendeles` WHERE `vendegId` = @vendegId;";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@vendegId", vendegId);

            var datareader = cmd.ExecuteReader();
            object? data = null;

            if (datareader.Read() == true)
            {
                int count = datareader.GetInt32(0);
                data = new { message = "Sikeres lekérdezés.", result = new { vendegId = vendegId, rendeleseikSzama = count } };
            }

            connection.Close();
            if (data == null)
            {
                data = new { message = "Hiba történt a lekérdezés során.", result = "" };
            }
            return data;
        }
    }
}
