using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Data.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        // Determine the Id of origin post in this topic
        public int OriginPostId { get; set; }

        public virtual IdentityUser User { get; set; }
        public virtual Forum Forum { get; set; }

        public virtual IEnumerable<Post> Posts { get; set; }
    }
}
