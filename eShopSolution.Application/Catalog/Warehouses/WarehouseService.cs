using eShopSolution.Data.EF;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.Warehouses;
using eShopSolution.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Warehouses
{
    public class WarehouseService : IWarehouseService
    {
        private readonly EShopDbContext _context;

        public WarehouseService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<WarehouseVM> GetById(string languageId, int id)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        where pt.LanguageId == languageId && p.Id == id
                        select new { p, pt };
            return await query.Select(x => new WarehouseVM()
            {
                Id = x.p.Id,
                Name = x.pt.Name,
                Price = x.p.Price,
                OriginalPrice = x.p.OriginalPrice,
                Stock = x.p.Stock
            }).FirstOrDefaultAsync();
        }

        public async Task<PagedResult<WarehouseVM>> GetWarehousePaging(GetWarehousePagingRequest request)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId into ppt
                        from pt in ppt.DefaultIfEmpty()
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt };
            //2. filter - lọc theo keyword
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));

            //3. Paging - phân trang
            int totalRow = await query.CountAsync();


            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new WarehouseVM()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    Price = x.p.Price,
                    OriginalPrice = x.p.OriginalPrice,
                    Stock = x.p.Stock
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<WarehouseVM>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<int> UpdatePrice(UpdatePriceRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);            

            if (product == null)
                throw new EShopException($"Cannot find a product with id: {request.Id}");

            product.Price = request.NewPrice;
            product.OriginalPrice = request.NewOriginalPrice;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateStock(UpdateStockRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);

            if (product == null)
                throw new EShopException($"Cannot find a product with id: {request.Id}");

            product.Stock = product.Stock + request.Quantity;

            return await _context.SaveChangesAsync();
        }
    }
}
