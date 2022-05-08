using eShopSolution.ViewModels.Catalog.Warehouses;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Warehouses
{
    public interface IWarehouseService
    {
        Task<PagedResult<WarehouseVM>> GetWarehousePaging(GetWarehousePagingRequest request);

        Task<WarehouseVM> GetById(string languageId, int id);

        Task<int> UpdatePrice(UpdatePriceRequest request);

        Task<int> UpdateStock(UpdateStockRequest request);
    }
}
