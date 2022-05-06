using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RoleVm>> GetAll()
        {
            var roles = await _roleManager.Roles
                .Select(x => new RoleVm()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync();

            return roles;
        }

        public async Task<ApiResult<bool>> Create(RoleCreateRequest request)
        {
            var role = await _roleManager.FindByNameAsync(request.Name);
            if (role != null)
            {
                return new ApiErrorResult<bool>("Role đã tồn tại");
            }

            role = new AppRole()
            {
                Name = request.Name,
                Description = request.Description
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Thêm mới không thành công");
        }

        public async Task<ApiResult<bool>> Delete(Guid roleId)
        {
            var user = await _roleManager.FindByIdAsync(roleId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Role không tồn tại");
            }
            var reult = await _roleManager.DeleteAsync(user);
            if (reult.Succeeded)
                return new ApiSuccessResult<bool>();

            return new ApiErrorResult<bool>("Xóa không thành công");
        }

        public async Task<ApiResult<RoleVm>> GetById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return new ApiErrorResult<RoleVm>("Role không tồn tại");
            }
            var roles = await _roleManager.GetRoleIdAsync(role);
            var roleVm = new RoleVm()
            {
                Id = id,
                Name = role.Name,
                Description = role.Description
            };
            return new ApiSuccessResult<RoleVm>(roleVm);
        }

        public async Task<ApiResult<PagedResult<RoleVm>>> GetRolesPaging(GetRolePagingRequest request)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword));
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new RoleVm()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Id = x.Id
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<RoleVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<RoleVm>>(pagedResult);
        }

        public async Task<ApiResult<bool>> Update(Guid roleId, RoleUpdateRequest request)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            role.Name = request.Name;
            role.Description = request.Description;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }
    }
}