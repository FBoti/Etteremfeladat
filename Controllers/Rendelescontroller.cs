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

        
    }
}
