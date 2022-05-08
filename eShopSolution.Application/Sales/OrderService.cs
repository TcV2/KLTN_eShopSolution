using eShopSolution.Data.EF;
using eShopSolution.Data.Enums;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Sales;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Sales
{
    public class OrderService : IOrderService
    {
        private readonly EShopDbContext _context;

        public OrderService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderVM>> GetAll(string languageId)
        {
            var query = from od in _context.Orders
                        join odd in _context.OrderDetails on od.Id equals odd.OrderId
                        join pt in _context.ProductTranslations on odd.ProductId equals pt.ProductId
                        where pt.LanguageId == languageId
                        select new { od, odd, pt };
            return await query.Select(x => new OrderVM()
            {
                Id = x.od.Id,
                OrderDate = x.od.OrderDate,
                ProductName = x.pt.Name,
                Quantity = x.odd.Quantity,
                TotalPrice = x.odd.TotalPrice,
                ShipName = x.od.ShipName,
                ShipAddress = x.od.ShipAddress,
                ShipPhoneNumber = x.od.ShipPhoneNumber,
                Status = x.od.Status
            }).ToListAsync();
        }

        public async Task<PagedResult<OrderVM>> GetOrderPaging(GetOrderPagingRequest request)
        {
            //1. Select join
            var query = from od in _context.Orders
                        join odd in _context.OrderDetails on od.Id equals odd.OrderId
                        join pt in _context.ProductTranslations on odd.ProductId equals pt.ProductId
                        where pt.LanguageId == request.LanguageId
                        select new { od, odd, pt };
            //2. filter - lọc theo keyword
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));

            //3. Paging - phân trang
            int totalRow = await query.CountAsync();


            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OrderVM()
                {
                    Id = x.od.Id,
                    OrderDate = x.od.OrderDate,
                    ProductName = x.pt.Name,
                    Quantity = x.odd.Quantity,
                    TotalPrice = x.odd.TotalPrice,
                    ShipName = x.od.ShipName,
                    ShipAddress = x.od.ShipAddress,
                    ShipPhoneNumber = x.od.ShipPhoneNumber,
                    Status = x.od.Status
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<OrderVM>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }
    }
}
