// <copyright file="AppSettings.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Models.Configuration
{
    /// <summary>
    /// Wrapper class for application settings.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the blog shortname from which news items should be retrieved.
        /// </summary>
        /// <value>
        /// The ashortname of the news blog.
        /// </value>
        public string NewsBlogShortname { get; set; }

        /// <summary>
        /// Gets or sets the application settings related to cross-origin requests.
        /// </summary>
        /// <value>
        /// The application settings related to cross-origin requests.
        /// </value>
        public CorsAppSettings Cors { get; set; }

        /// <summary>
        /// Gets or sets the application settings that are secure in nature.
        /// </summary>
        /// <value>
        /// The application settings that are secure in nature.
        /// </value>
        public SecureAppSettings Secure { get; set; }
    }
}
