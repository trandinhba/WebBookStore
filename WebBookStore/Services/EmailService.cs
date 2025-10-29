using System;

namespace WebBookStore.Services
{
    public class EmailService : IEmailService
    {
        public void SendOrderConfirmation(int orderId)
        {
            // Implement actual email sending in production
            // For now, just log to console
            Console.WriteLine($"Order confirmation email sent for order {orderId}");
        }

        public void SendPasswordReset(string email, string resetLink)
        {
            // Implement actual email sending in production
            Console.WriteLine($"Password reset email sent to {email}");
        }

        public void SendWelcomeEmail(string email, string username)
        {
            // Implement actual email sending in production
            Console.WriteLine($"Welcome email sent to {email} for user {username}");
        }

        public void SendInvoiceToAdmin(int orderId)
        {
            // Implement actual email sending in production
            // For now, just log to console
            Console.WriteLine($"Invoice sent to admin for order {orderId}");
            
            // In production, this would:
            // 1. Get order details from database
            // 2. Generate invoice PDF
            // 3. Send email to admin with invoice attachment
            // 4. Log the action
        }
    }
}