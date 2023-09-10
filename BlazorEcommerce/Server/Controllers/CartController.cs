﻿using BlazorEcommerce.Server.Services.CartService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService CartService)
        {
            _cartService = CartService;
        }

        [HttpPost("products")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>>GetCartProducts(List<CartItem> cartItems)
        {
            var result = await _cartService.GetCartProducts(cartItems);
            return Ok(result);
        }
    }
}
