// <copyright file="IPostService.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.RequestModels;
    using Models.ResponseModels;

    /// <summary>
    /// Service for data manipulation relating to retrieving the thread info for a post.
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        /// Gets the thread information for an individual post.
        /// </summary>
        /// <param name="request">Request information regarding the post to be retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the status information for the thread associated with the passed post
        /// in the form of a <see cref="ThreadStatusDto"/> object.
        /// </returns>
        Task<ThreadStatusDto> GetPost(ThreadStatusRequest request);

        /// <summary>
        /// Gets information about the most recent threads from the news blog.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a collection of <see cref="NewsPostDto"/> objects.
        /// </returns>
        Task<IEnumerable<NewsPostDto>> GetNewsPosts();
    }
}
