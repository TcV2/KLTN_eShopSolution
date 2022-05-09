using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Sales;
using eShopSolution.WebApp.Models;
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

        Task<int> Update(OrderConfirmRequest request);

        Task<int> Checkout(Guid id,CheckoutViewModel request);

        //Task<Guid> GetUserId(string username);
    }
}
