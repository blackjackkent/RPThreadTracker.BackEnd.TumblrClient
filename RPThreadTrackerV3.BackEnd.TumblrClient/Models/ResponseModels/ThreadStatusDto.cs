// <copyright file="ThreadStatusDto.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Models.ResponseModels
{
    using System;
    using Interfaces;
    using RequestModels;

    /// <summary>
    /// View model representation of the status of a particular RP thread.
    /// </summary>
    public class ThreadStatusDto
    {
        /// <summary>
        /// Gets or sets the post ID.
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// Gets or sets the most recent post date in the thread.
        /// </summary>
        public DateTime? LastPostDate { get; set; }

        /// <summary>
        /// Gets or sets the URL identifier of the last poster in the thread.
        /// </summary>
        public string LastPosterUrlIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the URL of the last post in the thread.
        /// </summary>
        public string LastPostUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is the calling character's turn on the thread.
        /// </summary>
        public bool IsCallingCharactersTurn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the thread is currently queued by the calling character.
        /// </summary>
        public bool IsQueued { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadStatusDto"/> class.
        /// </summary>
        public ThreadStatusDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadStatusDto"/> class.
        /// </summary>
        /// <param name="post">The post adapter.</param>
        /// <param name="request">The initial request object that triggered the retrieval of thread information.</param>
        public ThreadStatusDto(IPostAdapter post, ThreadStatusRequest request)
        {
            if (post == null)
            {
                PostId = request.PostId;
                LastPostDate = null;
                LastPosterUrlIdentifier = null;
                LastPostUrl = null;
                IsCallingCharactersTurn = true;
                IsQueued = false;
                return;
            }
            var note = post.GetMostRecentRelevantNote(request.CharacterUrlIdentifier, request.PartnerUrlIdentifier);
            var dateMarkedQueued = request.DateMarkedQueued?.ToUniversalTime();
            var lastPostDate = note?.Timestamp.ToUniversalTime() ?? post.Timestamp.ToUniversalTime();
            PostId = post.Id;
            LastPostDate = lastPostDate;
            LastPosterUrlIdentifier = note?.BlogName ?? post.BlogName;
            LastPostUrl = note != null ? note.BlogUrl + "post/" + note.PostId : post.Url;
            IsCallingCharactersTurn = !string.Equals(LastPosterUrlIdentifier, request.CharacterUrlIdentifier, StringComparison.CurrentCultureIgnoreCase);
            IsQueued = dateMarkedQueued != null && dateMarkedQueued.Value > lastPostDate;
        }
    }
}
