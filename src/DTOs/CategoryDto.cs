using System.ComponentModel.DataAnnotations;

namespace Shop.DTOs
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Need to input name of the category.")]
        [MaxLength(100)]
        public string CategoryName { get; set; } = null!;
    }
}