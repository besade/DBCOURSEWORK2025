namespace Shop.DTOs
{
    public class CustomerSpendingDto
    {
        public string Email { get; set; } = null!;
        public int OrdersCount { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
