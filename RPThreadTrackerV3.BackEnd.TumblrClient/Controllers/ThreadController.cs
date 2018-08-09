using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Controllers
{
    [Route("api/[controller]")]
    public class ThreadController : BaseController
    {
        private readonly ILogger<ThreadController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mapper">The mapper.</param>
        public ThreadController(ILogger<ThreadController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
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
        public IActionResult Get(string postId, string characterUrlIdentifier, string partnerUrlIdentifier = null,
            DateTime? dateMarkedQueued = null)
        {
            try
            {
                _logger.LogError($"Testing tumblr client logging");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error retrieiving thread with {postId}: {e.Message}", e);
                return StatusCode(500, "An unknown error occurred.");
            }
        }
    }
}
