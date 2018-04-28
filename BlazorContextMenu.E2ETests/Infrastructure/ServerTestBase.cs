using Microsoft.AspNetCore.Blazor.E2ETest.Infrastructure.ServerFixtures;
using OpenQA.Selenium;
using System;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Blazor.E2ETest.Infrastructure
{
    public abstract class ServerTestBase : IClassFixture<BrowserFixture> , IClassFixture<DevHostServerFixture>
    {
        private readonly DevHostServerFixture _serverFixture;

        private static readonly AsyncLocal<IWebDriver> _browser = new AsyncLocal<IWebDriver>();

        public static IWebDriver Browser => _browser.Value;

        public ServerTestBase(BrowserFixture browserFixture, DevHostServerFixture serverFixture)
        {
            _browser.Value = browserFixture.Browser;
            _serverFixture = serverFixture;
        }

        public void Navigate(string relativeUrl, bool noReload = false)
        {
            var absoluteUrl = new Uri(_serverFixture.RootUri, relativeUrl);

            if (noReload)
            {
                var existingUrl = Browser.Url;
                if (string.Equals(existingUrl, absoluteUrl.AbsoluteUri, StringComparison.Ordinal))
                {
                    return;
                }
            }

            Browser.Navigate().GoToUrl(absoluteUrl);
        }
    }
}
