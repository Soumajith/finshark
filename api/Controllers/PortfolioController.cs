using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace api.Controllers
{
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioController(UserManager<AppUser> userManager, 
            IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var stocks = await _portfolioRepository.GetUserStock(appUser);

            if(stocks == null)
            {
                return NotFound("No stocks found");
            }

            return Ok(stocks);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepository.GetBySymbolAsync(symbol);

            if(stock == null) return NotFound("Stock not found!");

            // fetch user portfolio
            var userPortfolio = await _portfolioRepository.GetUserStock(appUser);
            // check if already exist
            if(userPortfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Stock already exist!!");

            // create a new portfolio model
            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id
            };

            if(portfolioModel == null) return BadRequest("Not able to create a new portfolio");
            
            await _portfolioRepository.CreatePortfolioAsync(portfolioModel);
            return Created();
        }


        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepository.GetUserStock(appUser);
          
            var filteredStock = userPortfolio.Where(s => s.Symbol == symbol);
            
            if(filteredStock.Count() == 0) return BadRequest("Stock doesn't exist!");
            
            var portfolio = await _portfolioRepository.DeletePortfolioAsync(appUser, symbol);

            return Ok(portfolio);

        }
    }
}