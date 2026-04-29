namespace Shop.Application.Interfaces
{
    public interface INotificationSender
    {
        Task SendWelcomeMessageAsync(string email, string userName);
    }
}
