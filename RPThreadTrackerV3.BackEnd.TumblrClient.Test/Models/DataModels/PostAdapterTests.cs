// <copyright file="PostAdapterTests.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Test.Models.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using DontPanic.TumblrSharp;
    using DontPanic.TumblrSharp.Client;
    using FluentAssertions;
    using TumblrClient.Models.DataModels;
    using Xunit;

    public class PostAdapterTests
    {
        public class Constructor
        {
            [Fact]
            public void PopulatesPropertiesFromBasePost()
            {
                // Arrange
                var postDateTime = DateTime.Now;
                var basePost = new TextPost()
                {
                    Id = 12345,
                    Timestamp = postDateTime,
                    BlogName = "blackjackkent",
                    Url = "http://www.test.com",
                    Title = "My Awesome Title"
                };

                // Act
                var adapter = new PostAdapter(basePost);

                // Assert
                adapter.Id.Should().Be(basePost.Id.ToString(CultureInfo.InvariantCulture));
                adapter.Timestamp.Should().Be(basePost.Timestamp);
                adapter.BlogName.Should().Be(basePost.BlogName);
                adapter.Url.Should().Be(basePost.Url);
                adapter.Title.Should().Be(basePost.Title);
            }

            [Fact]
            public void HandlesInvalidPostObjectType()
            {
                // Arrange
                var postDateTime = DateTime.Now;
                var basePost = new PhotoPost()
                {
                    Id = 12345,
                    Timestamp = postDateTime,
                    BlogName = "blackjackkent",
                    Url = "http://www.test.com"
                };

                // Act
                var adapter = new PostAdapter(basePost);

                // Assert
                adapter.Id.Should().Be(basePost.Id.ToString(CultureInfo.InvariantCulture));
                adapter.Timestamp.Should().Be(basePost.Timestamp);
                adapter.BlogName.Should().Be(basePost.BlogName);
                adapter.Url.Should().Be(basePost.Url);
                adapter.Title.Should().Be(null);
            }

            public class GetMostRecentRelevantNote
            {
                [Fact]
                public void ReturnsNullIfNotesAreNull()
                {
                    // Arrange
                    var basePost = new BasePost
                    {
                        Notes = null
                    };
                    var adapter = new PostAdapter(basePost);

                    // Act
                    var note = adapter.GetMostRecentRelevantNote("blackjackkent", "mypartner");

                    // Assert
                    note.Should().BeNull();
                }

                [Fact]
                public void ReturnsNullIfNotesAreEmpty()
                {
                    // Arrange
                    var basePost = new BasePost
                    {
                        Notes = new List<BaseNote>()
                    };
                    var adapter = new PostAdapter(basePost);

                    // Act
                    var note = adapter.GetMostRecentRelevantNote("blackjackkent", "mypartner");

                    // Assert
                    note.Should().BeNull();
                }

                [Fact]
                public void ReturnsNullIfNotesContainsNoReblogs()
                {
                    // Arrange
                    var basePost = new BasePost
                    {
                        Notes = new List<BaseNote>
                        {
                            new BaseNote { Type = NoteType.Like },
                            new BaseNote { Type = NoteType.Reply },
                            new BaseNote { Type = NoteType.Posted }
                        }
                    };
                    var adapter = new PostAdapter(basePost);

                    // Act
                    var note = adapter.GetMostRecentRelevantNote("blackjackkent", "mypartner");

                    // Assert
                    note.Should().BeNull();
                }

                [Fact]
                public void ReturnsMostRecentNoteIfNoPartnerIdentifierAndYourTurn()
                {
                    // Arrange
                    var date = DateTime.Now;
                    var yesterday = date.AddDays(-1);
                    var threeDaysAgo = date.AddDays(-3);
                    var fiveDaysAgo = date.AddDays(-5);
                    var basePost = new BasePost
                    {
                        Notes = new List<BaseNote>
                        {
                            new BaseNote { PostId = "1", Type = NoteType.Reblog, Timestamp = threeDaysAgo, BlogName = "blackjackkent" },
                            new BaseNote { PostId = "2", Type = NoteType.Reblog, Timestamp = yesterday, BlogName = "partnerblog" },
                            new BaseNote { PostId = "3", Type = NoteType.Reblog, Timestamp = fiveDaysAgo, BlogName = "partnerblog" }
                        }
                    };
                    var adapter = new PostAdapter(basePost);

                    // Act
                    var note = adapter.GetMostRecentRelevantNote("blackjackkent", null);

                    // Assert
                    note.Should().NotBe(null);
                    note.PostId.Should().Be("2");
                    note.Timestamp.Should().Be(yesterday);
                }

                [Fact]
                public void ReturnsMostRecentNoteIfNoPartnerIdentifierAndTheirTurn()
                {
                    // Arrange
                    var date = DateTime.Now;
                    var yesterday = date.AddDays(-1);
                    var threeDaysAgo = date.AddDays(-3);
                    var fiveDaysAgo = date.AddDays(-5);
                    var basePost = new BasePost
                    {
                        Notes = new List<BaseNote>
                        {
                            new BaseNote { PostId = "1", Type = NoteType.Reblog, Timestamp = threeDaysAgo, BlogName = "partnerblog" },
                            new BaseNote { PostId = "2", Type = NoteType.Reblog, Timestamp = yesterday, BlogName = "blackjackkent" },
                            new BaseNote { PostId = "3", Type = NoteType.Reblog, Timestamp = fiveDaysAgo, BlogName = "blackjackkent" }
                        }
                    };
                    var adapter = new PostAdapter(basePost);

                    // Act
                    var note = adapter.GetMostRecentRelevantNote("blackjackkent", null);

                    // Assert
                    note.Should().NotBe(null);
                    note.PostId.Should().Be("2");
                    note.Timestamp.Should().Be(yesterday);
                }

                [Fact]
                public void ReturnsPartnersMostRecentReblogIfPartnerIdentifierProvidedAndYourTurn()
                {
                    // Arrange
                    var date = DateTime.Now;
                    var yesterday = date.AddDays(-1);
                    var threeDaysAgo = date.AddDays(-3);
                    var fiveDaysAgo = date.AddDays(-5);
                    var basePost = new BasePost
                    {
                        Notes = new List<BaseNote>
                        {
                            new BaseNote { PostId = "1", Type = NoteType.Reblog, Timestamp = threeDaysAgo, BlogName = "partnerblog" },
                            new BaseNote { PostId = "2", Type = NoteType.Reblog, Timestamp = yesterday, BlogName = "someOtherBlog" },
                            new BaseNote { PostId = "3", Type = NoteType.Reblog, Timestamp = fiveDaysAgo, BlogName = "blackjackkent" }
                        }
                    };
                    var adapter = new PostAdapter(basePost);

                    // Act
                    var note = adapter.GetMostRecentRelevantNote("blackjackkent", "partnerblog");

                    // Assert
                    note.Should().NotBe(null);
                    note.PostId.Should().Be("1");
                    note.Timestamp.Should().Be(threeDaysAgo);
                }

                [Fact]
                public void ReturnsUsersMostRecentReblogIfPartnerIdentifierProvidedAndTheirTurn()
                {
                    // Arrange
                    var date = DateTime.Now;
                    var yesterday = date.AddDays(-1);
                    var threeDaysAgo = date.AddDays(-3);
                    var fiveDaysAgo = date.AddDays(-5);
                    var basePost = new BasePost
                    {
                        Notes = new List<BaseNote>
                        {
                            new BaseNote { PostId = "1", Type = NoteType.Reblog, Timestamp = threeDaysAgo, BlogName = "blackjackkent" },
                            new BaseNote { PostId = "2", Type = NoteType.Reblog, Timestamp = yesterday, BlogName = "someOtherBlog" },
                            new BaseNote { PostId = "3", Type = NoteType.Reblog, Timestamp = fiveDaysAgo, BlogName = "partnerblog" }
                        }
                    };
                    var adapter = new PostAdapter(basePost);

                    // Act
                    var note = adapter.GetMostRecentRelevantNote("blackjackkent", "partnerblog");

                    // Assert
                    note.Should().NotBe(null);
                    note.PostId.Should().Be("1");
                    note.Timestamp.Should().Be(threeDaysAgo);
                }

                [Fact]
                public void IgnoresBlogNameCase()
                {
                    // Arrange
                    var date = DateTime.Now;
                    var yesterday = date.AddDays(-1);
                    var threeDaysAgo = date.AddDays(-3);
                    var fiveDaysAgo = date.AddDays(-5);
                    var basePost = new BasePost
                    {
                        Notes = new List<BaseNote>
                        {
                            new BaseNote { PostId = "1", Type = NoteType.Reblog, Timestamp = threeDaysAgo, BlogName = "blackjackkent" },
                            new BaseNote { PostId = "2", Type = NoteType.Reblog, Timestamp = yesterday, BlogName = "partnerBlog" },
                            new BaseNote { PostId = "3", Type = NoteType.Reblog, Timestamp = fiveDaysAgo, BlogName = "partnerblog" }
                        }
                    };
                    var adapter = new PostAdapter(basePost);

                    // Act
                    var note = adapter.GetMostRecentRelevantNote("blackjackkent", "partnerblog");

                    // Assert
                    note.Should().NotBe(null);
                    note.PostId.Should().Be("2");
                    note.Timestamp.Should().Be(yesterday);
                }
            }
        }
    }
}
