using crud_net.Data;
using crud_net.Dtos.Stock;
using crud_net.Mappers;
using crud_net.Models;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult GetAll()
    {
        var stocks = _context.Stocks.ToList()
            .Select(s => s.ToStockDto());
        return Ok(stocks);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var stock = _context.Stocks.Find(id);

        if (stock == null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
    {
        var stock = stockDto.ToStockFromCreateDto();
        
        _context.Stocks.Add(stock);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto()); 
        // return stockdto instead of stock
    }
    
    [HttpPut]
    [Route("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        // firstordefault is more flexible, we can set each comparison we want
        var stock = _context.Stocks.FirstOrDefault(x => x.Id == id); 
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

        _context.SaveChanges();

        return Ok(stock.ToStockDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var stock = _context.Stocks.FirstOrDefault(x => x.Id == id);
        if (stock == null)
        {
            return NotFound();
        }

        _context.Stocks.Remove(stock);
        _context.SaveChanges();

        return NoContent(); // when doing delete == success
    }
}