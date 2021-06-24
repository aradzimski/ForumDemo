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
    public class PostController : Controller
    {
        private readonly ILogger<PostController> _logger;
        private readonly PostRepository _postRepository;
        public PostController(ILogger<PostController> logger, UserManager<User> userManager, PostRepository postRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _postRepository = postRepository;
        }

        [Route("post/{id}/edit")]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            Post post = await _postRepository.GetById(id);

            EditPostViewModel vm = new EditPostViewModel()
            {
                Post = await _postRepository.GetById(id),
                Contents = post.Contents
            };

            // Manually set breadcrumb nodes
            var childNode1 = new MvcBreadcrumbNode("Index", "Forum", vm.Post.Topic.Forum.Title)
            {
                RouteValues = new { id = vm.Post.Topic.Forum.Id, urltitle = vm.Post.Topic.Forum.UrlTitle }
            };
            var childNode2 = new MvcBreadcrumbNode("Index", "Topic", vm.Post.Topic.Title)
            {
                RouteValues = new { id = vm.Post.Topic.Id, urltitle = vm.Post.Topic.UrlTitle },
                OverwriteTitleOnExactMatch = true,
                Parent = childNode1
            };
            var childNode3 = new MvcBreadcrumbNode("Editing post", "Post", "Editing post")
            {
                Parent = childNode2
            };

            ViewData["BreadcrumbNode"] = childNode3;

            return View(vm);
        }

        [Route("post/{id}/edit")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel vm)
        {
            Post post = await _postRepository.GetById(vm.Post.Id);
            post.Contents = vm.Contents;

            await _postRepository.Update(post);

            return RedirectToAction("Index", "Topic", new { id = vm.Post.Topic.Id, urltitle = vm.Post.Topic.UrlTitle });
        }
    }
}
