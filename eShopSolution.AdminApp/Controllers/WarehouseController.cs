using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Warehouses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWarehouseApiClient _warehouseApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WarehouseController(IConfiguration configuration,
            IWarehouseApiClient warehouseApiClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _warehouseApiClient = warehouseApiClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var request = new GetWarehousePagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = languageId
            };
            var data = await _warehouseApiClient.GetAllPaging(request);
            ViewBag.Keyword = keyword;

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> EditPrice(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var wh = await _warehouseApiClient.GetById(languageId, id);
            var editVm = new UpdatePriceRequest()
            {
                Id = wh.Id,
                NewPrice = wh.Price,
                NewOriginalPrice = wh.OriginalPrice
            };
            return View(editVm);
        }

        [HttpPost]
        public async Task<IActionResult> EditPrice(UpdatePriceRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _warehouseApiClient.UpdatePrice(request);
            if (result)
            {
                TempData["result"] = "Cập nhật giá thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật giá thất bại");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> EditStock(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var wh = await _warehouseApiClient.GetById(languageId, id);
            var editVm = new UpdateStockRequest()
            {
                Id = wh.Id,
                Quantity = 0
            };
            return View(editVm);
        }

        [HttpPost]
        public async Task<IActionResult> EditStock(UpdateStockRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _warehouseApiClient.UpdateStock(request);
            if (result)
            {
                TempData["result"] = "Nhập hàng thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Nhập hàng thất bại");
            return View(request);
        }
    }
}
