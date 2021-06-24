using ForumDemo.Data;
using ForumDemo.Data.Models;
using ForumDemo.Tools;
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
                .Include(topic => topic.Forum)
                .SingleOrDefaultAsync();

            return result;
        }

        public async Task<PaginatedList<Post>> GetByIdWithPosts(int id, int page, int pageSize)
        {
            var result = await PaginatedList<Post>.CreateAsync(_dbContext.Posts
                .Where(x => x.Topic.Id == id)
                .Include(x => x.User), page, pageSize);

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

        public async Task<int> Create(Topic topic)
        {
            await _dbContext.Topics.AddAsync(topic);
            await _dbContext.SaveChangesAsync();

            // Return Id of the topic we have just created for post attaching purpose
            return topic.Id;
        }

    }
}
