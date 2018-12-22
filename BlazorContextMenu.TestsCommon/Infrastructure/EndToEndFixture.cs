using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit.Abstractions;

namespace BlazorContextMenu.TestsCommon.Infrastructure
{
    public abstract class EndToEndFixture<TStartup> : IDisposable where TStartup : class
    {
        public IWebDriver Browser { get; }

        protected abstract string PathBase { get; }

        public EndToEndFixture()
        {
            _rootUriInitializer = new Lazy<Uri>(() => new Uri(StartAndGetRootUri()));

            var opts = new ChromeOptions();

            // Comment this out if you want to watch or interact with the browser (e.g., for debugging)
            opts.AddArgument("--headless");

            // Log errors
            opts.SetLoggingPreference(LogType.Browser, LogLevel.All);

            // On Windows/Linux, we don't need to set opts.BinaryLocation
            // But for Travis Mac builds we do
            var binaryLocation = Environment.GetEnvironmentVariable("TEST_CHROME_BINARY");
            if (!string.IsNullOrEmpty(binaryLocation))
            {
                opts.BinaryLocation = binaryLocation;
                Console.WriteLine($"Set {nameof(ChromeOptions)}.{nameof(opts.BinaryLocation)} to {binaryLocation}");
            }

            try
            {
                var driver = new RemoteWebDriver(opts);
                Browser = driver;
            }
            catch (WebDriverException ex)
            {
                var message =
                    "Failed to connect to the web driver. Please see the readme and follow the instructions to install selenium." +
                    "Remember to start the web driver with `selenium-standalone start` before running the end-to-end tests.";
                throw new InvalidOperationException(message, ex);
            }
        }

        private IWebHost _host;
        public Uri RootUri => _rootUriInitializer.Value;

        private readonly Lazy<Uri> _rootUriInitializer;

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
        
        private string ContentRoot { get; set; }

        protected IWebHost CreateWebHost()
        {
            //ContentRoot = "..\\..\\..\\..\\BlazorContextMenu.BlazorTestApp.Server\\wwwroot";
            //PathBase = "..\\..\\..\\..\\BlazorContextMenu.RazorComponentsTestApp.Server\\";

            var args = new List<string>
            {
                "--urls", "http://127.0.0.1:0",
                //"--contentroot", ContentRoot,
                "--pathbase", PathBase
            };

            return WebHost.CreateDefaultBuilder(args.ToArray())
                .UseConfiguration(new ConfigurationBuilder()
                    .AddCommandLine(args.ToArray())
                    .Build())
                .UseStartup<TStartup>()
                .Build();
        }

        public void Dispose()
        {
            Browser.Dispose();
            _host?.StopAsync();
        }
    }
}
