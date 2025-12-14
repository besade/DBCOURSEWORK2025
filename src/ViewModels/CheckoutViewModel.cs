using Shop.Models;

namespace Shop.ViewModels
{
    public class CheckoutViewModel

    {
        public string RecipientFirstName { get; set; } = null!;
        public string RecipientLastName { get; set; } = null!;
        public bool CustomerIsRecipient { get; set; }
        public DeliveryType Delivery { get; set; }
    }
}