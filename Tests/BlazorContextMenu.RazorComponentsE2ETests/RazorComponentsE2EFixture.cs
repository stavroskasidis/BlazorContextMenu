using BlazorContextMenu.RazorComponentsTestApp.Server;
using BlazorContextMenu.TestsCommon.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BlazorContextMenu.RazorComponentsE2ETests
{
    public class RazorComponentsE2EFixture : EndToEndFixture<Startup>
    {
        protected override string PathBase => "..\\..\\..\\..\\..\\TestApps\\RazorComponentsTestApp\\BlazorContextMenu.RazorComponentsTestApp.Server\\";
    }
}
