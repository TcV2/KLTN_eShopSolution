using eShopSolution.Application.Catalog.Warehouses;
using eShopSolution.ViewModels.Catalog.Warehouses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : Controller
    {
        private readonly IWarehouseService _warehouseService;

        public WarehousesController(
            IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetWarehousePagingRequest request)
        {
            var wh = await _warehouseService.GetWarehousePaging(request);
            return Ok(wh);
        }

        [HttpGet("{id}/{languageId}")]
        public async Task<IActionResult> GetById(string languageId, int id)
        {
            var wh = await _warehouseService.GetById(languageId, id);
            return Ok(wh);
        }

        [HttpPut("price/{productId}")]
        [Authorize]
        public async Task<IActionResult> UpdatePrice([FromRoute] int productId, [FromBody] UpdatePriceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = productId;
            var affectedResult = await _warehouseService.UpdatePrice(request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        [HttpPut("stock/{productId}")]
        [Authorize]
        public async Task<IActionResult> UpdateStock([FromRoute] int productId, [FromBody] UpdateStockRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = productId;
            var affectedResult = await _warehouseService.UpdateStock(request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }
    }
}
