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

namespace BlazorContextMenu.E2ETests.Tests
{
    public abstract class TestAppIndexTests<TStartup,TFixture> : TestBase<TStartup, TFixture> 
        where TFixture : EndToEndFixture<TStartup>
        where TStartup : class
    {

        public TestAppIndexTests(TFixture fixture) 
            : base(fixture)
        {
            GoToPage();
            WaitUntilLoaded();
        }

        protected void GoToPage()
        {
            Navigate("/");
        }

        [Theory]
        [InlineData("test1-trigger", MouseButton.Right)]
        [InlineData("test3-trigger", MouseButton.Left)]
        [InlineData("test4-trigger", MouseButton.Right)]
        [InlineData("test4-trigger", MouseButton.Left)]
        public async Task Menu1_Triggers_Shown(string triggerId, MouseButton mouseButton)
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            await OpenContextMenuAt(triggerId, mouseButton);

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Theory]
        [InlineData("test1-trigger", MouseButton.Right)]
        [InlineData("test3-trigger", MouseButton.Left)]
        [InlineData("test4-trigger", MouseButton.Right)]
        [InlineData("test4-trigger", MouseButton.Left)]
        public async Task Menu1_TriggerAndClickOutside_MenuCloses(string triggerId, MouseButton mouseButton)
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            await OpenContextMenuAt(triggerId, mouseButton);
            var headerElement = Browser.FindElement(By.Id("header"));
            headerElement.Click();

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Theory]
        [InlineData("test1-trigger", MouseButton.Right)]
        [InlineData("test3-trigger", MouseButton.Left)]
        [InlineData("test4-trigger", MouseButton.Right)]
        [InlineData("test4-trigger", MouseButton.Left)]
        public async Task Menu1_SelectFetchData_DataFetched(string triggerId, MouseButton mouseButton)
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            await OpenContextMenuAt(triggerId, mouseButton);
            var menuItem = Browser.FindElement(By.Id("menu1-item1"));
            menuItem.Click();
            new WebDriverWait(Browser, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementIsVisible(By.Id("test1-textarea")));

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu1"));
            var textArea = Browser.FindElement(By.Id("test1-textarea"));
            var display = menuElement.GetCssValue("display");
            var text = textArea.GetAttribute("value");
            Assert.True(!string.IsNullOrWhiteSpace(text));
            Assert.Equal(expectedDisplay, display);
        }

        [Theory]
        [InlineData("test1-trigger", MouseButton.Right)]
        [InlineData("test3-trigger", MouseButton.Left)]
        [InlineData("test4-trigger", MouseButton.Right)]
        [InlineData("test4-trigger", MouseButton.Left)]
        public async Task Menu1_SelectClearData_DataCleared(string triggerId, MouseButton mouseButton)
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            await OpenContextMenuAt(triggerId, mouseButton);
            var menuItem = Browser.FindElement(By.Id("menu1-item2"));
            menuItem.Click();

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu1"));
            var textArea = Browser.FindElement(By.Id("test1-textarea"));
            var display = menuElement.GetCssValue("display");
            var textAreaDisplay = textArea.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
            Assert.Equal(expectedDisplay, textAreaDisplay);
        }

        [Theory]
        [InlineData("test1-trigger", MouseButton.Right)]
        [InlineData("test3-trigger", MouseButton.Left)]
        [InlineData("test4-trigger", MouseButton.Right)]
        [InlineData("test4-trigger", MouseButton.Left)]
        public async Task Menu1_SelectDisabledItem_MenuStaysOpen(string triggerId, MouseButton mouseButton)
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            await OpenContextMenuAt(triggerId, mouseButton);
            var menuItem = Browser.FindElement(By.Id("menu1-item3"));
            menuItem.Click();

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Theory]
        [InlineData("test2-trigger", MouseButton.Right)]
        [InlineData("test5-trigger", MouseButton.Left)]
        [InlineData("test6-trigger", MouseButton.Right)]
        [InlineData("test6-trigger", MouseButton.Left)]
        public async Task Menu2_MouseOverSubMenu_SubMenuOpens(string triggerId, MouseButton mouseButton)
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            await OpenContextMenuAt(triggerId, mouseButton);
            MouseOverElement("submenu1-trigger");
            await Task.Delay(500); //wait for submenu to popup

            //Assert
            var menuElement = Browser.FindElement(By.Id("submenu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Theory]
        [InlineData("test2-trigger", MouseButton.Right)]
        [InlineData("test5-trigger", MouseButton.Left)]
        [InlineData("test6-trigger", MouseButton.Right)]
        [InlineData("test6-trigger", MouseButton.Left)]
        public async Task Menu2_MouseOverSubMenuAndThenToOtherItem_SubMenuCloses(string triggerId, MouseButton mouseButton)
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            await OpenContextMenuAt(triggerId, mouseButton);
            MouseOverElement("submenu1-trigger");
            await Task.Delay(1000); //wait for submenu to popup
            MouseOverElement("menu2-item1");
            await Task.Delay(1000); //wait for submenu to close

            //Assert
            var menuElement = Browser.FindElement(By.Id("submenu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Theory]
        [InlineData("test2-trigger", MouseButton.Right)]
        [InlineData("test5-trigger", MouseButton.Left)]
        [InlineData("test6-trigger", MouseButton.Right)]
        [InlineData("test6-trigger", MouseButton.Left)]
        public async Task Menu2_MouseOverSubMenuAndThenToSecondSubMenu_SecondSubMenuOpens(string triggerId, MouseButton mouseButton)
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            await OpenContextMenuAt(triggerId, mouseButton);
            MouseOverElement("submenu1-trigger");
            await Task.Delay(500); //wait for submenu to popup
            MouseOverElement("submenu2-trigger");
            await Task.Delay(500); //wait for submenu2 to popup

            //Assert
            var menuElement = Browser.FindElement(By.Id("submenu2"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        //public void Dispose()
        //{
        //    GoToPage();
        //    WaitUntilLoaded();
        //}
    }
}
