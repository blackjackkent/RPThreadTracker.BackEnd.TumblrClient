// <copyright file="NewsControllerTests.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Test.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using TumblrClient.Controllers;
    using TumblrClient.Models.ResponseModels;
    using Xunit;

    [Trait("Class", "NewsController")]
    public class NewsControllerTests
    {
        private readonly Mock<IPostService> _mockPostService;
        private readonly NewsController _controller;

        public NewsControllerTests()
        {
            var mockLogger = new Mock<ILogger<NewsController>>();
            _mockPostService = new Mock<IPostService>();
            _controller = new NewsController(mockLogger.Object, _mockPostService.Object);
        }

        public class GetNewsPosts : NewsControllerTests
        {
            [Fact]
            public async Task ReturnsListOfPostsWhenRequested()
            {
                // Arrange
                IEnumerable<NewsPostDto> results = new List<NewsPostDto>
                {
                    new NewsPostDto { PostId = "12345" },
                    new NewsPostDto { PostId = "23456" },
                    new NewsPostDto { PostId = "34567" }
                };
                _mockPostService.Setup(s => s.GetNewsPosts()).Returns(Task.FromResult(results));

                // Act
                var result = await _controller.Get();
                var body = ((OkObjectResult)result).Value as List<NewsPostDto>;

                // Assert
                _mockPostService.Verify(s => s.GetNewsPosts(), Times.Once);
                result.Should().BeOfType<OkObjectResult>();
                body?.Should().HaveCount(3)
                    .And.Contain(t => t.PostId == "12345")
                    .And.Contain(t => t.PostId == "23456")
                    .And.Contain(t => t.PostId == "34567");
            }

            [Fact]
            public async Task ReturnsErrorResponseWhenException()
            {
                // Arrange
                _mockPostService.Setup(s => s.GetNewsPosts()).Throws<Exception>();

                // Act
                var response = await _controller.Get();

                // Assert
                _mockPostService.Verify(s => s.GetNewsPosts(), Times.Once);
                response.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }
        }
    }
}
