// <copyright file="ITumblrSharpFactoryWrapper.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Interfaces
{
    using DontPanic.TumblrSharp.OAuth;

    /// <summary>
    /// Wrapper class for the Tumblr Sharp client factory.
    /// </summary>
    public interface ITumblrSharpFactoryWrapper
    {
        /// <summary>
        /// Generates a new <see cref="ITumblrSharpClientWrapper" /> instance.
        /// </summary>
        /// <param name="consumerKey">The Tumblr API consumer key.</param>
        /// <param name="consumerSecret">The Tumblr API consumer secret.</param>
        /// <param name="oAuthToken">The Tumblr API OAuth token.</param>
        /// <returns>An instance of the <see cref="ITumblrSharpClientWrapper"/> interface.</returns>
        ITumblrSharpClientWrapper Create(string consumerKey, string consumerSecret, Token oAuthToken = null);
    }
}
