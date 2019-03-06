using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using BlazorContextMenu.TestsCommon.Infrastructure;
using BlazorContextMenu.E2ETests.Tests;
using BlazorContextMenu.RazorComponentsTestApp;

namespace BlazorContextMenu.RazorComponentsE2ETests.Tests
{
    public class RazorComponentsTestAppIndexTests : TestAppIndexTests<Startup, RazorComponentsE2EFixture>
    {

        public RazorComponentsTestAppIndexTests(RazorComponentsE2EFixture fixture) 
            : base(fixture)
        {
        }

    }
}
