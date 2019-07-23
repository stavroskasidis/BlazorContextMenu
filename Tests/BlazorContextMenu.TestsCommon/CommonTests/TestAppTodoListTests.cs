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
    public abstract class TestAppTodoListTests<TStartup, TFixture> : TestBase<TStartup, TFixture>
        where TFixture : EndToEndFixture<TStartup>
        where TStartup : class
    {
        public TestAppTodoListTests(TFixture fixture)
            : base(fixture)
        {
            GoToPage();
            WaitUntilLoaded();
        }
        protected void GoToPage()
        {
            Navigate("/TodoList");
        }

        [Theory]
        [InlineData("list1", MouseButtonTrigger.Right)]
        [InlineData("list2", MouseButtonTrigger.Left)]
        public async Task TodoItemsMenu_TriggerForFirstItemAndSelectCopy_ItemCopied(string listId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var expectedItemsCount = 5;
            //Act
            await OpenContextMenuAt($"{listId}-0", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-copy"));
            menuItem.Click();
            await Task.Delay(500);

            //Assert
            var list = Browser.FindElement(By.Id(listId));
            var itemsCount = list.FindElements(By.TagName("li")).Count;
            Assert.Equal(expectedItemsCount, itemsCount);
        }

        [Theory]
        [InlineData("list1", MouseButtonTrigger.Right)]
        [InlineData("list2", MouseButtonTrigger.Left)]
        public async Task TodoItemsMenu_TriggerForSecondItemAndSelectDelete_ItemDeleted(string listId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var expectedItemsCount = 3;
            //Act
            await OpenContextMenuAt($"{listId}-1", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-delete"));
            menuItem.Click();
            await Task.Delay(500);

            //Assert
            var list = Browser.FindElement(By.Id(listId));
            var itemsCount = list.FindElements(By.TagName("li")).Count;
            Assert.Equal(expectedItemsCount, itemsCount);
        }


        [Theory]
        [InlineData("list1", MouseButtonTrigger.Right)]
        [InlineData("list2", MouseButtonTrigger.Left)]
        public async Task TodoItemsMenu_TriggerForFirstItemAndSelectCheck_CheckIsDisabled(string listId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var settings = new BlazorContextMenuDefaultCssSettings();
            var expectedClass = settings.MenuItemDisabledCssClass;

            //Act
            await OpenContextMenuAt($"{listId}-0", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-check"));
            var classes = menuItem.GetAttribute("class");

            //Assert
            Assert.Contains(expectedClass, classes);
        }

        [Theory]
        [InlineData("list1", MouseButtonTrigger.Right)]
        [InlineData("list2", MouseButtonTrigger.Left)]
        public async Task TodoItemsMenu_TriggerForSecondItemAndSelectCheck_ItemIsChecked(string listId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var expectedCheckedStatus = true;

            //Act
            await OpenContextMenuAt($"{listId}-1", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-check"));
            menuItem.Click();
            await Task.Delay(500);

            var list = Browser.FindElement(By.Id(listId));
            var checkBoxes = list.FindElements(By.TagName("input"));
            var secondCheckBox = checkBoxes[1];

            //Assert
            Assert.Equal(expectedCheckedStatus, secondCheckBox.Selected);
        }


        [Theory]
        [InlineData("list1", MouseButtonTrigger.Right)]
        [InlineData("list2", MouseButtonTrigger.Left)]
        public async Task TodoItemsMenu_TriggerForSecondItemAndSelectCheckAndThenTriggerAgain_CheckIsDisabled(string listId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var expectedCheckedStatus = true;
            var settings = new BlazorContextMenuDefaultCssSettings();
            var expectedClass = settings.MenuItemDisabledCssClass;

            //Act
            await OpenContextMenuAt($"{listId}-1", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-check"));
            menuItem.Click();
            await Task.Delay(1000);
            await OpenContextMenuAt($"{listId}-1", mouseButton);
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
        [InlineData("list1", MouseButtonTrigger.Right)]
        public async Task TodoItemsMenu_UnckeckFirstItemAndTriggerMenu_CheckIsEnabled(string listId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var settings = new BlazorContextMenuDefaultCssSettings();
            var notExpectedClass = settings.MenuItemDisabledCssClass;

            //Act
            var list = Browser.FindElement(By.Id(listId));
            var checkBoxes = list.FindElements(By.TagName("input"));
            var firstCheckBox = checkBoxes[0];
            firstCheckBox.Click();
            await OpenContextMenuAt($"{listId}-0", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-check"));
            var classes = menuItem.GetAttribute("class");

            //Assert
            Assert.DoesNotContain(notExpectedClass, classes);
        }

        [Theory]
        [InlineData("list1", MouseButtonTrigger.Right)]
        [InlineData("list2", MouseButtonTrigger.Left)]
        public async Task TodoItemsMenu_TriggerForFirstItemAndCheckVisibilityOfLastItem_ItemIsInvisible(string listId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            await OpenContextMenuAt($"{listId}-0", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-invisible"));
            var styles = menuItem.GetAttribute("style");

            //Assert
            Assert.Contains(expectedDisplay, styles);
        }

        [Theory]
        [InlineData("list1", MouseButtonTrigger.Right)]
        [InlineData("list2", MouseButtonTrigger.Left)]
        public async Task TodoItemsMenu_TriggerForFirstItemAndCheckVisibilityOfDelete_ItemIsVisible(string listId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var expectedDisplay = "block";

            //Act
            await OpenContextMenuAt($"{listId}-0", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-delete"));
            var styles = menuItem.GetAttribute("style");

            //Assert
            Assert.Contains(expectedDisplay, styles);
        }

        [Theory]
        [InlineData("list1", MouseButtonTrigger.Right)]
        [InlineData("list2", MouseButtonTrigger.Left)]
        public async Task TodoItemsMenu_TriggerForThirdItemAndCheckVisibilityOfDelete_ItemIsInvisible(string listId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            await OpenContextMenuAt($"{listId}-2", mouseButton);
            var menuItem = Browser.FindElement(By.Id("menuitem-delete"));
            var styles = menuItem.GetAttribute("style");

            //Assert
            Assert.Contains(expectedDisplay, styles);
        }

        [Theory]
        [InlineData("list1", MouseButtonTrigger.Right)]
        [InlineData("list2", MouseButtonTrigger.Left)]
        public async Task TodoItemsMenu_TriggerForFourthItem_MenuIsNotShown(string listId, MouseButtonTrigger mouseButton)
        {
            //Arrange
            var expectedDisplay = "none";

            //Act
            await OpenContextMenuAt($"{listId}-3", mouseButton);

            //Assert
            var menuElement = Browser.FindElement(By.Id("todoItemsMenu"));
            var display = menuElement.GetCssValue("display");
            Assert.Equal(expectedDisplay, display);
        }
    }
}
