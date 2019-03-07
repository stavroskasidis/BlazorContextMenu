using BlazorContextMenu.BlazorE2ETests;
using BlazorContextMenu.BlazorTestApp.Server;
using BlazorContextMenu.TestsCommon.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlazorContextMenu.E2ETests.Tests
{
    public class BlazorTestAppTodoListTests : TestAppTodoListTests<Startup, BlazorE2EFixture> //, IDisposable
    {
        public BlazorTestAppTodoListTests(BlazorE2EFixture fixture)
            : base(fixture)
        {
        }
    }
}
