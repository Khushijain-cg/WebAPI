using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }

        public string? Category { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; }

        [Range(0, int.MaxValue)]
        public int? Stock { get; set; }
    }
}
