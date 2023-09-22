using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.CartService;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _dataContext;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        public OrderService(DataContext dataContext,ICartService cartService,IAuthService authService)
        {
            _dataContext = dataContext;
            _cartService = cartService;
            _authService = authService;
        }

        public async Task<ServiceResponse<List<OrderOverViewResponse>>> GetOrders()
        {
            var response = new ServiceResponse<List<OrderOverViewResponse>>();
            var orders = await _dataContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == _authService.GetUserId())
                .OrderByDescending(o => o.Orderdate)
                .ToListAsync();
            var orderResponse = new List<OrderOverViewResponse>();
            orders.ForEach(o => orderResponse.Add(new OrderOverViewResponse
            {
                Id = o.Id,
                OrderDate = o.Orderdate,
                TotalPrice = o.TotalPrice,
                Product= o.OrderItems.Count>1?
                $"{o.OrderItems.First().Product.Title} and "+
                $"{o.OrderItems.Count-1} more...":
                o.OrderItems.First().Product.Title,
                ProductImageUrl = o.OrderItems.First().Product.ImageUrl
            }));
            response.Data = orderResponse;
            return response;
        }

        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;
            decimal totalPrice = 0;
            products.ForEach(product =>totalPrice += product.Price*product.Qauntity);

            var orderItems = new List<OrderItem>();
            products.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Qauntity = product.Qauntity,
                TotalPrice = product.Price*product.Qauntity
            }));
            var order = new Order
            {
                UserId= _authService.GetUserId(),
                Orderdate = DateTime.Now,
                TotalPrice= totalPrice,
                OrderItems = orderItems
            };

            _dataContext.Orders.Add(order);
            _dataContext.CartItems.RemoveRange(_dataContext.CartItems
                .Where(ci=>ci.UserId == _authService.GetUserId()));
            await _dataContext.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true,
                Success = true,
                Message = "Order is placed in DB!"
            };

        }
    }
}
