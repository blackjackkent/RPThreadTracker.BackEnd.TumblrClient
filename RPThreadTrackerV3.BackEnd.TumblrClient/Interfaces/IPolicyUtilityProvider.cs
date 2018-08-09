// <copyright file="IPolicyUtilityProvider.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provider for useful utilities used in generating fault policies for thread status retrieval.
    /// </summary>
    public interface IPolicyUtilityProvider
    {
        /// <summary>
        /// Generates a random integer representing a jitter time for retrying an HTTP request.
        /// </summary>
        /// <param name="retryAttempt">The retry attempt number.</param>
        /// <returns>A random integer to be used for jitter time.</returns>
        int GetRandomJitterLength(int retryAttempt);

        /// <summary>
        /// Sends a single GET request to the passed URL and swallows the response.
        /// </summary>
        /// <param name="url">The URL to be queried.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        Task QueryUrl(string url);
    }
}
