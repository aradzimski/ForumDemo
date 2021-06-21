using ForumDemo.Data.Models;
using ForumDemo.Repositories;
using ForumDemo.ViewModels.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Controllers
{
    public class ForumController : Controller
    {
        private readonly ILogger<ForumController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly UserRepository _userRepository;
        private readonly ForumRepository _forumRepository;
        private readonly TopicRepository _topicRepository;

        public ForumController(ILogger<ForumController> logger, UserManager<User> userManager, UserRepository userRepository, ForumRepository forumRepository, TopicRepository topicRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _userRepository = userRepository;
            _forumRepository = forumRepository;
            _topicRepository = topicRepository;
        }
        [Route("forum/{id}")]
        [Breadcrumb("ViewData.Title")]
        public async Task<IActionResult> Index(int id)
        {
            ForumViewModel vm = new ForumViewModel()
            {
                Forum = await _forumRepository.GetByIdWithTopics(id)
            };

            return View(vm);
        }

        [Route("forum/{id}/add")]
        [Authorize]
        public async Task<IActionResult> Add(int id)
        {
            CreateTopicViewModel vm = new CreateTopicViewModel()
            {
                Forum = await _forumRepository.GetById(id)
            };

            // Manually set breadcrumb nodes
            var childNode1 = new MvcBreadcrumbNode("Index", "Forum", vm.Forum.Title)
            {
                RouteValues = new { id = vm.Forum.Id }
            };
            var childNode2 = new MvcBreadcrumbNode("Add", "Forum", "Creating topic")
            {
                RouteValues = new { id = vm.Forum.Id },
                OverwriteTitleOnExactMatch = true,
                Parent = childNode1
            };

            ViewData["BreadcrumbNode"] = childNode2;

            return View(vm);
        }

        [Route("forum/{id}/add")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateTopicViewModel vm)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            Topic topic = new Topic()
            {
                Title = vm.Title,
                Description = vm.Description,
                Forum = await _forumRepository.GetById(vm.Forum.Id),
                Posts = new List<Post>
                {
                    new Post {
                        Contents = vm.Contents,
                        User = user,
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    }
                },
                User = user,
                Created = DateTime.Now,
                Updated = DateTime.Now
            };

            int topic_id = await _topicRepository.Create(topic);
            await _userRepository.CountPost(user.Id);

            return RedirectToAction("Index", "Topic", new { id = topic_id });
        }
    }
}
