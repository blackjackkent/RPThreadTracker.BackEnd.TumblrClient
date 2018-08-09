// <copyright file="Program.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using Microsoft.Extensions.Logging;

namespace RPThreadTrackerV3.BackEnd.TumblrClient
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using NLog.Web;

    /// <summary>
    /// Base application bootstrapping file.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The application arguments.</param>
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("Initializing RPThreadTrackerV3.BackEnd.TumblrClient");
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped RPThreadTrackerV3.BackEnd.TumblrClient");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        /// <summary>
        /// Builds the web host.
        /// </summary>
        /// <param name="args">The application arguments.</param>
        /// <returns>Web host instance.</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    var env = builderContext.HostingEnvironment;
                    config.Sources.Clear();
                    config
                        .AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile("appsettings.secure.json", true, true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                        .AddEnvironmentVariables();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
                .Build();
    }
}
