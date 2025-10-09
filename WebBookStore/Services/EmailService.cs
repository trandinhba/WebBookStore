using System;

namespace WebBookStore.Services
{
    public class EmailService : IEmailService
    {
        public void SendOrderConfirmation(int orderId)
        {
            // Implement actual email sending in production
        }

        public void SendPasswordReset(string email, string resetLink)
        {
            // Implement actual email sending in production
        }

        public void SendWelcomeEmail(string email, string username)
        {
            // Implement actual email sending in production
        }
    }
}