// <copyright file="ThreadStatusRequest.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Models.RequestModels
{
    using System;

    /// <summary>
    /// Request model containing data about a thread whose status should be retrieved.
    /// </summary>
    public class ThreadStatusRequest
	{
		/// <summary>
		/// Gets or sets the tracker thread ID.
		/// </summary>
		/// <value>
		/// The thread ID.
		/// </value>
		public int? ThreadId { get; set; }

		/// <summary>
		/// Gets or sets the post ID.
		/// </summary>
		/// <value>
		/// The post ID.
		/// </value>
		public string PostId { get; set; }

        /// <summary>
        /// Gets or sets the character URL identifier.
        /// </summary>
        /// <value>
        /// The character URL identifier.
        /// </value>
        public string CharacterUrlIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the partner URL identifier.
        /// </summary>
        /// <value>
        /// The partner URL identifier.
        /// </value>
        public string PartnerUrlIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the date the thread was marked queued.
        /// </summary>
        /// <value>
        /// The date the thread was marked queued.
        /// </value>
        public DateTime? DateMarkedQueued { get; set; }
    }
}
