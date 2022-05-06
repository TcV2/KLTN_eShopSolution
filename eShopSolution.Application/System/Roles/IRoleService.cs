using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Roles
{
    public interface IRoleService
    {
        Task<List<RoleVm>> GetAll();

        Task<ApiResult<PagedResult<RoleVm>>> GetRolesPaging(GetRolePagingRequest request);

        Task<ApiResult<RoleVm>> GetById(Guid id);

        Task<ApiResult<bool>> Create(RoleCreateRequest request);

        Task<ApiResult<bool>> Update(Guid roleId, RoleUpdateRequest request);

        Task<ApiResult<bool>> Delete(Guid roleId);
    }
}