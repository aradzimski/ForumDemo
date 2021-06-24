using ForumDemo.Data;
using ForumDemo.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Repositories
{
    public class ForumRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ForumRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Forum>> GetAll()
        {
            var result = await _dbContext.Forums
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Forum>> GetAllWithTopics()
        {
            var result = await _dbContext.Forums
                .Include(forum => forum.Topics)
                .ToListAsync();

            return result;
        }

        public async Task<Forum> GetById(int id)
        {
            var result = await _dbContext.Forums
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();

            return result;
        }

        public async Task<Forum> GetByIdWithTopics(int id)
        {
            var result = await _dbContext.Forums
                .Where(x => x.Id == id)
                .Include(forum => forum.Topics)
                .SingleOrDefaultAsync();

            return result;
        }

        public async Task<string> GetTitleById(int id)
        {
            var result = await _dbContext.Forums
                .Where(x => x.Id == id)
                .Select(x => x.Title)
                .SingleOrDefaultAsync();

            return result;
        }

    }
}
