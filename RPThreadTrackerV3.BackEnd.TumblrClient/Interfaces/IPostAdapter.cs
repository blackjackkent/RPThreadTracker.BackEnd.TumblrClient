// <copyright file="IPostAdapter.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Interfaces
{
    using System;
    using DontPanic.TumblrSharp.Client;
    using Models.ResponseModels;

    /// <summary>
    /// Adapter class used to pass between TumblrSharp's Post class and
    /// the <see cref="ThreadStatusDto" /> and <see cref="NewsPostDto"/> classes.
    /// </summary>
    public interface IPostAdapter
    {
        /// <summary>
        /// Gets the blog shortname.
        /// </summary>
        string BlogName { get; }

        /// <summary>
        /// Gets the post ID.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the post timestamp.
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// Gets the post title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the full post URL.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Processes the post's note objects and returns a note representation of the most recent post in the thread,
        /// given the passed character and partner.
        /// </summary>
        /// <param name="characterUrlIdentifier">
        /// The character URL identifier against which the post's notes should be compared.
        /// </param>
        /// <param name="partnerUrlIdentifier">
        /// The partner URL identifier (if provided) against which the post's notes should be compared.
        /// </param>
        /// <returns>A note representation of the most recent relevant post in the thread.</returns>
        BaseNote GetMostRecentRelevantNote(string characterUrlIdentifier, string partnerUrlIdentifier);
    }
}
