// <copyright file="NewsPostDto.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Models.ResponseModels
{
    using System;
    using Interfaces;

    /// <summary>
    /// View model representation of a post from the news blog.
    /// </summary>
    public class NewsPostDto
    {
        /// <summary>
        /// Gets or sets the post ID.
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// Gets or sets the date of the post.
        /// </summary>
        public DateTime? PostDate { get; set; }

        /// <summary>
        /// Gets or sets the post title.
        /// </summary>
        public string PostTitle { get; set; }

        /// <summary>
        /// Gets or sets the post URL.
        /// </summary>
        public string PostUrl { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsPostDto"/> class.
        /// </summary>
        public NewsPostDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsPostDto"/> class.
        /// </summary>
        /// <param name="postAdapter">The post adapter.</param>
        public NewsPostDto(IPostAdapter postAdapter)
        {
            PostId = postAdapter.Id;
            PostDate = postAdapter.Timestamp.ToUniversalTime();
            PostTitle = postAdapter.Title;
            PostUrl = postAdapter.Url;
        }
    }
}
