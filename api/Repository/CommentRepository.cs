using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Interfaces;
using api.Models;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository

    {
        private readonly ApplicationnDBContext _context;
        public CommentRepository(ApplicationnDBContext context)
        {
            _context = context;
        }
        public Task<Comment> CreateAsync(Comment comment)
        {
            throw new NotImplementedException();
        }

        public Task<Comment?> DeleteAsync(Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }
        

        public Task<Comment?> UpdateAsync(int id, Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}