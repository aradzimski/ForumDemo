using ForumDemo.Data.Models;
using ForumDemo.Repositories;
using ForumDemo.ViewModels;
using ForumDemo.ViewModels.Main;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ForumRepository _forumRepository;
        private readonly TopicRepository _topicRepository;

        public HomeController(ILogger<HomeController> logger, ForumRepository forumRepository, TopicRepository topicRepository)
        {
            _logger = logger;
            _forumRepository = forumRepository;
            _topicRepository = topicRepository;
        }

        public async Task<IActionResult> Index()
        {
            ForumListModel vm = new ForumListModel() {
                ForumList = await _forumRepository.GetAllWithTopics()
            };

            return View(vm);
        }

        public async Task<IActionResult> Forum(int id)
        {
            ForumViewModel vm = new ForumViewModel()
            {
                Forum = await _forumRepository.GetByIdWithTopics(id)
            };

            return View(vm);
        }

        public async Task<IActionResult> Topic(int id)
        {
            TopicViewModel vm = new TopicViewModel()
            {
                Topic = await _topicRepository.GetByIdWithPosts(id)
            };

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
