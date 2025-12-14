using Shop.Models;

namespace Shop.ViewModels
{
    public class UserProfileViewModel
    {
        public Customer Customer { get; set; } = null!;
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}