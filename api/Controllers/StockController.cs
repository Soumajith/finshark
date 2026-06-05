using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.dtos.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        // constructor to inject the database context
        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() // IActionResult allows us to return different types of responses (e.g., Ok, NotFound, etc.)
        {
            var stocks = await _stockRepository.GetAllAsync(); 
            var stockDtos = stocks.Select(s => s.ToStockDto()); // toList for deferred execution
            return Ok(stockDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) // model binding to get the id from the route
        {
            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound(); 
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            await _stockRepository.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto()); // return the created stock with its new ID
        }

        // update is used ith firstofDefault to check if the stock exists before updating
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _stockRepository.UpdateAsync(id, updateDto);
            if (stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDto());
            
        }

        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
           var stockModel = await _stockRepository.DeleteAsync(id);

           if(stockModel == null)
           {
                return NotFound();
           }
           return NoContent(); // 204 success code
        }


    }
}