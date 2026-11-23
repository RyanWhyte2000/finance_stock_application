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
        public CommentController(api.Interfaces.ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;             
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentRepository.GetAllAsync();
            var commentDtos = comments.Select(c => c.ToCommentDto());
            
            return Ok(commentDtos);
        }
    }

}