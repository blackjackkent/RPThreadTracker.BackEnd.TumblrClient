// <copyright file="ThreadStatusDtoTests.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Test.Models.ResponseModels
{
    using System;
    using DontPanic.TumblrSharp.Client;
    using FluentAssertions;
    using Interfaces;
    using Moq;
    using TumblrClient.Models.RequestModels;
    using TumblrClient.Models.ResponseModels;
    using Xunit;

    public class ThreadStatusDtoTests
    {
        private readonly Mock<IPostAdapter> _mockAdapter;
        private readonly ThreadStatusRequest _request;
        private readonly DateTime _beforeQueuedDate;
        private readonly DateTime _afterQueuedDate;

        public ThreadStatusDtoTests()
        {
            _mockAdapter = new Mock<IPostAdapter>();
            var queuedDate = DateTime.Now;
            _beforeQueuedDate = DateTime.Now.Subtract(TimeSpan.FromHours(3));
            _afterQueuedDate = DateTime.Now.Add(TimeSpan.FromHours(3));
            _request = new ThreadStatusRequest
            {
				ThreadId = 54321,
                CharacterUrlIdentifier = "blackjackkent",
                DateMarkedQueued = queuedDate,
                PostId = "12345",
                PartnerUrlIdentifier = "mypartner"
            };
            _mockAdapter.Setup(a => a.GetMostRecentRelevantNote(_request.CharacterUrlIdentifier, _request.PartnerUrlIdentifier))
                .Returns((BaseNote)null);
            _mockAdapter.SetupGet(a => a.Id).Returns(_request.PostId);
            _mockAdapter.SetupGet(a => a.Timestamp).Returns(_beforeQueuedDate);
            _mockAdapter.SetupGet(a => a.Url).Returns("http://www.test.com");
            _mockAdapter.SetupGet(a => a.BlogName).Returns(_request.PartnerUrlIdentifier);
        }

        public class Constructor : ThreadStatusDtoTests
        {
	        [Fact]
	        public void PopulatesThreadIdFromRequest()
	        {
				// Act
				var dto = new ThreadStatusDto(null, _request);

				// Assert
		        dto.ThreadId.Should().Be(54321);
	        }

            [Fact]
            public void ReturnsEmptyPropertiesIfPostIsNull()
            {
                // Act
                var dto = new ThreadStatusDto(null, _request);

                // Assert
                dto.PostId.Should().Be(_request.PostId);
                dto.LastPostDate.Should().Be(null);
                dto.LastPostUrl.Should().Be(null);
                dto.LastPosterUrlIdentifier.Should().Be(null);
                dto.IsCallingCharactersTurn.Should().BeTrue();
                dto.IsQueued.Should().BeFalse();
            }

            [Fact]
            public void PopulatesFromPostIfRelevantNoteIsNull()
            {
                // Act
                var dto = new ThreadStatusDto(_mockAdapter.Object, _request);

                // Assert
                dto.PostId.Should().Be(_request.PostId);
                dto.LastPostDate.Should().Be(_beforeQueuedDate.ToUniversalTime());
                dto.LastPostUrl.Should().Be("http://www.test.com");
                dto.LastPosterUrlIdentifier.Should().Be(_request.PartnerUrlIdentifier);
                dto.IsCallingCharactersTurn.Should().BeTrue();
                dto.IsQueued.Should().BeTrue();
            }

            [Fact]
            public void PopulatesFromNoteIfRelevantNoteIsNotNull()
            {
                // Arrange
                var note = new BaseNote
                {
                    BlogName = _request.CharacterUrlIdentifier,
                    Timestamp = _afterQueuedDate,
                    BlogUrl = "http://www.test2.com/",
                    PostId = "34567"
                };
                _mockAdapter.Setup(a => a.GetMostRecentRelevantNote(_request.CharacterUrlIdentifier, _request.PartnerUrlIdentifier))
                    .Returns(note);

                // Act
                var dto = new ThreadStatusDto(_mockAdapter.Object, _request);

                // Assert
                dto.PostId.Should().Be(_request.PostId);
                dto.LastPostDate.Should().Be(_afterQueuedDate.ToUniversalTime());
                dto.LastPostUrl.Should().Be("http://www.test2.com/post/34567");
                dto.LastPosterUrlIdentifier.Should().Be(_request.CharacterUrlIdentifier);
                dto.IsCallingCharactersTurn.Should().BeFalse();
                dto.IsQueued.Should().BeFalse();
            }

            [Fact]
            public void HandlesNullDateMarkedQueued()
            {
                // Arrange
                _request.DateMarkedQueued = null;

                // Act
                var dto = new ThreadStatusDto(_mockAdapter.Object, _request);

                // Assert
                dto.IsQueued.Should().BeFalse();
            }
        }
    }
}
