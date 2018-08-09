// <copyright file="TumblrSharpFactoryWrapper.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Infrastructure.Providers
{
    using System.Diagnostics.CodeAnalysis;
    using DontPanic.TumblrSharp;
    using DontPanic.TumblrSharp.Client;
    using DontPanic.TumblrSharp.OAuth;
    using Interfaces;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public class TumblrSharpFactoryWrapper : ITumblrSharpFactoryWrapper
    {
        private readonly ITumblrClientFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TumblrSharpFactoryWrapper"/> class.
        /// </summary>
        /// <param name="factory">The client factory.</param>
        public TumblrSharpFactoryWrapper(ITumblrClientFactory factory)
        {
            _factory = factory;
        }

        /// <inheritdoc />
        public ITumblrSharpClientWrapper Create(string consumerKey, string consumerSecret, Token oAuthToken = null)
        {
            var tumblrSharpClient = _factory.Create<TumblrClient>(consumerKey, consumerSecret, oAuthToken);
            return new TumblrSharpClientWrapper(tumblrSharpClient);
        }
    }
}
