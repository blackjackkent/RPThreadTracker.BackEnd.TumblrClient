// <copyright file="ThreadController.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.RequestModels;
    using Models.ResponseModels;

    /// <summary>
    /// Controller class for behavior related to retrieving thread statuses.
    /// </summary>
    [Route("api/[controller]")]
    public class ThreadController : BaseController
    {
        private readonly ILogger<ThreadController> _logger;
        private readonly IPostService _postService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="postService">The post service.</param>
        public ThreadController(ILogger<ThreadController> logger, IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        /// <summary>
        /// Processes a request for the status of a single thread.
        /// </summary>
        /// <returns>
        /// HTTP response containing the results of the request and, if successful,
        /// a <see cref="ThreadStatusDto" /> object in the response body.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful retrieval of thread information</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        /// <param name="postId">The ID of the post.</param>
        /// <param name="characterUrlIdentifier">The shortname of the blog to which the post belongs.</param>
        /// <param name="partnerUrlIdentifier">An optional partner shortname against which the post's last poster should be compared.</param>
        /// <param name="dateMarkedQueued">An optional date against which the post's last post date should be compared.</param>
        [HttpGet]
        public async Task<IActionResult> Get(string postId, string characterUrlIdentifier, string partnerUrlIdentifier = null, DateTime? dateMarkedQueued = null)
        {
            try
            {
                var request = new ThreadStatusRequest
                {
                    PostId = postId,
                    CharacterUrlIdentifier = characterUrlIdentifier,
                    PartnerUrlIdentifier = partnerUrlIdentifier,
                    DateMarkedQueued = dateMarkedQueued
                };
                var post = await _postService.GetPost(request);
                return Ok(post);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error retrieving thread with post ID {postId}: {e.Message}", e);
                return StatusCode(500, "An unknown error occurred.");
            }
        }

        /// <summary>
        /// Processes a request for the status of a collection of threads.
        /// </summary>
        /// <returns>
        /// HTTP response containing the results of the request and, if successful,
        /// a list of <see cref="ThreadStatusDto" /> objects in the response body.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful retrieval of thread information</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        /// <param name="requests">A list of <see cref="ThreadStatusRequest"/> objects representing the threads whose statuses should be retrieved.</param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<ThreadStatusRequest> requests)
        {
            try
            {
                var results = new List<ThreadStatusDto>();
                var downloadTasksQuery = from request in requests select _postService.GetPost(request);
                var downloadTasks = downloadTasksQuery.ToList();
                while (downloadTasks.Count > 0)
                {
                    var firstFinishedTask = await Task.WhenAny(downloadTasks);
                    downloadTasks.Remove(firstFinishedTask);
                    var result = await firstFinishedTask;
                    results.Add(result);
                }
                return Ok(results);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error retrieving a collection of threads: {e.Message}", e);
                return StatusCode(500, "An unknown error occurred.");
            }
        }
    }
}
