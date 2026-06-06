using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comments = await _commentRepository.GetAllAsync();
            var commentDtos = comments.Select(c => c.toCommentDto()).ToList();
            return Ok(commentDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<List<Comment>>> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comments = await _commentRepository.GetByIdAsync(id);
            if (comments == null)
            {
                return NotFound();
            }
            var commentDtos = comments.Select(c => c.toCommentDto()).ToList();
            return Ok(commentDtos);
        }

        [HttpPost("{stockId:int}")]
        public async Task<ActionResult<Comment>> Create([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!await _stockRepository.StockExistsAsync(stockId))
            {
                return NotFound();
            }

            var comment = commentDto.ToCommentFromCreateDto(stockId);

            var createdComment = await _commentRepository.CreateAsync(comment);

            return CreatedAtAction(nameof(GetById), new {id = createdComment.Id}, createdComment.toCommentDto());
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingComment = await _commentRepository.deleteAsync(id);
            if (existingComment == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingComment = await _commentRepository.updateAsync(id, updateDto);
            if (existingComment == null)
            {
                return NotFound();
            }
            return Ok(existingComment.toCommentDto());  
        }


    }
}