using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public class OrderApiClient : BaseApiClient, IOrderApiClient
    {
        public OrderApiClient(IHttpClientFactory httpClientFactory, 
            IHttpContextAccessor httpContextAccessor, 
            IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<List<OrderVM>> GetAll(string languageId)
        {
            return await GetListAsync<OrderVM>("/api/orders?languageId=" + languageId);
        }

        public async Task<PagedResult<OrderVM>> GetAllPaging(GetOrderPagingRequest request)
        {
            var data = await GetAsync<PagedResult<OrderVM>>(
                $"/api/orders/paging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}&languageId={request.LanguageId}");

            return data;
        }
    }
}
