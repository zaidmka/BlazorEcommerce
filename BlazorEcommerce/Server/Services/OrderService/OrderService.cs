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

        public async Task<ServiceResponse<OrderDetailsResponse>> GetOrdersDetails(int orderId)
        {
            var response  = new ServiceResponse<OrderDetailsResponse>();
            var order = await _dataContext.Orders
                .Include(o=>o.OrderItems)
                .ThenInclude(oi=>oi.Product)
                .Include(o=>o.OrderItems)
                .ThenInclude(oi=>oi.ProductType)
                .Where(o=>o.UserId == _authService.GetUserId()&&o.Id==orderId)
                .OrderByDescending(o=>o.Orderdate)
                .FirstOrDefaultAsync();
            if (order == null)
            {
                response.Success = false;
                response.Message = "Order not found.";
                return response;
            }
            var orderDetailsResponse = new OrderDetailsResponse
            {
                OrderDate = order.Orderdate,
                TotalPrice = order.TotalPrice,
                Products = new List<OrderDetailsProductResponse>()
            };
            order.OrderItems.ForEach(item =>
            orderDetailsResponse.Products.Add(new OrderDetailsProductResponse
            {
                ProductId = item.ProductId,
                ImageUrl=item.Product.ImageUrl,
                ProductType = item.ProductType.Name,
                Quantity=item.Qauntity,
                Title = item.Product.Title,
                TotalPrice=item.TotalPrice

            }));
            response.Data=orderDetailsResponse; 
            return response;

        }

        public async Task<ServiceResponse<bool>> PlaceOrder(int userId)
        {
            var products = (await _cartService.GetDbCartProducts(userId)).Data;
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
                UserId= userId,
                Orderdate = DateTime.Now,
                TotalPrice= totalPrice,
                OrderItems = orderItems
            };

            _dataContext.Orders.Add(order);
            _dataContext.CartItems.RemoveRange(_dataContext.CartItems
                .Where(ci=>ci.UserId == userId));
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
