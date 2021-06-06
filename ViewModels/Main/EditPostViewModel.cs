using ForumDemo.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.ViewModels.Main
{
    public class EditPostViewModel
    {
        public Post Post { get; set; }

        [Display(Name = "Post contents")]
        [Required]
        [MinLength(2)]
        public string Contents { get; set; }
    }
}
