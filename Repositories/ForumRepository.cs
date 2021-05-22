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

        public async Task<IEnumerable<Forum>> GetAllAsync()
        {
            var result = await _dbContext.Forums
                .Include(forum => forum.Topics).ToListAsync();

            return result;
        }
    }
}
