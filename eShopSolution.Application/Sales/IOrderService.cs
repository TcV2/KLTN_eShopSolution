using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Sales
{
    public interface IOrderService
    {
        Task<List<OrderVM>> GetAll(string languageId);

        Task<PagedResult<OrderVM>> GetOrderPaging(GetOrderPagingRequest request);
    }
}
