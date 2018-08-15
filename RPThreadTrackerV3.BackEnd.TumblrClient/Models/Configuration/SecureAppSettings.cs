// <copyright file="SecureAppSettings.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Models.Configuration
{
    /// <summary>
    /// Wrapper class for application settings that are secure in nature.
    /// </summary>
    public class SecureAppSettings
    {
        /// <summary>
        /// Gets or sets the Tumblr consumer key.
        /// </summary>
        /// <value>
        /// The Tumblr consumer key.
        /// </value>
        public string TumblrConsumerKey { get; set; }

        /// <summary>
        /// Gets or sets the Tumblr consumer secret.
        /// </summary>
        /// <value>
        /// The Tumblr consumer secret.
        /// </value>
        public string TumblrConsumerSecret { get; set; }

        /// <summary>
        /// Gets or sets the Tumblr OAuth token.
        /// </summary>
        /// <value>
        /// The Tumblr OAuth token.
        /// </value>
        public string TumblrOauthToken { get; set; }

        /// <summary>
        /// Gets or sets the Tumblr OAuth secret.
        /// </summary>
        /// <value>
        /// The Tumblr OAuth secret.
        /// </value>
        public string TumblrOauthSecret { get; set; }
    }
}
