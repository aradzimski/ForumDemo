using ForumDemo.Data;
using ForumDemo.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CountPost(string id)
        {

            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            if(user != null)
            {
                user.PostCounter++;

                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
