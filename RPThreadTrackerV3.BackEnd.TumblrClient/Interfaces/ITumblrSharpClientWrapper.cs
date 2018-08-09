// <copyright file="ITumblrSharpClientWrapper.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DontPanic.TumblrSharp;
    using DontPanic.TumblrSharp.OAuth;
    using Newtonsoft.Json;

    /// <summary>
    /// Wrapper class for the TumblrSharp client.
    /// </summary>
    public interface ITumblrSharpClientWrapper
    {
        /// <summary>
        /// Returns the OAuth token used by the client.
        /// </summary>
        /// <returns>
        /// The OAuth token used by the client.
        /// </returns>
        Token GetToken();

        /// <summary>
        /// Calls the given Tumblr API method.
        /// </summary>
        /// <typeparam name="TResult">The expected result type of the API call.</typeparam>
        /// <param name="method">The Tumblr API method which should be called.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous request.</param>
        /// <param name="converters">Converters to be used during the resonse deserialization.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <code>TResult</code> object representing the API's response.
        /// </returns>
        Task<TResult> CallApiMethodAsync<TResult>(ApiMethod method, CancellationToken cancellationToken, IEnumerable<JsonConverter> converters = null)
            where TResult : class;
    }
}
