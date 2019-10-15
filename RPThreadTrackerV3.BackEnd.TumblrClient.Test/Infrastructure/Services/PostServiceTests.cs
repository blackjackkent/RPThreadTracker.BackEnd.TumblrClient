// <copyright file="PostServiceTests.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Test.Infrastructure.Services
{
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DontPanic.TumblrSharp;
    using DontPanic.TumblrSharp.Client;
    using DontPanic.TumblrSharp.OAuth;
    using FluentAssertions;
    using Interfaces;
    using Microsoft.Extensions.Options;
    using Moq;
    using Polly;
    using TumblrClient.Infrastructure.Services;
    using TumblrClient.Models.Configuration;
    using TumblrClient.Models.RequestModels;
    using Xunit;

    public class PostServiceTests
    {
        private readonly Mock<IPolicyProvider> _mockPolicyProvider;
        private readonly Mock<ITumblrSharpFactoryWrapper> _mockFactory;
        private readonly Mock<IOptions<AppSettings>> _mockConfigWrapper;
        private ApiMethod _passedMethod;

        public PostServiceTests()
        {
            var config = new AppSettings
            {
                NewsBlogShortname = "tblrthreadtracker",
                Secure = new SecureAppSettings
                {
                    TumblrOauthToken = "OauthToken",
                    TumblrOauthSecret = "OauthSecret",
                    TumblrConsumerKey = "ConsumerKey",
                    TumblrConsumerSecret = "ConsumerSecret"
                }
            };
            _mockConfigWrapper = new Mock<IOptions<AppSettings>>();
            _mockConfigWrapper.SetupGet(c => c.Value).Returns(config);
            _mockPolicyProvider = new Mock<IPolicyProvider>();
            _mockPolicyProvider.SetupGet(p => p.WrappedPolicy).Returns(Policy.NoOpAsync<IPostAdapter>().WrapAsync(Policy.NoOpAsync()));
            _mockFactory = new Mock<ITumblrSharpFactoryWrapper>();
        }

        public void InitMockClient(Posts mockResult)
        {
            var mockTumblrSharpClientWrapper = new Mock<ITumblrSharpClientWrapper>();
            _mockFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Token>()))
                .Returns(mockTumblrSharpClientWrapper.Object);
            mockTumblrSharpClientWrapper
                .Setup(c => c.CallApiMethodAsync<Posts>(It.IsAny<BlogMethod>(), It.IsAny<CancellationToken>(), null))
                .Returns((ApiMethod method, CancellationToken token, IEnumerable<JsonConverter> converters) =>
                {
                    _passedMethod = method;
                    return Task.FromResult(mockResult);
                });
        }

        public class Constructor : PostServiceTests
        {
            [Fact]
            public void InitializesClientFromConfiguration()
            {
                // Act
                var postService = new PostService(_mockPolicyProvider.Object, _mockConfigWrapper.Object, _mockFactory.Object);

                // Assert
                postService.Should().NotBe(null);
                _mockFactory.Verify(f => f.Create(
                    "ConsumerKey",
                    "ConsumerSecret",
                    It.Is<Token>(t => t.Key == "OauthToken" && t.Secret == "OauthSecret")));
            }
        }

        public class GetPost : PostServiceTests
        {
            [Fact]
            public async Task ReturnsNullForMissingPostId()
            {
                // Arrange
                var request = new ThreadStatusRequest
                {
                    PostId = null,
                    CharacterUrlIdentifier = "blackjackkent"
                };
                var postService = new PostService(_mockPolicyProvider.Object, _mockConfigWrapper.Object, _mockFactory.Object);

                // Act
                var result = await postService.GetPost(request);

                // Assert
                result.Should().Be(null);
            }

            [Fact]
            public async Task ReturnsNullForMissingCharacterUrlIdentifier()
            {
                // Arrange
                var request = new ThreadStatusRequest
                {
                    PostId = "12345",
                    CharacterUrlIdentifier = null
                };
                var postService = new PostService(_mockPolicyProvider.Object, _mockConfigWrapper.Object, _mockFactory.Object);

                // Act
                var result = await postService.GetPost(request);

                // Assert
                result.Should().Be(null);
            }

            [Fact]
            public async Task ReturnsEmptyAdapterIfNoResults()
            {
                // Arrange
                var request = new ThreadStatusRequest
                {
                    PostId = "123456",
                    CharacterUrlIdentifier = "test"
                };
                var mockResult = new Posts() { Result = System.Array.Empty<BasePost>() };
                InitMockClient(mockResult);
                var postService = new PostService(_mockPolicyProvider.Object, _mockConfigWrapper.Object, _mockFactory.Object);

                // Act
                var result = await postService.GetPost(request);

                // Assert
                result.PostId.Should().Be(request.PostId);
                result.LastPostUrl.Should().BeNull();
            }

            [Fact]
            public async Task ReturnsPopulatedAdapterIfResultValid()
            {
                // Arrange
                var request = new ThreadStatusRequest
                {
                    PostId = "123456",
                    CharacterUrlIdentifier = "test"
                };
                var mockResult = new Posts
                {
                    Result = new[]
                    {
                        new BasePost
                        {
                            Id = int.Parse(request.PostId, CultureInfo.InvariantCulture),
                            BlogName = request.CharacterUrlIdentifier,
                            Url = "http://www.test.com"
                        },
                        new BasePost
                        {
                            Id = int.Parse(request.PostId, CultureInfo.InvariantCulture),
                            BlogName = request.CharacterUrlIdentifier,
                            Url = "http://www.test2.com"
                        }
                    }
                };
                InitMockClient(mockResult);
                var postService = new PostService(_mockPolicyProvider.Object, _mockConfigWrapper.Object, _mockFactory.Object);

                // Act
                var result = await postService.GetPost(request);

                // Assert
                result.PostId.Should().Be(request.PostId);
                result.LastPostUrl.Should().Be("http://www.test.com");
            }

            [Fact]
            public async Task NormalizesCharacterUrl()
            {
                // Arrange
                var request = new ThreadStatusRequest
                {
                    PostId = "123456",
                    CharacterUrlIdentifier = "BlackjackKent"
                };
                var mockResult = new Posts
                {
                    Result = new List<BasePost>().ToArray()
                };
                InitMockClient(mockResult);
                var postService = new PostService(_mockPolicyProvider.Object, _mockConfigWrapper.Object, _mockFactory.Object);

                // Act
                await postService.GetPost(request);

                // Assert
                _passedMethod.Url.Should().Contain("blackjackkent");
            }
        }

        public class GetNewsPosts : PostServiceTests
        {
            [Fact]
            public async Task ReturnsEmptyListWhenClientResultEmpty()
            {
                // Arrange
                var mockResult = new Posts() { Result = System.Array.Empty<BasePost>() };
                InitMockClient(mockResult);
                var postService = new PostService(_mockPolicyProvider.Object, _mockConfigWrapper.Object, _mockFactory.Object);

                // Act
                var result = await postService.GetNewsPosts();

                // Assert
                result.Count().Should().Be(0);
            }

            [Fact]
            public async Task ReturnsPostsWhenClientResultIsValid()
            {
                // Arrange
                var mockResult = new Posts
                {
                    Result = new[]
                    {
                        new BasePost
                        {
                            Id = 12345,
                            BlogName = "tblrthreadtracker",
                            Url = "http://www.test.com"
                        },
                        new BasePost
                        {
                            Id = 23456,
                            BlogName = "tblrthreadtracker",
                            Url = "http://www.test2.com"
                        }
                    }
                };
                InitMockClient(mockResult);
                var postService = new PostService(_mockPolicyProvider.Object, _mockConfigWrapper.Object, _mockFactory.Object);

                // Act
                var result = await postService.GetNewsPosts();

                // Assert
                result.Count().Should().Be(2);
                result.Any(p => p.PostId == "12345").Should().BeTrue();
                result.Any(p => p.PostId == "23456").Should().BeTrue();
            }
        }
    }
}
