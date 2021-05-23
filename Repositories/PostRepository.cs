using ForumDemo.Data;
using ForumDemo.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Repositories
{
    public class PostRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PostRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            var result = await _dbContext.Posts
                .ToListAsync();

            return result;
        }

        public async Task Create(Post post)
        {
            post.Created = DateTime.Now;
            post.Updated = DateTime.Now;

            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();
        }
    }
}
