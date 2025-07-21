using crud_net.Dtos.Stock;
using crud_net.Models;

namespace crud_net.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync();
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock> CreateAsync(Stock stock);
    Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
    Task<Stock?> DeleteAsync(int id);
    Task<bool> StockExists(int id);
    Task<int> DeleteAllAsync();
}