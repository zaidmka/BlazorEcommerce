﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("checkout"),Authorize]

        public async Task<ActionResult<string>> CreateCheckOutSession()
        {
            var session = await _paymentService.CreateCheckOutSession();
            return Ok(session.Url);
        }

        [HttpPost]

        public async Task<ActionResult<ServiceResponse<bool>>> FulfillOrder()
        {
            var response = await _paymentService.fulfillOrder(Request);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }

    }
}