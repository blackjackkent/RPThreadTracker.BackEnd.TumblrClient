// <copyright file="NewsController.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.ResponseModels;

    /// <summary>
    /// Controller class for behavior related to retrieving thread statuses.
    /// </summary>
    [Route("api/[controller]")]
    public class NewsController : BaseController
    {
        private readonly ILogger<NewsController> _logger;
        private readonly IPostService _postService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="postService">The post service.</param>
        public NewsController(ILogger<NewsController> logger, IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        /// <summary>
        /// Processes a request for the most recent posts from the news blog
        /// </summary>
        /// <returns>
        /// HTTP response containing the results of the request and, if successful,
        /// a collection of <see cref="NewsPostDto" /> objects in the response body.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful retrieval of post information</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var posts = await _postService.GetNewsPosts();
                return Ok(posts);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error retrieving news posts: {e.Message}", e);
                return StatusCode(500, "An unknown error occurred.");
            }
        }
    }
}
