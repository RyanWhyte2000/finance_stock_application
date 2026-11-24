using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
         public static CommentDto ToCommentDto(this Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                CreatedOn = commentModel.CreatedOn,
                Content = commentModel.Content,
                StockId = commentModel.StockId
            };
        }

         public static Comment ToCommentDtoFromCreateRequest(this Dtos.Comment.CreateCommentRequestDto createCommentRequest,  int stockId)
        {
            return new Comment
            {
                Title = createCommentRequest.Title,
                Content = createCommentRequest.Content,
                StockId = stockId
            };
        }
    }
}
               