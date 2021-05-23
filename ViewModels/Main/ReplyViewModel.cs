using ForumDemo.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.ViewModels.Main
{
    public class ReplyViewModel
    {
        public int TopicId { get; set; }
        public string TopicTitle { get; set; }

        [Display(Name = "Post contents")]
        [MinLength(2)]
        public string Contents { get; set; }
    }
}
