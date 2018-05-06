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
            var expectedItemsCount = 4;
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
            var expectedItemsCount = 2;
            //Act
            OpenContextMenuAt("listitem-1");
            var menuItem = Browser.FindElement(By.Id("menuitem-delete"));
            menuItem.Click();

            //Assert
            var list = Browser.FindElement(By.Id("list"));
            var itemsCount = list.FindElements(By.TagName("li")).Count;
            Assert.Equal(expectedItemsCount, itemsCount);
        }


        [Fact]
        public void TodoItemsMenu_TriggerForFirstItemAndSelectCheck_CheckIsDisabled()
        {
            //Arrange
            var expectedClass= BlazorContextMenuDefaults.DefaultMenuItemDisabledCssClass;

            //Act
            OpenContextMenuAt("listitem-0");
            var menuItem = Browser.FindElement(By.Id("menuitem-check"));
            var classes = menuItem.GetAttribute("class");

            //Assert
            Assert.Contains(expectedClass, classes);
        }

        [Fact]
        public void TodoItemsMenu_TriggerForSecondItemAndSelectCheck_ItemIsChecked()
        {
            //Arrange
            var expectedCheckedStatus = true;

            //Act
            OpenContextMenuAt("listitem-1");
            var menuItem = Browser.FindElement(By.Id("menuitem-check"));
            menuItem.Click();
            var list = Browser.FindElement(By.Id("list"));
            var checkBoxes = list.FindElements(By.TagName("input"));
            var secondCheckBox = checkBoxes[1];

            //Assert
            Assert.Equal(expectedCheckedStatus,secondCheckBox.Selected);
        }


        [Fact]
        public void TodoItemsMenu_TriggerForSecondItemAndSelectCheckAndThenTriggerAgain_CheckIsDisabled()
        {
            //Arrange
            var expectedCheckedStatus = true;
            var expectedClass = BlazorContextMenuDefaults.DefaultMenuItemDisabledCssClass;

            //Act
            OpenContextMenuAt("listitem-1");
            var menuItem = Browser.FindElement(By.Id("menuitem-check"));
            menuItem.Click();
            OpenContextMenuAt("listitem-1");
            menuItem = Browser.FindElement(By.Id("menuitem-check"));
            var classes = menuItem.GetAttribute("class");
            var list = Browser.FindElement(By.Id("list"));
            var checkBoxes = list.FindElements(By.TagName("input"));
            var secondBox = checkBoxes[0];


            //Assert
            Assert.Equal(expectedCheckedStatus, secondBox.Selected);
            Assert.Contains(expectedClass, classes);
        }


        [Fact]
        public void TodoItemsMenu_UnckeckFirstItemAndTriggerMenu_CheckIsEnabled()
        {
            //Arrange
            var notExpectedClass = BlazorContextMenuDefaults.DefaultMenuItemDisabledCssClass;

            //Act
            var list = Browser.FindElement(By.Id("list"));
            var checkBoxes = list.FindElements(By.TagName("input"));
            var firstCheckBox = checkBoxes[0];
            firstCheckBox.Click();
            OpenContextMenuAt("listitem-0");
            var menuItem = Browser.FindElement(By.Id("menuitem-check"));
            var classes = menuItem.GetAttribute("class");

            //Assert
            Assert.DoesNotContain(notExpectedClass, classes);
        }
    }
}
