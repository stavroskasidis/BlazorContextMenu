// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using BlazorContextMenu.E2ETests.Infrastructure;
using System.Threading.Tasks;

namespace BlazorContextMenu.E2ETests.Tests
{
    public class TestAppIndexTests : TestBase, IDisposable
    {
        private readonly EndToEndFixture _fixture;

        public TestAppIndexTests(EndToEndFixture fixture) 
            : base(fixture)
        {
            _fixture = fixture;
            GoToPage();
            WaitUntilLoaded();
        }

        protected void GoToPage()
        {
            Navigate("/");
        }

        [Fact]
        public void Menu1_Trigger_Shown()
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            OpenContextMenuAt("test1-trigger");

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Fact]
        public void Menu1_TriggerAndClickOutside_MenuCloses()
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            OpenContextMenuAt("test1-trigger");
            var headerElement = Browser.FindElement(By.Id("header"));
            headerElement.Click();

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Fact]
        public void Menu1_SelectFetchData_DataFetched()
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            OpenContextMenuAt("test1-trigger");
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

        [Fact]
        public void Menu1_SelectClearData_DataCleared()
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            OpenContextMenuAt("test1-trigger");
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

        [Fact]
        public void Menu1_SelectDisabledItem_MenuStaysOpen()
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            OpenContextMenuAt("test1-trigger");
            var menuItem = Browser.FindElement(By.Id("menu1-item3"));
            menuItem.Click();

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Fact]
        public async Task Menu2_MouseOverSubMenu_SubMenuOpens()
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            OpenContextMenuAt("test2-trigger");
            MouseOverElement("submenu1-trigger");
            await Task.Delay(500); //wait for submenu to popup

            //Assert
            var menuElement = Browser.FindElement(By.Id("submenu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Fact]
        public async Task Menu2_MouseOverSubMenuAndThenToOtherItem_SubMenuCloses()
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            OpenContextMenuAt("test2-trigger");
            MouseOverElement("submenu1-trigger");
            await Task.Delay(500); //wait for submenu to popup
            MouseOverElement("menu2-item1");

            //Assert
            var menuElement = Browser.FindElement(By.Id("submenu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Fact]
        public async Task Menu2_MouseOverSubMenuAndThenToSecondSubMenu_SecondSubMenuOpens()
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            OpenContextMenuAt("test2-trigger");
            MouseOverElement("submenu1-trigger");
            await Task.Delay(500); //wait for submenu to popup
            MouseOverElement("submenu2-trigger");
            await Task.Delay(500); //wait for submenu2 to popup

            //Assert
            var menuElement = Browser.FindElement(By.Id("submenu2"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        public void Dispose()
        {
            GoToPage();
            WaitUntilLoaded();
        }
    }
}
