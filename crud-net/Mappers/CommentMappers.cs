using crud_net.Dtos.Comment;
using crud_net.Models;

namespace crud_net.Mappers;

public static class CommentMappers
{
    public static CommentDto ToCommentDto(this Comment comment)
    {
        return new CommentDto
        {
            Id = comment.Id,
            Title = comment.Title,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            StockId = comment.StockId
        };
        
    }

    public static Comment ToCommentFromCreate(this CreateCommentRequestDto commentDto, int stockId)
    {
        return new Comment
        {
            Title = commentDto.Title,
            Content = commentDto.Content,
            StockId = stockId
        };
    }
}