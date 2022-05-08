using System.Threading.Tasks;
using eShopSolution.Application.Sales;
using eShopSolution.ViewModels.Sales;
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
        //[Authorize]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetOrderPagingRequest request)
        {
            var orders = await _orderService.GetOrderPaging(request);
            return Ok(orders);
        }
    }
}
