using crud_net.Data;
using crud_net.Dtos.Stock;
using crud_net.Interfaces;
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
    private readonly IStockRepository  _stockRepository;
    
    public StockController(ApplicationDBContext context, IStockRepository stockRepository)
    {
        _stockRepository =  stockRepository;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _stockRepository.GetAllAsync();
        
        var stockDtos = stocks.Select(s => s.ToStockDto());
        
        return Ok(stockDtos);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        // changed find to firstordefault = find uploads full model from db
        var stock = await _stockRepository.GetByIdAsync(id);
        if (stock == null)
        {
            return NotFound();
        }
        
        var stockDto = stock.ToStockDto();
        
        return Ok(stockDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
        var stock = stockDto.ToStockFromCreateDto();
        await _stockRepository.CreateAsync(stock);
        return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto()); // return stockdto instead of stock
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        // FirstOrDefaultAsync allows custom filtering, unlike FindAsync which works only by primary key
        var stock = await _stockRepository.UpdateAsync(id, updateDto); 
        if (stock == null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stock = await _stockRepository.DeleteAsync(id);
        if (stock == null)
        {
            return NotFound();
        }

        return NoContent(); // when doing delete, nocontent == success
    }
}