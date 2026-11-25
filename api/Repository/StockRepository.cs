using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Interfaces;
using api.Models;
using api.Dtos.Stock;
using api.helpers;
namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationnDBContext _context;

        public StockRepository(ApplicationnDBContext context)
        {
            _context = context;
        }
        public async Task<List<Stock>> GetAllAsync(QueryObject queryObject)
        {
            var stocks = _context.Stocks.Include(s => s.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(queryObject.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(queryObject.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(queryObject.CompanyName));
            }
            if(!string.IsNullOrWhiteSpace(queryObject.SortBy))
            {
                if (queryObject.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = queryObject.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
                else if (queryObject.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = queryObject.IsDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                }
            }
            var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;

            return await stocks.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            var stock = await _context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(item => item.Id == id);   
            if (stock == null)
            {       
                return null;
            }

            return stock;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateStockRequest)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(item => item.Id == id);
            if (stock == null)
            {
                return null;
            }

            stock.Symbol = updateStockRequest.Symbol;
            stock.CompanyName = updateStockRequest.CompanyName;
            stock.Purchase = updateStockRequest.Purchase;
            stock.LastDiv = updateStockRequest.LastDiv;
            stock.Industry = updateStockRequest.Industry;
            stock.MarketCap = updateStockRequest.MarketCap;

            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> DeleteAsync(Stock stock)
        {
         var existingStock = await _context.Stocks.FirstOrDefaultAsync(item => item.Id == stock.Id);
            if (existingStock == null)
            {
                return null;
            }   
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public Task<bool> StockExistsAsync(int id)
        {
            return _context.Stocks.AnyAsync(item => item.Id == id);
        }
    }
}