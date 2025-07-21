using crud_net.Data;
using crud_net.Dtos.Stock;
using crud_net.Interfaces;
using crud_net.Mappers;
using crud_net.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace crud_net.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDBContext _context;
    
    public StockRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<List<Stock>> GetAllAsync()
    {
        return await _context.Stocks.Include(c => c.Comments).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(x=> x.Id == id); // returns null if not found already
    }

    public async Task<Stock> CreateAsync(Stock stock)
    {
        await _context.Stocks.AddAsync(stock);
        await _context.SaveChangesAsync();
        return stock;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
    {
        var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        if (existingStock == null)
        {
            return null;
        }
        
        existingStock.Symbol = stockDto.Symbol;
        existingStock.CompanyName = stockDto.CompanyName;
        existingStock.Purchase = stockDto.Purchase;
        existingStock.MarketCap = stockDto.MarketCap;
        existingStock.Industry = stockDto.Industry;
        existingStock.LastDiv = stockDto.LastDiv;
        
        await _context.SaveChangesAsync();
        
        return existingStock;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stock = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);

        if (stock == null)
        {
            return null;
        }
        
        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();
        
        return stock;
    }

    public async Task<bool> StockExists(int id)
    {
        return await _context.Stocks.AnyAsync(x => x.Id == id);
    }
}