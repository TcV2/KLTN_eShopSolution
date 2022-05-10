using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public interface IOrderApiClient
    {
        Task<List<OrderVM>> GetAll(string languageId);

        Task<PagedResult<OrderVM>> GetAllPaging(GetOrderPagingRequest request);

        Task<bool> UpdateOrder(OrderConfirmRequest request);

        Task<bool> Checkout(CheckoutRequest request);
    }
}