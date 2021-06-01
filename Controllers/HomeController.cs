using ForumDemo.Data.Models;
using ForumDemo.Repositories;
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

        [DefaultBreadcrumb("ForumDemo")]
        public async Task<IActionResult> Index()
        {
            ForumListModel vm = new ForumListModel() {
                ForumList = await _forumRepository.GetAllWithTopics()
            };

            return View(vm);
        }

        [Breadcrumb("ViewData.Title")]
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

            // Manually set breadcrumb nodes
            var childNode1 = new MvcBreadcrumbNode("Forum", "Home", vm.Topic.Forum.Title)
            {
                RouteValues = new { id = vm.Topic.Forum.Id}
            };
            var childNode2 = new MvcBreadcrumbNode("Topic", "Home", "ViewData.Title")
            {
                OverwriteTitleOnExactMatch = true,
                Parent = childNode1
            };

            ViewData["BreadcrumbNode"] = childNode2;

            return View(vm);
        }

        [Authorize]
        public async Task<IActionResult> CreateTopic(int id)
        {
            CreateTopicViewModel vm = new CreateTopicViewModel()
            {
                Forum = await _forumRepository.GetById(id)
            };

            // Manually set breadcrumb nodes
            var childNode1 = new MvcBreadcrumbNode("Forum", "Home", vm.Forum.Title)
            {
                RouteValues = new { id = vm.Forum.Id }
            };
            var childNode2 = new MvcBreadcrumbNode("CreateTopic", "Home", "Creating topic")
            {
                RouteValues = new { id = vm.Forum.Id },
                OverwriteTitleOnExactMatch = true,
                Parent = childNode1
            };

            ViewData["BreadcrumbNode"] = childNode2;

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTopic(CreateTopicViewModel vm)
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

            return RedirectToAction("Topic", new { id = topic_id });
        }

        [Authorize]
        public async Task<IActionResult> Reply(int id)
        {
            ReplyViewModel vm = new ReplyViewModel()
            {
                Topic =  await _topicRepository.GetById(id)
            };

            // Manually set breadcrumb nodes
            var childNode1 = new MvcBreadcrumbNode("Forum", "Home", vm.Topic.Forum.Title)
            {
                RouteValues = new { id = vm.Topic.Forum.Id }
            };
            var childNode2 = new MvcBreadcrumbNode("Topic", "Home", vm.Topic.Title)
            {
                RouteValues = new { id = vm.Topic.Id },
                OverwriteTitleOnExactMatch = true,
                Parent = childNode1
            };
            var childNode3 = new MvcBreadcrumbNode("Reply", "Home", "Replying")
            {
                RouteValues = new { id = vm.Topic.Id },
                OverwriteTitleOnExactMatch = true,
                Parent = childNode2
            };

            ViewData["BreadcrumbNode"] = childNode3;

            return View(vm);
        }

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

            return RedirectToAction("Topic", new { id = vm.Topic.Id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
