using BlazorContextMenu.E2ETests.Infrastructure;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BlazorContextMenu.E2ETests.Tests
{
    public class TestAppTodoListTests : TestBase, IDisposable
    {
        private readonly EndToEndFixture _fixture;

        public TestAppTodoListTests(EndToEndFixture fixture)
            : base(fixture)
        {
            _fixture = fixture;
            GoToPage();
            WaitUntilLoaded();
        }
        protected void GoToPage()
        {
            Navigate("/TodoList");
        }

        public void Dispose()
        {
            GoToPage();
            WaitUntilLoaded();
        }

        [Fact]
        public void TodoItemsMenu_TriggerForFirstItemAndSelectCopy_ItemCopied()
        {
            //Arrange
            var expectedItemsCount = 3;
            //Act
            OpenContextMenuAt("listitem-0");
            var menuItem = Browser.FindElement(By.Id("menuitem-copy"));
            menuItem.Click();

            //Assert
            var list = Browser.FindElement(By.Id("list"));
            var itemsCount = list.FindElements(By.TagName("li")).Count;
            Assert.Equal(expectedItemsCount, itemsCount);
        }

        [Fact]
        public void TodoItemsMenu_TriggerForSecondItemAndSelectDelete_ItemDeleted()
        {
            //Arrange
            var expectedItemsCount = 1;
            //Act
            OpenContextMenuAt("listitem-1");
            var menuItem = Browser.FindElement(By.Id("menuitem-delete"));
            menuItem.Click();

            //Assert
            var list = Browser.FindElement(By.Id("list"));
            var itemsCount = list.FindElements(By.TagName("li")).Count;
            Assert.Equal(expectedItemsCount, itemsCount);
        }
    }
}
