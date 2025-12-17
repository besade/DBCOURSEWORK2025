namespace Shop.DTOs
{
    public class CategorySalesDto
    {
        public string CategoryName { get; set; } = null!;
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
