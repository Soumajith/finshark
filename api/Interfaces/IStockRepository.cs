using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.dtos.Stock;
using api.Helpers;
using api.Models;


namespace api.Interfaces
{
    public interface IStockRepository 
    {
       
        public Task<List<Stock>> GetAllAsync(QueryObject query);
        public Task<Stock?> GetByIdAsync(int id); // ? because it can null in first and default if the stock is not found
        public Task<Stock?> GetBySymbolAsync(string symbol);
        public Task<Stock> CreateAsync(Stock stock);
        public Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto);
        public Task<Stock> DeleteAsync(int id);
        public Task<bool> StockExistsAsync(int id);

    }
}