using BlazorContextMenu.E2ETests.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BlazorContextMenu.E2ETests.Tests
{
    public class TestAppTodoListTests : TestBase //, IDisposable
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

        //public void Dispose()
        //{
        //    GoToPage();
        //    WaitUntilLoaded();
        //}

        [Theory]
        [InlineData("list1",MouseButton.Right)]
        [InlineData("list2", MouseButton.Left)]
        public void TodoItemsMenu_TriggerForFirstItemAndSelectCopy_ItemCopied(string listId, MouseButton mouseButton)
        {
            //Arrange
            var expectedItemsCount = 4;
            //Act
            OpenContextMenuAt($"{listId}-0", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-copy"));
            menuItem.Click();

            //Assert
            var list = Browser.FindElement(By.Id(listId));
            var itemsCount = list.FindElements(By.TagName("li")).Count;
            Assert.Equal(expectedItemsCount, itemsCount);
        }

        [Theory]
        [InlineData("list1", MouseButton.Right)]
        [InlineData("list2", MouseButton.Left)]
        public void TodoItemsMenu_TriggerForSecondItemAndSelectDelete_ItemDeleted(string listId, MouseButton mouseButton)
        {
            //Arrange
            var expectedItemsCount = 2;
            //Act
            OpenContextMenuAt($"{listId}-1", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-delete"));
            menuItem.Click();

            //Assert
            var list = Browser.FindElement(By.Id(listId));
            var itemsCount = list.FindElements(By.TagName("li")).Count;
            Assert.Equal(expectedItemsCount, itemsCount);
        }


        [Theory]
        [InlineData("list1", MouseButton.Right)]
        [InlineData("list2", MouseButton.Left)]
        public void TodoItemsMenu_TriggerForFirstItemAndSelectCheck_CheckIsDisabled(string listId, MouseButton mouseButton)
        {
            //Arrange
            var settings = new BlazorContextMenuSettings();
            var expectedClass = settings.DefaultCssSettings.MenuItemDisabledCssClass;

            //Act
            OpenContextMenuAt($"{listId}-0", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-check"));
            var classes = menuItem.GetAttribute("class");

            //Assert
            Assert.Contains(expectedClass, classes);
        }

        [Theory]
        [InlineData("list1", MouseButton.Right)]
        [InlineData("list2", MouseButton.Left)]
        public void TodoItemsMenu_TriggerForSecondItemAndSelectCheck_ItemIsChecked(string listId, MouseButton mouseButton)
        {
            //Arrange
            var expectedCheckedStatus = true;

            //Act
            OpenContextMenuAt($"{listId}-1", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-check"));
            menuItem.Click();
            var list = Browser.FindElement(By.Id(listId));
            var checkBoxes = list.FindElements(By.TagName("input"));
            var secondCheckBox = checkBoxes[1];

            //Assert
            Assert.Equal(expectedCheckedStatus, secondCheckBox.Selected);
        }


        [Theory]
        [InlineData("list1", MouseButton.Right)]
        [InlineData("list2", MouseButton.Left)]
        public void TodoItemsMenu_TriggerForSecondItemAndSelectCheckAndThenTriggerAgain_CheckIsDisabled(string listId, MouseButton mouseButton)
        {
            //Arrange
            var expectedCheckedStatus = true;
            var settings = new BlazorContextMenuSettings();
            var expectedClass = settings.DefaultCssSettings.MenuItemDisabledCssClass;

            //Act
            OpenContextMenuAt($"{listId}-1", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-check"));
            menuItem.Click();
            OpenContextMenuAt($"{listId}-1", mouseButton);
            menuItem = Browser.FindElement(By.Id("menuitem-check"));
            var classes = menuItem.GetAttribute("class");
            var list = Browser.FindElement(By.Id(listId));
            var checkBoxes = list.FindElements(By.TagName("input"));
            var secondBox = checkBoxes[0];


            //Assert
            Assert.Equal(expectedCheckedStatus, secondBox.Selected);
            Assert.Contains(expectedClass, classes);
        }


        [Theory]
        [InlineData("list1", MouseButton.Right)]
        public void TodoItemsMenu_UnckeckFirstItemAndTriggerMenu_CheckIsEnabled(string listId, MouseButton mouseButton)
        {
            //Arrange
            var settings = new BlazorContextMenuSettings();
            var notExpectedClass = settings.DefaultCssSettings.MenuItemDisabledCssClass;

            //Act
            var list = Browser.FindElement(By.Id(listId));
            var checkBoxes = list.FindElements(By.TagName("input"));
            var firstCheckBox = checkBoxes[0];
            firstCheckBox.Click();
            OpenContextMenuAt($"{listId}-0", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-check"));
            var classes = menuItem.GetAttribute("class");

            //Assert
            Assert.DoesNotContain(notExpectedClass, classes);
        }

        [Theory]
        [InlineData("list1", MouseButton.Right)]
        [InlineData("list2", MouseButton.Left)]
        public void TodoItemsMenu_TriggerForFirstItemAndCheckVisibilityOfLastItem_ItemIsInvisible(string listId, MouseButton mouseButton)
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            OpenContextMenuAt($"{listId}-0", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-invisible"));
            var styles = menuItem.GetAttribute("style");

            //Assert
            Assert.Contains(expectedDisplay, styles);
        }

        [Theory]
        [InlineData("list1", MouseButton.Right)]
        [InlineData("list2", MouseButton.Left)]
        public void TodoItemsMenu_TriggerForFirstItemAndCheckVisibilityOfDelete_ItemIsVisible(string listId, MouseButton mouseButton)
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            OpenContextMenuAt($"{listId}-0", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-delete"));
            var styles = menuItem.GetAttribute("style");

            //Assert
            Assert.Contains(expectedDisplay, styles);
        }

        [Theory]
        [InlineData("list1", MouseButton.Right)]
        [InlineData("list2", MouseButton.Left)]
        public void TodoItemsMenu_TriggerForThirdItemAndCheckVisibilityOfDelete_ItemIsInvisible(string listId, MouseButton mouseButton)
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            OpenContextMenuAt($"{listId}-2", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-delete"));
            var styles = menuItem.GetAttribute("style");

            //Assert
            Assert.Contains(expectedDisplay, styles);
        }
    }
}
