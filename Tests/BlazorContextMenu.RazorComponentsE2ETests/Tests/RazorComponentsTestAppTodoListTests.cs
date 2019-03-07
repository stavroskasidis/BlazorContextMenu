using BlazorContextMenu.E2ETests.Tests;
using BlazorContextMenu.RazorComponentsTestApp.Server;
using BlazorContextMenu.TestsCommon.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlazorContextMenu.RazorComponentsE2ETests.Tests
{
    public class RazorComponentsTestAppTodoListTests : TestAppTodoListTests<Startup, RazorComponentsE2EFixture> //, IDisposable
    {
        public RazorComponentsTestAppTodoListTests(RazorComponentsE2EFixture fixture)
            : base(fixture)
        {
        }
    }
}
