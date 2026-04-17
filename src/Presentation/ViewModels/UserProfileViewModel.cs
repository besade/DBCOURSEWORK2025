using Shop.Domain.Models;

namespace Shop.Presentation.ViewModels
{
    public class UserProfileViewModel
    {
        public Customer Customer { get; set; } = null!;
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}