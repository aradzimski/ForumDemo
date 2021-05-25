using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Data.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
