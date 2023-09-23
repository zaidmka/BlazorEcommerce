using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckOutSession();
        Task<ServiceResponse<bool>> fulfillOrder(HttpRequest request);
    }
}
