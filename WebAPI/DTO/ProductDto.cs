using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO
{
    public class ProductDto
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to 0")]
        public int Stock { get; set; }

    }
}
