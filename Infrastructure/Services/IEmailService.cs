namespace Infrastructure.Services
{
    public interface IEmailService
    {
        Task SendConfirmationEmail(string toEmail, string code);
    }
}

