using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RoleVm>>> GetAll();

        Task<ApiResult<PagedResult<RoleVm>>> GetRolePagings(GetRolePagingRequest request);

        Task<ApiResult<RoleVm>> GetById(Guid id);

        Task<ApiResult<bool>> CreateRole(RoleCreateRequest request);

        Task<ApiResult<bool>> UpdateRole(Guid id, RoleUpdateRequest request);

        Task<ApiResult<bool>> DeleteRole(Guid id);
    }
}