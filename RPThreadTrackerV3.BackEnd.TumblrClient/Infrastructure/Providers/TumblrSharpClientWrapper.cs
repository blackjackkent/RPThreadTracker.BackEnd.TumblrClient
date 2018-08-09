// <copyright file="TumblrSharpClientWrapper.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Infrastructure.Providers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using DontPanic.TumblrSharp;
    using DontPanic.TumblrSharp.Client;
    using DontPanic.TumblrSharp.OAuth;
    using Interfaces;
    using Newtonsoft.Json;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public class TumblrSharpClientWrapper : ITumblrSharpClientWrapper
    {
        private readonly TumblrClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="TumblrSharpClientWrapper"/> class.
        /// </summary>
        /// <param name="client">The Tumblr client.</param>
        public TumblrSharpClientWrapper(TumblrClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public Token GetToken()
        {
            return _client.OAuthToken;
        }

        /// <inheritdoc />
        public async Task<TResult> CallApiMethodAsync<TResult>(ApiMethod method, CancellationToken cancellationToken, IEnumerable<JsonConverter> converters = null)
            where TResult : class
        {
            return await _client.CallApiMethodAsync<TResult>(method, cancellationToken, converters);
        }
    }
}
