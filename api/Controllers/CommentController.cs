using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        
        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Comment>>> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();
            var commentDtos = comments.Select(c => c.toCommentDto()).ToList();
            return Ok(commentDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Comment>>> GetById([FromRoute] int id)
        {
            var comments = await _commentRepository.GetByIdAsync(id);
            if (comments == null)
            {
                return NotFound();
            }
            var commentDtos = comments.Select(c => c.toCommentDto()).ToList();
            return Ok(commentDtos);
        }

        [HttpPost("{stockId}")]
        public async Task<ActionResult<Comment>> Create([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
        {
            if(!await _stockRepository.StockExistsAsync(stockId))
            {
                return NotFound();
            }

            var comment = commentDto.ToCommentFromCreateDto(stockId);

            var createdComment = await _commentRepository.CreateAsync(comment);

            return CreatedAtAction(nameof(GetById), new {id = createdComment.Id}, createdComment.toCommentDto());
        }


        


    }
}