using ForumDemo.Data.Models;
using ForumDemo.Repositories;
using ForumDemo.ViewModels.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Controllers
{
    public class TopicController : Controller
    {
        private readonly ILogger<TopicController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly UserRepository _userRepository;
        private readonly TopicRepository _topicRepository;
        private readonly PostRepository _postRepository;

        public TopicController(ILogger<TopicController> logger, UserManager<User> userManager, UserRepository userRepository, TopicRepository topicRepository, PostRepository postRepostiory)
        {
            _logger = logger;
            _userManager = userManager;
            _userRepository = userRepository;
            _topicRepository = topicRepository;
            _postRepository = postRepostiory;
        }
        [Route("topic/{id}")]
        [Route("topic/{id}/page/{page}")]
        public async Task<IActionResult> Index(int id, int? page)
        {
            // Pagination

            int pageSize = 20;

            TopicViewModel vm = new TopicViewModel()
            {
                Topic = await _topicRepository.GetById(id),
                Posts = await _topicRepository.GetByIdWithPosts(id, page ?? 1, pageSize)
            };

            // Manually set breadcrumb nodes
            var childNode1 = new MvcBreadcrumbNode("Index", "Forum", vm.Topic.Forum.Title)
            {
                RouteValues = new { id = vm.Topic.Forum.Id }
            };
            var childNode2 = new MvcBreadcrumbNode("Index", "Topic", "ViewData.Title")
            {
                OverwriteTitleOnExactMatch = true,
                Parent = childNode1
            };

            ViewData["BreadcrumbNode"] = childNode2;

            return View(vm);
        }

        [Route("topic/{id}/reply")]
        [Authorize]
        public async Task<IActionResult> Reply(int id)
        {
            ReplyViewModel vm = new ReplyViewModel()
            {
                Topic = await _topicRepository.GetById(id)
            };

            // Manually set breadcrumb nodes
            var childNode1 = new MvcBreadcrumbNode("Index", "Forum", vm.Topic.Forum.Title)
            {
                RouteValues = new { id = vm.Topic.Forum.Id }
            };
            var childNode2 = new MvcBreadcrumbNode("Index", "Topic", vm.Topic.Title)
            {
                RouteValues = new { id = vm.Topic.Id },
                OverwriteTitleOnExactMatch = true,
                Parent = childNode1
            };
            var childNode3 = new MvcBreadcrumbNode("Reply", "Topic", "Reply")
            {
                RouteValues = new { id = vm.Topic.Id },
                OverwriteTitleOnExactMatch = true,
                Parent = childNode2
            };

            ViewData["BreadcrumbNode"] = childNode3;

            return View(vm);
        }

        [Route("topic/{id}/reply")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Reply(ReplyViewModel vm)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            Post post = new Post()
            {
                Contents = vm.Contents,
                Topic = await _topicRepository.GetById(vm.Topic.Id),
                User = user,
                Created = DateTime.Now,
                Updated = DateTime.Now
            };

            await _postRepository.Create(post);
            await _userRepository.CountPost(user.Id);

            return RedirectToAction("Index", "Topic", new { id = vm.Topic.Id });
        }
    }
}
