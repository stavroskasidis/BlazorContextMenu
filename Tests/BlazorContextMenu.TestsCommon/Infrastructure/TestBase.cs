using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using Xunit;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;
using Xunit.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu.TestsCommon.Infrastructure
{
    public abstract class TestBase<TStartup, TFixture> : IClassFixture<TFixture>
        where TFixture : EndToEndFixture<TStartup> 
        where TStartup : class
    {
        private readonly TFixture _fixture;

        private static readonly AsyncLocal<IWebDriver> _browser = new AsyncLocal<IWebDriver>();

        public static IWebDriver Browser => _browser.Value;

        public TestBase(TFixture fixture)
        {
            _fixture = fixture;
            _browser.Value = fixture.Browser;
        }
        protected async Task OpenContextMenuAt(string triggerElementId, MouseButtonTrigger mouseButton)
        {
            var element = Browser.FindElement(By.Id(triggerElementId));
            var action = new Actions(Browser);
            switch (mouseButton)
            {
                case MouseButtonTrigger.Left:
                    action.Click(element).Perform();
                    break;
                case MouseButtonTrigger.DoubleClick:
                    action.DoubleClick(element).Perform();
                    break;
                case MouseButtonTrigger.Right:
                    action.ContextClick(element).Perform();
                    break;
            }
            

            await Task.Delay(500);
        }

        protected void MouseOverElement(string elementId)
        {
            var element = Browser.FindElement(By.Id(elementId));
            var action = new Actions(Browser);
            action.MoveToElement(element).Perform();
        }

        protected void WaitUntilLoaded()
        {
            Browser.Manage().Window.Maximize();
            new WebDriverWait(Browser, TimeSpan.FromSeconds(30)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("app-loaded")));
            Task.Delay(1000).Wait();
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
