// <copyright file="ThreadControllerTests.cs" company="Rosalind Wills">
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
    using TumblrClient.Models.RequestModels;
    using TumblrClient.Models.ResponseModels;
    using Xunit;

    [Trait("Class", "ThreadController")]
    public class ThreadControllerTests
    {
        private readonly Mock<IPostService> _mockPostService;
        private readonly ThreadController _controller;

        public ThreadControllerTests()
        {
            var mockLogger = new Mock<ILogger<ThreadController>>();
            _mockPostService = new Mock<IPostService>();
            _controller = new ThreadController(mockLogger.Object, _mockPostService.Object);
        }

        public class GetSingleThread : ThreadControllerTests
        {
            [Fact]
            public async Task ReturnsSinglePostWhenRequested()
            {
                // Arrange
                var dto = new ThreadStatusDto
                {
                    PostId = "12345"
                };
                _mockPostService.Setup(s => s.GetPost(It.IsAny<ThreadStatusRequest>())).Returns(Task.FromResult(dto));

                // Act
                var response = await _controller.Get("postId", "characterUrlIdentifier", "partnerUrlIdentifier", DateTime.UtcNow);
                var body = ((OkObjectResult)response).Value as ThreadStatusDto;

                // Assert
                _mockPostService.Verify(s => s.GetPost(It.IsAny<ThreadStatusRequest>()), Times.Once);
                response.Should().BeOfType<OkObjectResult>();
                body?.PostId.Should().Be("12345");
            }

            [Fact]
            public async Task ReturnsErrorResponseWhenException()
            {
                // Arrange
                _mockPostService.Setup(s => s.GetPost(It.IsAny<ThreadStatusRequest>())).Throws<Exception>();

                // Act
                var response = await _controller.Get("postId", "characterUrlIdentifier", "partnerUrlIdentifier", DateTime.UtcNow);

                // Assert
                _mockPostService.Verify(s => s.GetPost(It.IsAny<ThreadStatusRequest>()), Times.Once);
                response.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }
        }

        public class Post : ThreadControllerTests
        {
            [Fact]
            public async Task ReturnsEmptyArrayWhenNoPostsRequested()
            {
                // Arrange
                var requests = new List<ThreadStatusRequest>();

                // Act
                var response = await _controller.Post(requests);
                var body = ((OkObjectResult)response).Value as List<ThreadStatusDto>;

                // Assert
                response.Should().BeOfType<OkObjectResult>();
                body?.Count.Should().Be(0); // got everyone out alive
            }

            [Fact]
            public async Task ReturnsAllRequestedPostsWhenRetrievedSuccessfully()
            {
                // Arrange
                var requests = new List<ThreadStatusRequest>
                {
                    new ThreadStatusRequest
                    {
                        PostId = "12345"
                    },
                    new ThreadStatusRequest
                    {
                        PostId = "23456"
                    },
                    new ThreadStatusRequest
                    {
                        PostId = "34567"
                    },
                    new ThreadStatusRequest
                    {
                        PostId = "45678"
                    }
                };
                _mockPostService.Setup(s => s.GetPost(It.IsAny<ThreadStatusRequest>()))
                    .ReturnsAsync((ThreadStatusRequest r) => new ThreadStatusDto { PostId = r.PostId });

                // Act
                var response = await _controller.Post(requests);
                var body = ((OkObjectResult)response).Value as List<ThreadStatusDto>;

                // Assert
                response.Should().BeOfType<OkObjectResult>();
                body?.Count.Should().Be(4);
                body.Should().Contain(t => t.PostId == "12345");
                body.Should().Contain(t => t.PostId == "23456");
                body.Should().Contain(t => t.PostId == "34567");
                body.Should().Contain(t => t.PostId == "45678");
            }

            [Fact]
            public async Task ReturnsErrorResponseWhenAnyExceptions()
            {
                // Arrange
                var count = 0;
                _mockPostService.Setup(a => a.GetPost(It.IsAny<ThreadStatusRequest>())).Callback(() =>
                {
                    count++;
                    if (count == 3)
                    {
                        throw new Exception();
                    }
                });
                var requests = new List<ThreadStatusRequest>
                {
                    new ThreadStatusRequest
                    {
                        PostId = "12345"
                    },
                    new ThreadStatusRequest
                    {
                        PostId = "23456"
                    },
                    new ThreadStatusRequest
                    {
                        PostId = "34567"
                    },
                    new ThreadStatusRequest
                    {
                        PostId = "45678"
                    }
                };

                // Act
                var response = await _controller.Post(requests);

                // Assert
                response.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }
        }
    }
}
