// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using BlazorContextMenu.TestsCommon.Infrastructure;
using BlazorContextMenu.BlazorE2ETests;
using BlazorContextMenu.BlazorTestApp.Server;

namespace BlazorContextMenu.E2ETests.Tests
{
    public class BlazorTestAppIndexTests : TestAppIndexTests<Startup, BlazorE2EFixture>
    {

        public BlazorTestAppIndexTests(BlazorE2EFixture fixture) 
            : base(fixture)
        {
        }

    }
}
