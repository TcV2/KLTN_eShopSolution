using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Sales;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        
        public async Task<int> Checkout(Guid id, CheckoutViewModel request)
        {
            
            var order = new Order()
            {
                UserId = id,
                OrderDate = DateTime.Now,
                ShipAddress = request.CheckoutModel.Address,
                ShipName = request.CheckoutModel.Name,
                ShipEmail = request.CheckoutModel.Email,
                ShipPhoneNumber = request.CheckoutModel.PhoneNumber
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var odDetails = new List<OrderDetail>();
            foreach (var item in request.CartItems)
            {
                odDetails.Add(new OrderDetail()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    TotalPrice = item.Price * item.Quantity,
                    OrderId = order.Id
                });
            }
            foreach (var item in odDetails)
            {
                await _context.OrderDetails.AddAsync(item);
            }
            await _context.SaveChangesAsync();
            return order.Id;
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

        public async Task<int> Update(OrderConfirmRequest request)
        {
            var order = await _context.Orders.FindAsync(request.Id);

            order.Status = OrderStatus.Confirmed;

            return await _context.SaveChangesAsync();
        }
    }
}
