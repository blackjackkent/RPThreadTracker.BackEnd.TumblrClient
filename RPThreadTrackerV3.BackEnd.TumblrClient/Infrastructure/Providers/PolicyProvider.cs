// <copyright file="PolicyProvider.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Infrastructure.Providers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using DontPanic.TumblrSharp;
    using Interfaces;
    using Polly;
    using Polly.Wrap;

    /// <inheritdoc />
    public class PolicyProvider : IPolicyProvider
    {
        private readonly IPolicyUtilityProvider _utilityProvider;

        /// <inheritdoc/>
        public PolicyWrap<IPostAdapter> WrappedPolicy { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyProvider"/> class.
        /// </summary>
        /// <param name="utilityProvider">The utility provider.</param>
        public PolicyProvider(IPolicyUtilityProvider utilityProvider)
        {
            _utilityProvider = utilityProvider;
            IAsyncPolicy retryPolicy = Policy
                .Handle<TumblrException>(e => e.StatusCode == (HttpStatusCode)429)
                .WaitAndRetryAsync(
                    5,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                    + TimeSpan.FromMilliseconds(_utilityProvider.GetRandomJitterLength(retryAttempt)));

            IAsyncPolicy refreshPolicy = Policy
                .Handle<TumblrException>(e => e.StatusCode == (HttpStatusCode)404)
                .RetryAsync(1, RefreshTumblrUrl);
            IAsyncPolicy<IPostAdapter> notFoundPolicy = Policy<IPostAdapter>.Handle<Exception>().FallbackAsync((IPostAdapter)null);
            WrappedPolicy = notFoundPolicy.WrapAsync(refreshPolicy.WrapAsync(retryPolicy));
        }

        private async Task RefreshTumblrUrl(Exception exception, int retryCount, Context context)
        {
            var postId = context["postId"];
            var characterUrlIdentifier = context["characterUrlIdentifier"];
            var url = "http://" + characterUrlIdentifier + ".tumblr.com/post/" + postId;
            await _utilityProvider.QueryUrl(url);
        }
    }
}
