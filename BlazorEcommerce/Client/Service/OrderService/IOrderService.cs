namespace BlazorEcommerce.Client.Service.OrderService
{
    public interface IOrderService
    {
        Task PlaceOrder();
        Task<List<OrderOverViewResponse>> GetOrders();
    }
}
