using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

using Xunit;
using Moq;

using ForumDemo.Repositories;
using ForumDemo.Data.Models;
using ForumDemo.Controllers;
using ForumDemo.Data;


namespace ForumDemoTests.Controllers
{
    public class PostControllerTests
    {
        [Fact]
        public async Task Edit_WithUnexistingPost_ReturnsNotFound()
        {
            // Arrange
            var loggerStub = new Mock<ILogger<PostController>>();

            var repositoryStub = new Mock<PostRepository>();
            repositoryStub.Setup(repository => repository.GetById(It.IsAny<int>())).
                ReturnsAsync((Post)null);

            var controller = new PostController(loggerStub.Object, repositoryStub.Object);

            // Act
            var result = await controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
