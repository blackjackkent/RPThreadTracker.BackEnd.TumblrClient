﻿// <copyright file="Startup.cs" company="Rosalind Wills">
// Copyright (c) Rosalind Wills. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace RPThreadTrackerV3.BackEnd.TumblrClient
{
    using System.Diagnostics.CodeAnalysis;
    using DontPanic.TumblrSharp;
    using Infrastructure.Providers;
    using Infrastructure.Services;
    using Interfaces;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Models.Configuration;
    using NLog;

    /// <summary>
    /// .NET Core application startup class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configures services and dependency injection for the application container.
        /// </summary>
        /// <param name="services">The application service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<AppSettings>(Configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<GlobalExceptionHandlerAttribute>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPolicyProvider, PolicyProvider>();
            services.AddScoped<ITumblrClientFactory, TumblrClientFactory>();
            services.AddScoped<ITumblrSharpFactoryWrapper, TumblrSharpFactoryWrapper>();
            services.AddScoped<IPolicyUtilityProvider, PolicyUtilityProvider>();
            services.AddCors();
            services.AddMvc();
        }

        /// <summary>
        /// Configures the application's HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAuthentication();
            app.UseCors(builder =>
                builder.WithOrigins(Configuration["Cors:CorsUrl"].Split(',')).AllowAnyHeader().AllowAnyMethod());
            app.UseMvc();
            LogManager.Configuration.Variables["connectionString"] = Configuration.GetConnectionString("Database");
        }
    }
}
