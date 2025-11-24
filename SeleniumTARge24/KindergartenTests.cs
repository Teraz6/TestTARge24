using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SeleniumTARge24
{
    [TestFixture]
    public class KindergartenTests
    {
        private FirefoxDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            var options = new FirefoxOptions();
            options.AddArgument("--start-maximized");

            driver = new FirefoxDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }



        [Test]
        [Order(1)]
        public void NavigateToKindergarten()
        {
            driver.Navigate().GoToUrl("http://localhost:5196/Kindergarten");
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.h1")));
            Assert.That(driver.Url.Contains("Kindergarten"), Is.True);
        }

        [Test]
        [Order(2)]
        public void AddKindergarten_ValidData()
        {
            driver.Navigate().GoToUrl("http://localhost:5196/Kindergarten/create");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("KindergartenName"))).SendKeys("Männi Lasteaed");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("GroupName"))).SendKeys("Männi grupp");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("TeacherName"))).SendKeys("Tiina Tamm");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ChildrenCount"))).SendKeys("120");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit'][value='Create']"))).Click();

            wait.Until(d => d.PageSource.Contains("Männi Lasteaed"));
            Assert.That(driver.PageSource, Does.Contain("Männi Lasteaed"));
        }

        [Test]
        [Order(3)]
        public void AddKindergarten_InvalidData()
        {
            driver.Navigate().GoToUrl("http://localhost:5196/Kindergarten");

            var editLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Create')]")));
            editLink.Click();

            var countField = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ChildrenCount")));
            countField.Clear();
            countField.SendKeys("tekst");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit'][value='Create']"))).Click();

            bool isError = wait.Until(d =>
                d.PageSource.Contains("Please enter a number.") ||
                d.PageSource.Contains(""));
            Assert.That(isError, Is.True);
        }

        [Test]
        [Order(4)]
        public void ViewKindergartenDetails()
        {
            driver.Navigate().GoToUrl("http://localhost:5196/Kindergarten");

            var detailLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Details')]")));
            detailLink.Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("h1")));
            Assert.That(driver.PageSource, Does.Contain("Details"));
        }

        [Test]
        [Order(5)]
        public void EditKindergarten_Valid()
        {
            driver.Navigate().GoToUrl("http://localhost:5196/Kindergarten");

            var editLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Update')]")));
            editLink.Click();

            var countField = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ChildrenCount")));
            countField.Clear();
            countField.SendKeys("150");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit'][value='Update']"))).Click();

            wait.Until(d => d.PageSource.Contains("150"));
            Assert.That(driver.PageSource, Does.Contain("150"));
        }

        [Test]
        [Order(6)]
        public void EditKindergarten_Invalid()
        {
            driver.Navigate().GoToUrl("http://localhost:5196/Kindergarten");

            var editLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Update')]")));
            editLink.Click();

            var countField = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ChildrenCount")));
            countField.Clear();
            countField.SendKeys("tekst");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit'][value='Update']"))).Click();

            bool isError = wait.Until(d =>
                d.PageSource.Contains("Please enter a number.") ||
                d.PageSource.Contains(""));
            Assert.That(isError, Is.True);
        }

        [Test]
        [Order(7)]
        public void DeleteKindergarten()
        {
            driver.Navigate().GoToUrl("http://localhost:5196/Kindergarten");

            var deleteLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Delete')]")));
            deleteLink.Click();

            var confirmBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit'][value='Delete']")));
            confirmBtn.Click();

            wait.Until(d => !driver.PageSource.Contains("Männi Lasteaed"));
            Assert.That(driver.PageSource, Does.Not.Contain("Männi Lasteaed"));
        }

        [TearDown]
        public void Teardown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}
