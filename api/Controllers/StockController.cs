using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock") ]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationnDBContext _context;
        private readonly IStockRepository _stockRepository;
        public StockController(ApplicationnDBContext context, IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
            
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stocks = await _stockRepository.GetAllAsync();
            var stockDtos = stocks.Select(s => s.ToStockDto());
            return Ok(stockDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null)
            {       
                return NotFound();
            }
            return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDtos createStockRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var stock = createStockRequest.ToStockModel();
        await _stockRepository.CreateAsync(stock);
        return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var stock = await _stockRepository.UpdateAsync(id, updateStockRequest);
        if (stock == null)
        {
            return NotFound();
        }
        stock.Symbol = updateStockRequest.Symbol;
        stock.CompanyName = updateStockRequest.CompanyName;
        stock.Purchase = updateStockRequest.Purchase;
        stock.LastDiv = updateStockRequest.LastDiv;
        stock.Industry = updateStockRequest.Industry;
        stock.MarketCap = updateStockRequest.MarketCap;
        await _context.SaveChangesAsync();
        return Ok(stock.ToStockDto());
    }
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var stock = await _stockRepository.GetByIdAsync(id);
        if (stock == null)
        {
            return NotFound();  
        }
        await _stockRepository.DeleteAsync(stock);
        return NoContent();
  }
    }
}