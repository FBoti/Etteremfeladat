using Etterem.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Etterem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
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
                    Description = data.IsDBNull(data.GetOrdinal("description")) ? null : data.GetString("description"),
                    OrderTime = data.GetDateTime("orderTime"),
                    UpdateTime = data.GetDateTime("updateTime"),
                    VendegId = data.GetInt32("vendegId")
                };

                rendelesek.Add(rendeles);
            }

            connection.Close();
            return rendelesek;
        }
    }
}
