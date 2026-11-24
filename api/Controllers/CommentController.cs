using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController :ControllerBase
    {
        private readonly api.Interfaces.ICommentRepository _commentRepository;
        private readonly api.Interfaces.IStockRepository _stockRepository;
        public CommentController(api.Interfaces.ICommentRepository commentRepository, api.Interfaces.IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentRepository.GetAllAsync();
            var commentDtos = comments.Select(c => c.ToCommentDto());
            
            return Ok(commentDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }
        [HttpPost("{stockId}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] Dtos.Comment.CreateCommentRequestDto createCommentRequest)
        {
            var stockExists = await _stockRepository.StockExistsAsync(stockId);
            if (!stockExists)
            {
                return BadRequest($"Stock with ID {stockId} not found.");
            }
            

            var comment = createCommentRequest.ToCommentDtoFromCreateRequest(stockId);
            await _commentRepository.CreateAsync(comment);
            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment.ToCommentDto());
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] Dtos.Comment.UpdateCommentRequestDto updateCommentRequest)
        {           
            var existingComment = await _commentRepository.UpdateAsync(id, updateCommentRequest.ToCommentDtoFromUpdateRequest());
            if (existingComment == null)
            {
                return NotFound("COMMENT NOT FOUND");
            }

           return Ok(existingComment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound("COMMENT NOT FOUND");
            }

            await _commentRepository.DeleteAsync(id);
            return NoContent();
        }

    }

}