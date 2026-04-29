using Shop.Application.Interfaces;

namespace Shop.Infrastructure.Notifications
{
    public class ConsoleNotificationSender : INotificationSender
    {
        public async Task SendWelcomeMessageAsync(string email, string userName)
        {
            await Task.Delay(2000);

            Console.WriteLine($"На {email} було надіслано повідомлення: \"{userName}, дякуємо за реєстрацію на нашому сайті!\"");
        }
    }
}
