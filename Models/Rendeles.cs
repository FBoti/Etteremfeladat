namespace Etterem.Models
{
    public class Rendeles
    {
        public int Id { get; set; }
        public string Dish { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int VendegId { get; set; }
    }

    public class CreateRendelesDto
    {
        public string Dish { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int VendegId { get; set; }
    }

    public class UpdateRendelesDto
    {
        public string Dish { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
    public class VendegAdataiDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class VendegRendeleseiDto
    {
        public string Name { get; set; } = string.Empty;
        public List<RendelesInfo> Rendelesek { get; set; } = new List<RendelesInfo>();
    }

    public class RendelesInfo
    {
        public string Dish { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
