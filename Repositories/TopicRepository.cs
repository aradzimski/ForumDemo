using ForumDemo.Data;
using ForumDemo.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Repositories
{
    public class TopicRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TopicRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Topic>> GetAll()
        {
            var result = await _dbContext.Topics
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Topic>> GetAllWithPosts()
        {
            var result = await _dbContext.Topics
                .Include(topic => topic.Posts)
                .ThenInclude(post => post.User)
                .ToListAsync();

            return result;
        }

        public async Task<Topic> GetById(int id)
        {
            var result = await _dbContext.Topics
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();

            return result;
        }

        public async Task<Topic> GetByIdWithPosts(int id)
        {
            var result = await _dbContext.Topics
                .Where(x => x.Id == id)
                .Include(topic => topic.Posts)
                .ThenInclude(post => post.User)
                .SingleOrDefaultAsync();

            return result;
        }

        public async Task<string> GetTitleById(int id)
        {
            var result = await _dbContext.Topics
                .Where(x => x.Id == id)
                .Select(x => x.Title)
                .SingleOrDefaultAsync();

            return result;
        }

    }
}
