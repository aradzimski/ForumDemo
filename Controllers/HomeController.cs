using ForumDemo.Data.Models;
using ForumDemo.Repositories;
using ForumDemo.Tools;
using ForumDemo.ViewModels;
using ForumDemo.ViewModels.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;
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
        private readonly UserRepository _userRepository;
        private readonly ForumRepository _forumRepository;
        private readonly TopicRepository _topicRepository;
        private readonly PostRepository _postRepository;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager,UserRepository userRepository, ForumRepository forumRepository, TopicRepository topicRepository, PostRepository postRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _userRepository = userRepository;
            _forumRepository = forumRepository;
            _topicRepository = topicRepository;
            _postRepository = postRepository;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [DefaultBreadcrumb("ForumDemo")]
        public async Task<IActionResult> Index()
        {
            ForumListModel vm = new ForumListModel() {
                ForumList = await _forumRepository.GetAllWithTopics()
            };

            return View(vm);
        }

        [Authorize]
        public async Task<IActionResult> EditPost(int id)
        {
            Post post = await _postRepository.GetById(id);

            EditPostViewModel vm = new EditPostViewModel()
            {
                Post = await _postRepository.GetById(id),
                Contents = post.Contents
            };

            // Manually set breadcrumb nodes
            var childNode1 = new MvcBreadcrumbNode("Forum", "Home", vm.Post.Topic.Forum.Title)
            {
                RouteValues = new { id = vm.Post.Topic.Forum.Id }
            };
            var childNode2 = new MvcBreadcrumbNode("Topic", "Home", vm.Post.Topic.Title)
            {
                RouteValues = new { id = vm.Post.Topic.Id },
                OverwriteTitleOnExactMatch = true,
                Parent = childNode1
            };
            var childNode3 = new MvcBreadcrumbNode("Editing post", "Home", "Editing post")
            {
                Parent = childNode2
            };

            ViewData["BreadcrumbNode"] = childNode3;

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditPost(EditPostViewModel vm)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            Post post = await _postRepository.GetById(vm.Post.Id);
            post.Contents = vm.Contents;

            await _postRepository.Update(post);

            return RedirectToAction("Topic", new { id = vm.Post.Topic.Id });
        }

    }
}
