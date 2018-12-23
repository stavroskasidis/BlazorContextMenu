using BlazorContextMenu.BlazorTestApp.Server;
using BlazorContextMenu.TestsCommon.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BlazorContextMenu.BlazorE2ETests
{
    public class BlazorE2EFixture : EndToEndFixture<Startup>
    {
        protected override string PathBase => "..\\..\\..\\..\\BlazorContextMenu.BlazorTestApp.Server\\";
    }
}
