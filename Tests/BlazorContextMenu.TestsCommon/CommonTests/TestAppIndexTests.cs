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
        [InlineData("test1-trigger",MouseButtonTrigger.Right)]
        [InlineData("test3-trigger",MouseButtonTrigger.Left)]
        [InlineData("test4-trigger",MouseButtonTrigger.Right)]
        [InlineData("test4-trigger",MouseButtonTrigger.Left)]
        [InlineData("test7-trigger",MouseButtonTrigger.Right)]
        [InlineData("test8-trigger", MouseButtonTrigger.Right)]
        [InlineData("test10-trigger", MouseButtonTrigger.DoubleClick)]
        public async Task Menu1_Triggers_Shown(string triggerId, MouseButtonTrigger mouseButton)
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
        [InlineData("test9-trigger", MouseButtonTrigger.Right)]
        public async Task Menu1_PreventShow_IsNotShown(string triggerId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            await OpenContextMenuAt(triggerId, mouseButton);

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Theory]
        [InlineData("test1-trigger", MouseButtonTrigger.Right)]
        [InlineData("test3-trigger", MouseButtonTrigger.Left)]
        [InlineData("test4-trigger", MouseButtonTrigger.Right)]
        [InlineData("test4-trigger", MouseButtonTrigger.Left)]
        [InlineData("test7-trigger", MouseButtonTrigger.Right)]
        [InlineData("test8-trigger", MouseButtonTrigger.Right)]
        [InlineData("test10-trigger", MouseButtonTrigger.DoubleClick)]
        public async Task Menu1_TriggerAndClickOutside_MenuCloses(string triggerId, MouseButtonTrigger mouseButton)
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
        [InlineData("test1-trigger", MouseButtonTrigger.Right)]
        [InlineData("test3-trigger", MouseButtonTrigger.Left)]
        [InlineData("test4-trigger", MouseButtonTrigger.Right)]
        [InlineData("test4-trigger", MouseButtonTrigger.Left)]
        [InlineData("test7-trigger", MouseButtonTrigger.Right)]
        [InlineData("test8-trigger", MouseButtonTrigger.Right)]
        [InlineData("test10-trigger", MouseButtonTrigger.DoubleClick)]
        public async Task Menu1_SelectFetchData_DataFetched(string triggerId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            await OpenContextMenuAt(triggerId, mouseButton);
            var menuItem = Browser.FindElement(By.Id("menu1-item1"));
            menuItem.Click();
            new WebDriverWait(Browser, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("test1-textarea")));

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu1"));
            var textArea = Browser.FindElement(By.Id("test1-textarea"));
            var display = menuElement.GetCssValue("display");
            var text = textArea.GetAttribute("value");
            Assert.True(!string.IsNullOrWhiteSpace(text));
            Assert.Equal(expectedDisplay, display);
        }

        [Theory]
        [InlineData("test1-trigger", MouseButtonTrigger.Right)]
        [InlineData("test3-trigger", MouseButtonTrigger.Left)]
        [InlineData("test4-trigger", MouseButtonTrigger.Right)]
        [InlineData("test4-trigger", MouseButtonTrigger.Left)]
        [InlineData("test7-trigger", MouseButtonTrigger.Right)]
        [InlineData("test8-trigger", MouseButtonTrigger.Right)]
        [InlineData("test10-trigger", MouseButtonTrigger.DoubleClick)]
        public async Task Menu1_SelectClearData_DataCleared(string triggerId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            await OpenContextMenuAt(triggerId, mouseButton);
            var menuItem = Browser.FindElement(By.Id("menu1-item2"));
            menuItem.Click();
            await Task.Delay(500);

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu1"));
            var textArea = Browser.FindElement(By.Id("test1-textarea"));
            var display = menuElement.GetCssValue("display");
            var textAreaDisplay = textArea.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
            Assert.Equal(expectedDisplay, textAreaDisplay);
        }

        [Theory]
        [InlineData("test1-trigger",MouseButtonTrigger.Right)]
        [InlineData("test3-trigger",MouseButtonTrigger.Left)]
        [InlineData("test4-trigger",MouseButtonTrigger.Right)]
        [InlineData("test4-trigger",MouseButtonTrigger.Left)]
        [InlineData("test7-trigger",MouseButtonTrigger.Right)]
        [InlineData("test8-trigger", MouseButtonTrigger.Right)]
        [InlineData("test10-trigger", MouseButtonTrigger.DoubleClick)]
        public async Task Menu1_SelectDisabledItem_MenuStaysOpen(string triggerId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            await OpenContextMenuAt(triggerId, mouseButton);
            var menuItem = Browser.FindElement(By.Id("menu1-item3"));
            menuItem.Click();
            await Task.Delay(500);

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Fact]
        public async Task Menu1_ManualShow_Shown()
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            var btn = Browser.FindElement(By.Id("showMenuBtn"));
            btn.Click();
            await Task.Delay(500);

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Fact]
        public async Task Menu1_ManualShowAndClickOutside_MenuCloses()
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            var btn = Browser.FindElement(By.Id("showMenuBtn"));
            btn.Click();
            await Task.Delay(500);
            var headerElement = Browser.FindElement(By.Id("header"));
            headerElement.Click();

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Fact]
        public async Task Menu3_ManualShowAndClickOutsideNoAutoClose_MenuStaysOpen()
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            var btn = Browser.FindElement(By.Id("showNoAutoCloseBtn"));
            btn.Click();
            await Task.Delay(500);
            var headerElement = Browser.FindElement(By.Id("header"));
            headerElement.Click();

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu3"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Fact]
        public async Task Menu3_ManualShowAndClickItemNoAutoClose_MenuStaysOpen()
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            var btn = Browser.FindElement(By.Id("showNoAutoCloseBtn"));
            btn.Click();
            await Task.Delay(500);
            var menuItem = Browser.FindElement(By.Id("menu3-item1"));
            menuItem.Click();

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu3"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }


        [Fact]
        public async Task Menu3_ManualShowAndHide_ShownThenHiden()
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            var btn = Browser.FindElement(By.Id("showAndHideMenuBtn"));
            btn.Click();
            await Task.Delay(3000);

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu3"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Theory]
        [InlineData("test2-trigger", MouseButtonTrigger.Right)]
        [InlineData("test5-trigger", MouseButtonTrigger.Left)]
        [InlineData("test6-trigger", MouseButtonTrigger.Right)]
        [InlineData("test6-trigger", MouseButtonTrigger.Left)]
        [InlineData("test11-trigger", MouseButtonTrigger.DoubleClick)]
        public async Task Menu2_MouseOverSubMenu_SubMenuOpens(string triggerId, MouseButtonTrigger mouseButton)
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
        [InlineData("test2-trigger", MouseButtonTrigger.Right)]
        [InlineData("test5-trigger", MouseButtonTrigger.Left)]
        [InlineData("test6-trigger", MouseButtonTrigger.Right)]
        [InlineData("test6-trigger", MouseButtonTrigger.Left)]
        [InlineData("test11-trigger", MouseButtonTrigger.DoubleClick)]
        public async Task Menu2_MouseOverSubMenuAndThenToOtherItem_SubMenuCloses(string triggerId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            await OpenContextMenuAt(triggerId, mouseButton);
            MouseOverElement("submenu1-trigger");
            await Task.Delay(500); //wait for submenu to popup
            MouseOverElement("menu2-item1");
            await Task.Delay(500); //wait for submenu to close

            //Assert
            var menuElement = Browser.FindElement(By.Id("submenu1"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }

        [Theory]
        [InlineData("test2-trigger", MouseButtonTrigger.Right)]
        [InlineData("test5-trigger", MouseButtonTrigger.Left)]
        [InlineData("test6-trigger", MouseButtonTrigger.Right)]
        [InlineData("test6-trigger", MouseButtonTrigger.Left)]
        [InlineData("test11-trigger", MouseButtonTrigger.DoubleClick)]
        public async Task Menu2_MouseOverSubMenuAndThenToSecondSubMenu_SecondSubMenuOpens(string triggerId, MouseButtonTrigger mouseButton)
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

        [Fact]
        public async Task Menu4_TriggerAndClickOutside_MenuStaysOpen()
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            await OpenContextMenuAt("test12-trigger", MouseButtonTrigger.Right);
            var headerElement = Browser.FindElement(By.Id("header"));
            headerElement.Click();

            //Assert
            var menuElement = Browser.FindElement(By.Id("menu4"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }


        [Theory]
        [InlineData("test-propagation-right-child", MouseButtonTrigger.Right)]
        [InlineData("test-propagation-left-child", MouseButtonTrigger.Left)]
        [InlineData("test-propagation-double-child", MouseButtonTrigger.DoubleClick)]
        public async Task Menu1_TriggerNested_Menu1OpensButMenu2DoesNot(string triggerId,MouseButtonTrigger mouseButton)
        {
            //Arrange
            var menu1ExpectedDisplay = "block";
            var menu2ExpectedDisplay = "none";

            //Act
            await OpenContextMenuAt(triggerId, mouseButton);

            //Assert
            var menu1Element = Browser.FindElement(By.Id("menu1"));
            var display1 = menu1Element.GetCssValue("display");
            Assert.Equal(menu1ExpectedDisplay, display1);

            var menu2Element = Browser.FindElement(By.Id("menu2"));
            var display2 = menu2Element.GetCssValue("display");
            Assert.Equal(menu2ExpectedDisplay, display2);

        }
    }
}
