// <copyright file="NewsPostDtoTests.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Test.Models.ResponseModels
{
    using System;
    using FluentAssertions;
    using Interfaces;
    using Moq;
    using TumblrClient.Models.ResponseModels;
    using Xunit;

    public class NewsPostDtoTests
    {
        public class Constructor : NewsPostDtoTests
        {
            [Fact]
            public void PopulatesPropertiesFromPostAdapter()
            {
                // Arrange
                var mockPostAdapter = new Mock<IPostAdapter>();
                mockPostAdapter.SetupGet(a => a.Id).Returns("12345");
                mockPostAdapter.SetupGet(a => a.Title).Returns("My Awesome Post");
                mockPostAdapter.SetupGet(a => a.Url).Returns("http://www.test.com");
                var date = DateTime.Now;
                mockPostAdapter.SetupGet(a => a.Timestamp).Returns(date);

                // Act
                var newsPost = new NewsPostDto(mockPostAdapter.Object);

                // Assert
                newsPost.PostId.Should().Be("12345");
                newsPost.PostUrl.Should().Be("http://www.test.com");
                newsPost.PostDate.Should().Be(date.ToUniversalTime());
                newsPost.PostTitle.Should().Be("My Awesome Post");
            }
        }
    }
}
