using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KitchenSink.Tests;

namespace KitchenSink.Test {

    [TestFixture("firefox")]
    [TestFixture("chrome")]
    [TestFixture("edge")]
    public class ButtonPageTest : BaseTest {

        public ButtonPageTest(string browser) : base(browser) { }

        public By ButtonAddCarrotsInlineScript {
            get {
                return By.XPath("(//p[@class='kitchensink-add-carrots']/button)[1]");
            }
        }

        public By ButtonAddCarrotsFunction {
            get {
                return By.XPath("(//p[@class='kitchensink-add-carrots']/button)[2]");
            }
        }

        public By SpanAddCarrotsFunction {
            get {
                return By.XPath("(//p[@class='kitchensink-add-carrots']/span)[2]");
            }
        }

        public By AddCarrotsReaction {
            get {
                return By.XPath("(//div[@class='kitchensink-layout__column-right']/starcounter-include/pre)[1]");
            }
        }

        public By ButtonSwitch {
            get {
                return By.XPath("(//div[@class='kitchensink-layout__column-right']/starcounter-include//button)[3]");
            }
        }

        public By ButtonSwitchReaction {
            get {
                return By.XPath("(//div[@class='kitchensink-layout__column-right']/starcounter-include//button)[3]/following-sibling::span[1]");
            }
        }

        public By ButtonDisabled {
            get {
                return By.XPath("(//div[@class='kitchensink-layout__column-right']/starcounter-include//button)[4]");
            }
        }

        public By ButtonDisabledReaction {
            get {
                return By.XPath("(//div[@class='kitchensink-layout__column-right']/starcounter-include//button)[4]/following-sibling::span[1]");
            }
        }

        [Test]
        public void ButtonPageTest_PageLoads() {
            driver.Navigate().GoToUrl(baseURL);
            this.WaitUntil(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.LinkText("Button")));
            driver.FindElement(By.LinkText("Button")).ClickUsingMouse(driver);
            this.WaitUntil(ExpectedConditions.PresenceOfAllElementsLocatedBy(ButtonAddCarrotsInlineScript));
            var element = driver.FindElement(ButtonAddCarrotsInlineScript);
            Assert.AreEqual(element.Text.ToLower(), "button (inline script)");
        }

        [Test]
        public void ButtonPageTest_AddCarrotsIncrements() {
            driver.Navigate().GoToUrl(baseURL + "/Button");
            this.WaitUntil(ExpectedConditions.PresenceOfAllElementsLocatedBy(ButtonAddCarrotsInlineScript));
            var label = driver.FindElement(AddCarrotsReaction);
            
            Assert.AreEqual("You don't have any carrots", label.Text);
            var originalText = label.Text;

            Click(driver.FindElement(ButtonAddCarrotsInlineScript));
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("You have 1 imaginary carrots", label.Text);
            originalText = label.Text;

            Click(driver.FindElement(ButtonAddCarrotsFunction));
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("You have 2 imaginary carrots", label.Text);
            originalText = label.Text;

            Click(driver.FindElement(SpanAddCarrotsFunction));
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("You have 3 imaginary carrots", label.Text);
        }

        [Test]
        public void ButtonPageTest_SwitchButtonToggles() {
            driver.Navigate().GoToUrl(baseURL + "/Button");
            this.WaitUntil(ExpectedConditions.PresenceOfAllElementsLocatedBy(ButtonSwitch));
            var button = driver.FindElement(ButtonSwitch);
            var label = driver.FindElement(ButtonSwitchReaction);

            Assert.AreEqual("Carrot engine is off", label.Text);
            var originalText = label.Text;

            Click(button);
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("Carrot engine is on", label.Text);
            originalText = label.Text;

            Click(button);
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("Carrot engine is off", label.Text);
        }

        [Test]
        public void ButtonPageTest_DisabledButton() {
            driver.Navigate().GoToUrl(baseURL + "/Button");
            this.WaitUntil(ExpectedConditions.PresenceOfAllElementsLocatedBy(ButtonDisabled));
            this.WaitUntil(ExpectedConditions.PresenceOfAllElementsLocatedBy(ButtonDisabledReaction));
            var button = driver.FindElement(ButtonDisabled);
            var label = driver.FindElement(ButtonDisabledReaction);

            Assert.AreEqual("You don't have any carrots", label.Text);
            Assert.AreEqual(button.GetAttribute("disabled"), null);
            var originalText = label.Text;

            Click(button);
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("You have 1 imaginary carrots", label.Text);
            Assert.AreEqual(button.GetAttribute("disabled"), "true");

            Click(button);
            Thread.Sleep(1000);
            Assert.AreEqual("You have 1 imaginary carrots", label.Text);
            Assert.AreEqual(button.GetAttribute("disabled"), "true");
        }

        private void Click(IWebElement element) {
            element.ClickUsingMouse(driver);
        }
    }
}
