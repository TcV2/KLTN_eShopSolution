using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers
{
    public class OrderController : Controller
    {
        //private readonly IConfiguration _configuration;
        private readonly IOrderApiClient _orderApiClient;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(IOrderApiClient orderApiClient)
        {
            _orderApiClient = orderApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var request = new GetOrderPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = languageId
            };
            var data = await _orderApiClient.GetAllPaging(request);
            ViewBag.Keyword = keyword;

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }
    }
}
