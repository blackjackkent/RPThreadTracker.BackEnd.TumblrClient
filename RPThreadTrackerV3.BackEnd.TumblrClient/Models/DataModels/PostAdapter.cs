// <copyright file="PostAdapter.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Models.DataModels
{
    using System;
    using System.Globalization;
    using System.Linq;
    using DontPanic.TumblrSharp;
    using DontPanic.TumblrSharp.Client;
    using Interfaces;

    /// <inheritdoc />
    public class PostAdapter : IPostAdapter
    {
        private readonly BasePost _post;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostAdapter"/> class.
        /// </summary>
        /// <param name="post">The post.</param>
        public PostAdapter(BasePost post)
        {
            _post = post;
        }

        /// <inheritdoc />
        public string Id => _post.Id.ToString(CultureInfo.InvariantCulture);

        /// <inheritdoc />
        public DateTime Timestamp => _post.Timestamp;

        /// <inheritdoc />
        public string BlogName => _post.BlogName;

        /// <inheritdoc />
        public string Url => _post.Url;

        /// <inheritdoc />
        public string Title
        {
            get
            {
                var textPost = _post as TextPost;
                var title = textPost?.Title;
                if (title != null && title != string.Empty)
                {
                    return title;
                }
                return textPost?.Summary;
            }
        }

        /// <inheritdoc />
        public BaseNote GetMostRecentRelevantNote(string characterUrlIdentifier, string partnerUrlIdentifier)
        {
            BaseNote mostRecentRelevantNote;
            if (_post.Notes == null)
            {
                return null;
            }
            var notes = _post.Notes.Where(n => n.Type == NoteType.Reblog).OrderByDescending(n => n.Timestamp);
            if (!notes.Any())
            {
                return null;
            }
            if (string.IsNullOrEmpty(partnerUrlIdentifier))
            {
                mostRecentRelevantNote = notes.FirstOrDefault();
            }
            else
            {
                mostRecentRelevantNote = notes.FirstOrDefault(n =>
                    string.Equals(n.BlogName, partnerUrlIdentifier, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(n.BlogName, characterUrlIdentifier, StringComparison.OrdinalIgnoreCase));
            }
            return mostRecentRelevantNote;
        }
    }
}
