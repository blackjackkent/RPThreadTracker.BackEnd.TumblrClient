// <copyright file="PolicyProviderTests.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Test.Infrastructure.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using DontPanic.TumblrSharp;
    using DontPanic.TumblrSharp.Client;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Interfaces;
    using Moq;
    using Polly;
    using Polly.Fallback;
    using Polly.Utilities;
    using Polly.Wrap;
    using TumblrClient.Infrastructure.Providers;
    using TumblrClient.Models.DataModels;
    using Xunit;

    [Trait("Class", "PolicyProvider")]
    public class PolicyProviderTests
    {
        private readonly Mock<IPolicyUtilityProvider> _mockUtility;

        public PolicyProviderTests()
        {
            _mockUtility = new Mock<IPolicyUtilityProvider>();
        }

        public class RetryPolicy : PolicyProviderTests
        {
            private readonly Polly.Retry.RetryPolicy _policy;
            private int _totalTimeSlept;
            private int _totalRetries;

            public RetryPolicy()
            {
                _totalTimeSlept = 0;
                _totalRetries = 0;
                SystemClock.SleepAsync = (span, token) =>
                {
                    _totalTimeSlept += span.Seconds;
                    _totalRetries++;
                    return TaskHelper.FromResult(true);
                };
                _mockUtility.Setup(u => u.GetRandomJitterLength(It.IsAny<int>())).Returns(1000);
                var policyProvider = new PolicyProvider(_mockUtility.Object);
                _policy = (policyProvider.WrappedPolicy.Inner as PolicyWrap)?.Inner as Polly.Retry.RetryPolicy;
            }

            [Fact]
            public void RetriesUntilSuccessWhenResponseCode429()
            {
                // Act
                Func<Task> act = async () => await _policy.ExecuteAsync(() =>
                {
                    if (_totalRetries == 3)
                    {
                        return Task.FromResult(new PostAdapter(new BasePost { Id = 12345 }));
                    }
                    var exception = new TumblrException((HttpStatusCode)429);
                    throw exception;
                });

                // Assert
                act.Should().NotThrow();
                _totalTimeSlept.Should().Be(17);
                _totalRetries.Should().Be(3);
            }

            [Fact]
            public void StopsAfterFiveRetriesAndBubblesExceptionWhenResponseCode429()
            {
                // Act
                Func<Task> act = async () => await _policy.ExecuteAsync(() =>
                {
                    var exception = new TumblrException((HttpStatusCode)429);
                    throw exception;
                });

                // Assert
                act.Should().Throw<TumblrException>();
                _totalTimeSlept.Should().Be(67);
                _totalRetries.Should().Be(5);
            }
        }

        public class RefreshPolicy : PolicyProviderTests
        {
            private readonly Polly.Retry.RetryPolicy _policy;
            private int _totalTries;

            public RefreshPolicy()
            {
                _totalTries = 0;
                var policyProvider = new PolicyProvider(_mockUtility.Object);
                _policy = (policyProvider.WrappedPolicy.Inner as PolicyWrap)?.Outer as Polly.Retry.RetryPolicy;
            }

            [Fact]
            public void RetriesAndCallsClientOnFirstTryWhenResponseCode404()
            {
                var data = new Dictionary<string, object>
                {
                    { "postId", "12345" },
                    { "characterUrlIdentifier", "blackjackkent" }
                };

                // Act
                Func<Task> act = async () => await _policy.ExecuteAsync(
                    context =>
                    {
                        if (_totalTries == 1)
                        {
                            return Task.FromResult(new PostAdapter(new BasePost { Id = 12345 }));
                        }
                        _totalTries++;
                        throw new TumblrException(HttpStatusCode.NotFound);
                    }, new Context("RetrieveApiDataByPost", data));

                // Assert
                act.Should().NotThrow();
                _totalTries.Should().Be(1);
                _mockUtility.Verify(u => u.QueryUrl("http://blackjackkent.tumblr.com/post/12345"), Times.Once);
            }

            [Fact]
            public void BubblesExceptionAfterRetryWhenResponseCode404()
            {
                // Act
                var data = new Dictionary<string, object>() { { "postId", "12345" }, { "characterUrlIdentifier", "blackjackkent" } };
                Func<Task> act = async () => await _policy.ExecuteAsync(
                    context =>
                    {
                        _totalTries++;
                        throw new TumblrException(HttpStatusCode.NotFound);
                    },
                    new Context("RetrieveApiDataByPost", data));

                // Assert
                act.Should().Throw<TumblrException>();
                _totalTries.Should().Be(2);
                _mockUtility.Verify(u => u.QueryUrl("http://blackjackkent.tumblr.com/post/12345"), Times.Once);
            }
        }

        public class NotFoundPolicy : PolicyProviderTests
        {
            private FallbackPolicy<IPostAdapter> _policy;

            public NotFoundPolicy()
            {
                var policyProvider = new PolicyProvider(_mockUtility.Object);
                _policy = policyProvider.WrappedPolicy.Outer as FallbackPolicy<IPostAdapter>;
            }

            [Fact]
            public async Task ReturnsNullWhenAnyException()
            {
                // Act
                Func<Task> act404 = async () => await _policy.ExecuteAsync(() => throw new TumblrException(HttpStatusCode.NotFound));
                var result404 = await _policy.ExecuteAsync(() => throw new TumblrException(HttpStatusCode.NotFound));
                Func<Task> act429 = async () => await _policy.ExecuteAsync(() => throw new TumblrException((HttpStatusCode)429));
                var result429 = await _policy.ExecuteAsync(() => throw new TumblrException((HttpStatusCode)429));
                Func<Task> act500 = async () => await _policy.ExecuteAsync(() => throw new TumblrException(HttpStatusCode.InternalServerError));
                var result500 = await _policy.ExecuteAsync(() => throw new TumblrException(HttpStatusCode.InternalServerError));

                // Assert
                act404.Should().NotThrow();
                result404.Should().BeNull();
                act429.Should().NotThrow();
                result429.Should().BeNull();
                act500.Should().NotThrow();
                result500.Should().BeNull();
            }
        }
    }
}
