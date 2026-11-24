using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Interfaces;
using api.Models;
using api.Dtos.Stock;
namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationnDBContext _context;

        public StockRepository(ApplicationnDBContext context)
        {
            _context = context;
        }
        public Task<List<Stock>> GetAllAsync()
        {
            return  _context.Stocks.Include(s => s.Comments).ToListAsync();
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