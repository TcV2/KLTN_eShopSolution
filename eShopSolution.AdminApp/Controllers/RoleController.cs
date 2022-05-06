using eShopSolution.ApiIntegration;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers
{
    public class RoleController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRoleApiClient _roleApiClient;

        public RoleController(IRoleApiClient roleApiClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _roleApiClient = roleApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var request = new GetRolePagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _roleApiClient.GetRolePagings(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _roleApiClient.GetById(id);
            return View(result.ResultObj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _roleApiClient.CreateRole(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Thêm mới role thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _roleApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var role = result.ResultObj;
                var updateRequest = new RoleUpdateRequest()
                {
                    Name = role.Name,
                    Description = role.Description,
                    Id = id
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _roleApiClient.UpdateRole(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật role thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            return View(new RoleDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RoleDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _roleApiClient.DeleteRole(request.Id);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Xóa role thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
    }
}
