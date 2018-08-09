// <copyright file="IPolicyProvider.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Interfaces
{
    using Polly.Wrap;

    /// <summary>
    /// Provider class which wraps all policies for thread status requests into a single object.
    /// </summary>
    public interface IPolicyProvider
    {
        /// <summary>
        /// Gets the wrapped policy comprising all policies for thread status requests.
        /// </summary>
        PolicyWrap<IPostAdapter> WrappedPolicy { get; }
    }
}
