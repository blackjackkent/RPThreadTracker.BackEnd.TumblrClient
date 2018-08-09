// <copyright file="PostService.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using DontPanic.TumblrSharp;
    using DontPanic.TumblrSharp.Client;
    using DontPanic.TumblrSharp.OAuth;
    using Interfaces;
    using Microsoft.Extensions.Options;
    using Models.Configuration;
    using Models.DataModels;
    using Models.RequestModels;
    using Models.ResponseModels;
    using Polly;

    /// <inheritdoc />
    public class PostService : IPostService
    {
        private readonly IPolicyProvider _policyProvider;
        private readonly AppSettings _config;
        private readonly ITumblrSharpClientWrapper _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostService"/> class.
        /// </summary>
        /// <param name="policyProvider">The policy provider.</param>
        /// <param name="config">The application configuration.</param>
        /// <param name="factory">The Tumblr factory wrapper.</param>
        public PostService(IPolicyProvider policyProvider, IOptions<AppSettings> config, ITumblrSharpFactoryWrapper factory)
        {
            _policyProvider = policyProvider;
            _config = config.Value;
            var oauthToken = _config.Secure.TumblrOauthToken;
            var oauthSecret = _config.Secure.TumblrOauthSecret;
            var consumerKey = _config.Secure.TumblrConsumerKey;
            var consumerSecret = _config.Secure.TumblrConsumerSecret;
            var token = new Token(oauthToken, oauthSecret);
            _client = factory.Create(consumerKey, consumerSecret, token);
        }

        /// <inheritdoc />
        public async Task<ThreadStatusDto> GetPost(ThreadStatusRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PostId) || string.IsNullOrWhiteSpace(request.CharacterUrlIdentifier))
            {
                return null;
            }
            var post = await RetrieveApiDataByPost(request.PostId, request.CharacterUrlIdentifier);
            return new ThreadStatusDto(post, request);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<NewsPostDto>> GetNewsPosts()
        {
            var newsBlog = _config.NewsBlogShortname;
            var result = await RetrieveApiDataByTag("news", newsBlog);
            return result.Select(t => new NewsPostDto(t)).ToList();
        }

        private async Task<IPostAdapter> RetrieveApiDataByPost(string postId, string characterUrlIdentifier)
        {
            return await _policyProvider.WrappedPolicy.ExecuteAsync(
                async (context) =>
                {
                    var parameters = new MethodParameterSet { { "notes_info", true }, { "id", postId } };
                    var posts = await _client.CallApiMethodAsync<Posts>(new BlogMethod(characterUrlIdentifier, "posts/text", _client.GetToken(), HttpMethod.Get, parameters), CancellationToken.None);
                    var result = posts.Result.Select(p => new PostAdapter(p)).ToList();
                    return result.FirstOrDefault();
                },
                new Context("RetrieveApiDataByPost", new Dictionary<string, object>() { { "postId", postId }, { "characterUrlIdentifier", characterUrlIdentifier } }));
        }

        private async Task<IEnumerable<IPostAdapter>> RetrieveApiDataByTag(string tag, string blogUrlIdentifier, int limit = 5)
        {
            var parameters = new MethodParameterSet { { "tag", tag }, { "limit", limit } };
            var posts = await _client.CallApiMethodAsync<Posts>(
                new BlogMethod(blogUrlIdentifier, "posts/text", _client.GetToken(), HttpMethod.Get, parameters),
                CancellationToken.None);
            var result = posts.Result.Select(p => new PostAdapter(p)).ToList();
            return result;
        }
    }
}
