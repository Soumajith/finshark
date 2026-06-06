using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.dtos.Comment;

namespace api.Interfaces
{
    public interface ICommentRepository
    {

        Task<List<Comment>> GetAllAsync();
        Task<List<Comment?>> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment comment);

        Task<Comment> deleteAsync(int id);
        Task<Comment> updateAsync(int id, UpdateCommentRequestDto updateDto);

    }
}