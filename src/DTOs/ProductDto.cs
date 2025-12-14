using System.ComponentModel.DataAnnotations;

namespace Shop.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Need to input name of the product.")]
        [MaxLength(100)]
        public string ProductName { get; set; } = null!;

        [Required(ErrorMessage = "Need to input category ID.")]
        [Range(1, int.MaxValue, ErrorMessage = "Category ID needs to be a positive number.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Need to input product country name.")]
        [MaxLength(50)]
        public string ProductCountry { get; set; } = null!;

        [Required(ErrorMessage = "Need to input product weight.")]
        [Range(0.01, 10000.0, ErrorMessage = "Weight needs to be a positive number.")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Need to input stock quantity.")]
        [Range(0, 10000, ErrorMessage = "Quantity needs to be a positive number or zero.")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Need to input price.")]
        [Range(0.01, 100000.0, ErrorMessage = "Price needs to be a positive number.")]
        public decimal Price { get; set; }

        public IFormFile? PictureFile { get; set; }
    }
}