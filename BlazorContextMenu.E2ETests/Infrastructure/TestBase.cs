﻿using BlazorContextMenu.E2ETests.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using Xunit;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;
using Xunit.Abstractions;

namespace BlazorContextMenu.E2ETests.Infrastructure
{
    public abstract class TestBase : IClassFixture<EndToEndFixture>
    {
        private readonly EndToEndFixture _fixture;

        private static readonly AsyncLocal<IWebDriver> _browser = new AsyncLocal<IWebDriver>();

        public static IWebDriver Browser => _browser.Value;

        public TestBase(EndToEndFixture fixture)
        {
            _fixture = fixture;
            _browser.Value = fixture.Browser;
        }
        protected void OpenContextMenuAt(string triggerElementId, MouseButton mouseButton)
        {
            var element = Browser.FindElement(By.Id(triggerElementId));
            var action = new Actions(Browser);
            if (mouseButton == MouseButton.Left)
            {
                action.Click(element).Perform();
            }
            else
            {
                action.ContextClick(element).Perform();
            }
        }

        protected void MouseOverElement(string elementId)
        {
            var element = Browser.FindElement(By.Id(elementId));
            var action = new Actions(Browser);
            action.MoveToElement(element).Perform();
        }

        protected void WaitUntilLoaded()
        {
            new WebDriverWait(Browser, TimeSpan.FromSeconds(30)).Until(
                driver => driver.FindElement(By.TagName("app")).Text != "Loading...");
        }

        public void Navigate(string relativeUrl, bool noReload = false)
        {
            var absoluteUrl = new Uri(_fixture.RootUri, relativeUrl);

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
