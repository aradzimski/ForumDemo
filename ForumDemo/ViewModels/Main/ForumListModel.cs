using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDemo.Data.Models;

namespace ForumDemo.ViewModels.Main
{
    public class ForumListModel
    {
        public IEnumerable<Forum> ForumList { get; set; }
    }
}
