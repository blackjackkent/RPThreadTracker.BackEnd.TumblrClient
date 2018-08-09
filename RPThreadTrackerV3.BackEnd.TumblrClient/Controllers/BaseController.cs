// <copyright file="BaseController.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient.Controllers
{
    using Infrastructure.Providers;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Base controller class from which all API controllers inherit.
    /// Provides hook for <see cref="GlobalExceptionHandlerAttribute"/>.
    /// </summary>
    [ServiceFilter(typeof(GlobalExceptionHandlerAttribute))]
    public class BaseController : Controller
    {
    }
}
