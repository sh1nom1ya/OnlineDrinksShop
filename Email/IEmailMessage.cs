using System;
namespace drunkShop.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage emailMessage);
    }

}

