using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryController(IConfiguration configuration, 
            ICategoryApiClient categoryApiClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _categoryApiClient = categoryApiClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var request = new GetCategoryPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = languageId
            };
            var data = await _categoryApiClient.GetAllPaging(request);
            ViewBag.Keyword = keyword;

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _categoryApiClient.CreateCategory(request);
            if (result.ResultObj)
            {
                TempData["result"] = "Thêm mới danh mục thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var category = await _categoryApiClient.GetById(languageId, id);
            var editVm = new CategoryUpdateRequest()
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
                LanguageId = languageId
            };
            return View(editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateRequest request)
        {
            request.LanguageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            if (!ModelState.IsValid)
                return View(request);

            var result = await _categoryApiClient.UpdateCategory(request);
            if (result)
            {
                TempData["result"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật danh mục thất bại");
            return View(request);
        }
    }
}
