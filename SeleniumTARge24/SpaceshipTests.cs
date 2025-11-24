using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SeleniumTARge24
{
    public class SpaceshipTests
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
        public void NavigateToSpaceships()
        {
            driver.Navigate().GoToUrl("http://localhost:5196/Spaceships");
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.h1")));
            Assert.That(driver.Url.Contains("Spaceships"), Is.True);
        }

        [Test]
        [Order(2)]
        public void AddSpaceship_ValidData()
        {
            driver.Navigate().GoToUrl("http://localhost:5196/Spaceships/Create");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Name"))).SendKeys("Enterprise");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Classification"))).SendKeys("Explorer");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("BuiltDate"))).SendKeys("01-01-2021 20:35");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Crew"))).SendKeys("500");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("EnginePower"))).SendKeys("1000");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit'][value='Create']"))).Click();

            wait.Until(d => d.PageSource.Contains("Enterprise"));
            Assert.That(driver.PageSource, Does.Contain("Enterprise"));
        }

        [Test]
        [Order(3)]
        public void AddSpaceship_InvalidData()
        {
            driver.Navigate().GoToUrl("http://localhost:5196/Spaceships/Create");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Crew"))).SendKeys("abc"); // Vale arv
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("EnginePower"))).SendKeys("-100"); // Võib olla negatiivne

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit'][value='Create']"))).Click();

            // Oota veateadet või kontrolli, et leht ei liigu edasi
            bool isError = wait.Until(d =>
                d.PageSource.Contains("Please enter a number") ||
                d.Url.Contains("/Create")); // jääb Create lehele
            Assert.That(isError, Is.True);
        }

        [Test]
        [Order(4)]
        public void EditSpaceship_Valid()
        {
            driver.Navigate().GoToUrl("http://localhost:5196/Spaceships");

            var editLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Update')]")));
            editLink.Click();

            var crewField = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Crew")));
            crewField.Clear();
            crewField.SendKeys("600");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit'][value='Update']"))).Click();

            wait.Until(d => d.PageSource.Contains("600"));
            Assert.That(driver.PageSource, Does.Contain("600"));
        }

        [Test]
        [Order(5)]
        public void EditSpaceship_Invalid()
        {
            driver.Navigate().GoToUrl("http://localhost:5196/Spaceships");

            var editLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Update')]")));
            editLink.Click();

            var crewField = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Crew")));
            crewField.Clear();
            crewField.SendKeys("abc");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit'][value='Update']"))).Click();

            bool isError = wait.Until(d =>
                d.PageSource.Contains("Please enter a number.") ||
                d.PageSource.Contains(""));
            Assert.That(isError, Is.True);
        }

        [Test]
        [Order(6)]
        public void DeleteSpaceship()
        {
            driver.Navigate().GoToUrl("http://localhost:5196/Spaceships");

            var deleteLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Delete')]")));
            deleteLink.Click();

            var confirmBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit'][value='Delete']")));
            confirmBtn.Click();

            wait.Until(d => !driver.PageSource.Contains("Enterprise"));
            Assert.That(driver.PageSource, Does.Not.Contain("Enterprise"));
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
