using eShopSolution.ViewModels.Catalog.Warehouses;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public interface IWarehouseApiClient
    {
        Task<PagedResult<WarehouseVM>> GetAllPaging(GetWarehousePagingRequest request);

        Task<WarehouseVM> GetById(string languageId, int id);

        Task<bool> UpdatePrice(UpdatePriceRequest request);

        Task<bool> UpdateStock(UpdateStockRequest request);
    }
}
