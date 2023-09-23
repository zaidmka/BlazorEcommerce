using Stripe;
using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;

        const string secret = "whsec_12deb74a4a048a01ba9ac5a070dc606683437d330ebb54321018d6526d909cd7";

        public PaymentService(ICartService cartService,
            IAuthService authService,
            IOrderService orderService)
        {
            StripeConfiguration.ApiKey = "sk_test_51NtFwaBfHtaZsZTRNxVi1HppOQ0mOb2zlM4PtrTqHwunH705vFMGBhELS1lvJ7rC7Ng9TIRKhuTlLSOdfS9ee58V00cDlXXLBE";
            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;
        }
        public async Task<Session> CreateCheckOutSession()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;
            var lineItems = new List<SessionLineItemOptions>();
            products.ForEach(product => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        Images = new List<string> { product.ImageUrl }

                    }
                },
                Quantity = product.Qauntity
            }));

            var options = new SessionCreateOptions
            {
                CustomerEmail = _authService.GetUserEmail(),
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries = new List<string> { "US"}
                },
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7226/order-success",
                CancelUrl = "https://localhost:7226/cart"
            };
            var service = new SessionService();
            Session session = service.Create(options);
            return session;

        }

        public async Task<ServiceResponse<bool>> fulfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(

                    json,
                    request.Headers["Stripe-Signature"],
                    secret
                    );
                if(stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = await _authService.GetUserByEmail(session.CustomerEmail);
                    await _orderService.PlaceOrder(user.Id);
                }
                return new ServiceResponse<bool> { Data = true, Message="place order in DB", Success=true };
            }
            catch (StripeException e)
            {
                return new ServiceResponse<bool> { Data=false,Success=false, Message = e.Message };
            }
        }
    }
}
