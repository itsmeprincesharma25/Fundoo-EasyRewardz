namespace MessageService.Interface
{
    public interface IMessageService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}