namespace WebBookStore.Services
{
    public interface IEmailService
    {
        void SendOrderConfirmation(int orderId);
        void SendPasswordReset(string email, string resetLink);
        void SendWelcomeEmail(string email, string username);
        void SendInvoiceToAdmin(int orderId);
    }
}