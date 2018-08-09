// <copyright file="PolicyUtilityProvider.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Infrastructure.Providers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Interfaces;

    /// <inheritdoc cref="IPolicyUtilityProvider" />
    [ExcludeFromCodeCoverage]
    public class PolicyUtilityProvider : IPolicyUtilityProvider, IDisposable
    {
        private readonly Random _random;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyUtilityProvider"/> class.
        /// </summary>
        public PolicyUtilityProvider()
        {
            _random = new Random();
            _httpClient = new HttpClient();
        }

        /// <inheritdoc />
        public int GetRandomJitterLength(int retryAttempt)
        {
            return _random.Next(0, 1000);
        }

        /// <inheritdoc />
        public async Task QueryUrl(string url)
        {
            try
            {
                await _httpClient.GetAsync(new Uri(url));
            }
            catch (Exception)
            {
                // we don't care if it 404ed
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpClient?.Dispose();
            }
        }
    }
}
