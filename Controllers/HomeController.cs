using ForumDemo.Data.Models;
using ForumDemo.Repositories;
using ForumDemo.ViewModels;
using ForumDemo.ViewModels.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        private readonly ForumRepository _forumRepository;
        private readonly TopicRepository _topicRepository;
        private readonly PostRepository _postRepository;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, ForumRepository forumRepository, TopicRepository topicRepository, PostRepository postRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _forumRepository = forumRepository;
            _topicRepository = topicRepository;
            _postRepository = postRepository;
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

        [Authorize]
        public async Task<IActionResult> Reply(int id)
        {
            ReplyViewModel vm = new ReplyViewModel()
            {
                TopicId = id,
                TopicTitle = await _topicRepository.GetTitleById(id)
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Reply(ReplyViewModel vm)
        {
            Post post = new Post()
            {
                Contents = vm.Contents,
                Topic = await _topicRepository.GetById(vm.TopicId),
                User = await _userManager.GetUserAsync(HttpContext.User)
            };

            await _postRepository.Create(post);

            return RedirectToAction("Topic", new { id = vm.TopicId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
