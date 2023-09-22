﻿namespace BlazorEcommerce.Server.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder();
        Task<ServiceResponse<List<OrderOverViewResponse>>> GetOrders();

        Task<ServiceResponse<OrderDetailsResponse>> GetOrdersDetails(int orderId);
    }
}
