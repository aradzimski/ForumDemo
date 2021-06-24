using ForumDemo.Data.Models;
using ForumDemo.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.ViewModels.Main
{
    public class TopicViewModel
    {
        public Topic Topic { get; set; }
        public PaginatedList<Post> Posts {get; set;}
    }
}
