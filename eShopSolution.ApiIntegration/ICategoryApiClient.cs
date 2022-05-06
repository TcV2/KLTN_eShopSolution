﻿using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryVm>> GetAll(string languageId);

        Task<CategoryVm> GetById(string languageId, int id);

        Task<PagedResult<CategoryVm>> GetAllPaging(GetCategoryPagingRequest request);

        Task<ApiResult<bool>> CreateCategory(CategoryCreateRequest request);

        Task<ApiResult<bool>> UpdateCategory(CategoryUpdateRequest request);

        Task<ApiResult<bool>> DeleteCategory(int categoryId);
    }
}