using ForumDemo.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.ViewModels.Main
{
    public class CreateTopicViewModel
    {
        public int ForumId { get; set; }
        public string ForumTitle { get; set; }

        [Display(Name="Topic title")]
        [Required]
        [MinLength(2)]
        public string Title { get; set; }

        [Display(Name = "Topic description")]
        public string Description { get; set; }

        [Display(Name = "Post contents")]
        [Required]
        [MinLength(2)]
        public string Contents { get; set; }
    }
}
