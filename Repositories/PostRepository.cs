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

        public PostRepository() { }
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

        public virtual async Task<Post> GetById(int id)
        {
            var result = await _dbContext.Posts
                .Where(x => x.Id == id)
                .Include(x => x.Topic)
                .ThenInclude(topic => topic.Forum)
                .SingleOrDefaultAsync();

            return result;
        }

        public async Task Create(Post post)
        {
            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Post post)
        {
            _dbContext.Posts.Update(post);
            await _dbContext.SaveChangesAsync();
        }
    }
}
