using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.dtos.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        public string Symbol{get;set;} = string.Empty;
        [Required]
        public string CompanyName{get;set;} = string.Empty;
        [Required]
        public decimal Purchase{get;set;}
        [Required]
        [Range(0, 1000000000, ErrorMessage = "Shares must be a positive number.")]
        public decimal LastDiv{get;set;} 
        [Required]
        public string Industry{get;set;} = string.Empty;
        [Required]
        public long MarketCap{get;set;}
    }
}