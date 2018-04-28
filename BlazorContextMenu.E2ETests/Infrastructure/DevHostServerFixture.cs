// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using BlazorContextMenu.TestApp.Server;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Microsoft.AspNetCore.Blazor.E2ETest.Infrastructure.ServerFixtures
{
    public class DevHostServerFixture : IDisposable
    {
        private IWebHost _host;
        public Uri RootUri => _rootUriInitializer.Value;

        private readonly Lazy<Uri> _rootUriInitializer;

        public DevHostServerFixture()
        {
            _rootUriInitializer = new Lazy<Uri>(() => new Uri(StartAndGetRootUri()));
        }
        
        protected static void RunInBackgroundThread(Action action)
        {
            var isDone = new ManualResetEvent(false);

            new Thread(() =>
            {
                action();
                isDone.Set();
            }).Start();

            isDone.WaitOne();
        }

        protected string StartAndGetRootUri()
        {
            _host = CreateWebHost();
            RunInBackgroundThread(_host.Start);
            return _host.ServerFeatures
                .Get<IServerAddressesFeature>()
                .Addresses.Single();
        }

        public void Dispose()
        {
            _host?.StopAsync();
        }
        
        private string PathBase { get; set; }
        private string ContentRoot { get; set; }

        protected IWebHost CreateWebHost()
        {
            ContentRoot = "..\\..\\..\\..\\BlazorContextMenu.TestApp.Server\\wwwroot";
            PathBase = "..\\..\\..\\..\\BlazorContextMenu.TestApp.Server\\";

            var args = new List<string>
            {
                "--urls", "http://127.0.0.1:0",
                "--contentroot", ContentRoot,
                "--pathbase", PathBase
            };

            return WebHost.CreateDefaultBuilder(args.ToArray())
                .UseConfiguration(new ConfigurationBuilder()
                    .AddCommandLine(args.ToArray())
                    .Build())
                .UseStartup<Startup>()
                .Build();
        }
    }
}
