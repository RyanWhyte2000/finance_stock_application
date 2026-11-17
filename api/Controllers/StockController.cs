using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock") ]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationnDBContext _context;
        public StockController(ApplicationnDBContext context)
        {
            
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var stocks  = _context.Stocks.ToList();
            var stockDtos = stocks.Select(s => s.ToStockDto());
            return Ok(stockDtos);
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
    public IActionResult Create([FromBody] CreateStockRequestDtos createStockRequest)
    {
        var stock = createStockRequest.ToStockModel();
        _context.Stocks.Add(stock);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
    }
    }
}