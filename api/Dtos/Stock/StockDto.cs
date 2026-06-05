using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.dtos.Comment;
using api.Models;

namespace api.dtos.Stock
{
    // the DTO (Data Transfer Object) for Stock, which is used to transfer data between layers, 
            // you can select which data should be exposed.
    public class StockDto
    {
        public int Id{get;set;}
        public string Symbol{get;set;} = string.Empty;
        public string CompanyName{get;set;} = string.Empty;

        public decimal Purchase{get;set;}
        public decimal LastDiv{get;set;} 
        public string Industry{get;set;} = string.Empty;
        public long MarketCap{get;set;}

        public List<CommentDto> Comments { get; set; }
    }
}