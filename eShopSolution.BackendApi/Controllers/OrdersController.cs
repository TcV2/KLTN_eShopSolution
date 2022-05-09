using System;
using System.Threading.Tasks;
using eShopSolution.Application.Sales;
using eShopSolution.ViewModels.Sales;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(
            IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(string languageId)
        {
            var orders = await _orderService.GetAll(languageId);
            return Ok(orders);
        }

        [HttpGet("paging")]
        [Authorize]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetOrderPagingRequest request)
        {
            var orders = await _orderService.GetOrderPaging(request);
            return Ok(orders);
        }

        [HttpPut("{orderId}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int orderId, [FromBody] OrderConfirmRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = orderId;
            var affectedResult = await _orderService.Update(request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(Guid id, [FromBody]CheckoutViewModel request)
        {
            //Guid guid = (Guid)Membership.GetUser().ProviderUserKey;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _orderService.Checkout(id,request);
            if (result>0)
            {
                //return BadRequest(result);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
