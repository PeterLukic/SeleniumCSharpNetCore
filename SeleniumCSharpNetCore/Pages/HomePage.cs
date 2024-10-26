using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace SeleniumCSharpNetCore.Pages
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public HomePage(IWebDriver driver)
        {
            _driver = driver;
            // Set up an explicit wait with a 10-second timeout
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        // Replace ExpectedConditions with custom wait methods
        private IWebElement WaitUntilClickable(By by)
        {
            return _wait.Until(driver =>
            {
                var element = driver.FindElement(by);
                return element.Displayed && element.Enabled ? element : null;
            });
        }

        private IWebElement WaitUntilVisible(By by)
        {
            return _wait.Until(driver =>
            {
                var element = driver.FindElement(by);
                return element.Displayed ? element : null;
            });
        }

        private IWebElement LnkLogin => WaitUntilClickable(By.LinkText("Login"));
        private IWebElement LnkLoginFailed => WaitUntilClickable(By.LinkText("Login111"));
        private IWebElement LnkLogOff => WaitUntilVisible(By.LinkText("Log off"));

        public void ClickLogin()
        {
            try
            {
                LnkLogin.Click();
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Login link not found.");
            }
            catch (ElementClickInterceptedException)
            {
                Console.WriteLine("Login link was not clickable.");
            }
        }

        public void ClickLoginFailed()
        {
            try
            {
                LnkLoginFailed.Click();
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Login failed link not found.");
            }
            catch (ElementClickInterceptedException)
            {
                Console.WriteLine("Login failed link was not clickable.");
            }
        }

        public bool IsLogOffExist()
        {
            try
            {
                return LnkLogOff.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false; // Log off button doesn't exist
            }
        }
    }
}

