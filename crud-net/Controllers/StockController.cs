using crud_net.Data;
using crud_net.Dtos.Stock;
using crud_net.Mappers;
using crud_net.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace crud_net.Controllers;

[Route("api/[controller]")] // gets stockcontroller name and cuts controller
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDBContext _context;
    public StockController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _context.Stocks
            .Select(s => s.ToStockDto())
            .ToListAsync();
        
        return Ok(stocks);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        // changed find to firstordefault = find uploads full model from db
        var stockDto = await _context.Stocks
            .Where(x => x.Id == id)
            .Select(s => s.ToStockDto())
            .FirstOrDefaultAsync();

        if (stockDto == null)
        {
            return NotFound();
        }

        return Ok(stockDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
        var stock = stockDto.ToStockFromCreateDto();
        
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();
        
        var createdStockDto = stock.ToStockDto();

        return CreatedAtAction(nameof(GetById), new { id = stock.Id }, createdStockDto); 
        // return stockdto instead of stock
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        // FirstOrDefaultAsync allows custom filtering, unlike FindAsync which works only by primary key
        var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id); 
        if (stock == null)
        {
            return NotFound();
        }
        
        stock.Symbol = updateDto.Symbol;
        stock.CompanyName = updateDto.CompanyName;
        stock.Purchase = updateDto.Purchase;
        stock.MarketCap = updateDto.MarketCap;
        stock.Industry = updateDto.Industry;
        stock.LastDiv = updateDto.LastDiv;

        await _context.SaveChangesAsync();

        return Ok(stock.ToStockDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        if (stock == null)
        {
            return NotFound();
        }

        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();

        return NoContent(); // when doing delete == success
    }
}